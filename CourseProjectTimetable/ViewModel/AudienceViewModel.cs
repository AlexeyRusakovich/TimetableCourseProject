using CourseProject.Models;
using CourseProjectTimetable.ViewModel.Commands;
using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections.Specialized;
using System.Data.Entity;

namespace CourseProjectTimetable.ViewModel
{
    public class AudienceViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public AudienceViewModel()
        {
            this.context = new TimetableCourseProject();
            context.Audience.Load();
            AudienceNumberDatabase = context.Audience.Local;
            AudienceNumber = new ObservableCollection<Audience>(AudienceNumberDatabase);
            audienceModel = new AudienceModel();
        }
            

        #region Properties

        #region Other       
        private ObservableCollection<Audience> AudienceNumberDatabase { get; set; }

        private ObservableCollection<Audience> audienceNumber;
        public ObservableCollection<Audience> AudienceNumber
        {
            get { return audienceNumber; }
            set
            {
                audienceNumber = value;
                OnPropertyChanged();
            }
        }

        private TimetableCourseProject context;

        private AudienceModel audienceModel;

        private Audience changedAudience;

        private Audience currentDataGridItem;
        public Audience CurrentDataGridItem
        {
            set
            {
                currentDataGridItem = value;
                OnPropertyChanged();
                ChangeNumber = currentDataGridItem.Number.Substring(0, currentDataGridItem.Number.IndexOf('-'));
                ChangeCorps = currentDataGridItem.Number.Substring(currentDataGridItem.Number.IndexOf('-') + 1, 1);
                ChangeCapacity = currentDataGridItem.Capacity.ToString();
            }
        }

        Regex regexAuditorium = new Regex(@"^\d{1,3}[аб]?$");

        private Visibility cancelVisibility = Visibility.Collapsed;
        public Visibility CancelVisibility
        {
            get { return cancelVisibility; }
            set
            {
                cancelVisibility = value;
                OnPropertyChanged();
            }
        }

        private object editButtonContent = "Изменить";
        public object EditButtonContent
        {
            get { return editButtonContent; }
            set
            {
                editButtonContent = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region Add

        public string AddErrors;
        public string AddNumber { get; set; }
        public string AddCorps { get; set; }
        public string AddName
        {
            get
            {
                return AddNumber + '-' + AddCorps;
            }
        }
        public string AddCapacity { get; set; }

        #endregion

        #region Change

        private string ChangeErrors;

        private string changeNumber;
        private string changeCorps;
        private string changeCapacity;

        public string ChangeNumber
        {
            get { return changeNumber; }
            set
            {
                changeNumber = value;
                OnPropertyChanged();
            }
        }
        public string ChangeCorps
        {
            get { return changeCorps; }
            set
            {
                changeCorps = value;
                OnPropertyChanged();
            }
        }
        public string ChangeName
        {
            get
            {
                return changeNumber + '-' + changeCorps;
            }
        }
        public string ChangeCapacity
        {
            get { return changeCapacity; }
            set
            {
                changeCapacity = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Search

        private string searchNumber;
        private string searchCorps;
        private string searchCapacity;

        public string SearchNumber
        {
            get { return searchNumber; }
            set
            {
                searchNumber = value;
                OnPropertyChanged();
                FilterAudience();
            }
        }
        public string SearchCorps
        {
            get { return searchCorps; }
            set
            {
                searchCorps = value;
                OnPropertyChanged();
                FilterAudience();
            }
        }
        public string SearchCapacity
        {
            get { return searchCapacity; }
            set
            {
                searchCapacity = value;
                OnPropertyChanged();
                FilterAudience();
            }
        }

        #endregion

        #endregion

        #region Commands

        private Command audienceAdd;
        public Command AudienceAdd
        {
            get
            {
                if (audienceAdd == null)
                    audienceAdd = new Command(AudienceAdd_Method);
                return audienceAdd;
            }
        }

        private Command audienceRemove;
        public Command AudienceRemove
        {
            get
            {
                if (audienceRemove == null)
                    audienceRemove = new Command(AudienceRemove_Method);
                return audienceRemove;
            }
        }

        private Command audienceChange;
        public Command AudienceChange
        {
            get
            {
                if (audienceChange == null)
                    audienceChange = new Command(AudienceChange_Method);
                return audienceChange;
            }
        }

        private Command audienceCancel;
        public Command AudienceCancel
        {
            get
            {
                if (audienceCancel == null)
                    audienceCancel = new Command(AudienceCancel_Method);
                return audienceCancel;
            }
        }
        #endregion

        #region Methods

        private Command filter;

        public Command Filter
        {
            get
            {
                if (filter == null)
                        filter = new Command(filterHandler);
                return filter;
            }
        }

        private void filterHandler(object obj)
        {
            FilterAudience();
        }

        private async void FilterAudience()
        {
            TimetableCourseProject Context = new TimetableCourseProject();
            AudienceNumberDatabase = new ObservableCollection<Audience>(await Context.Audience.ToListAsync());

            context.Audience.Load();
            if (AudienceNumber != null)
            {
                AudienceNumber.Clear();
                if (AudienceNumberDatabase.Count != 0)
                    foreach (var obj in AudienceNumberDatabase)
                        if (FilterAudience(obj))
                            AudienceNumber.Add(obj);
            }
        }

        public bool FilterAudience(object obj)
        {
            if (obj is Audience audience)
            {
                if (!String.IsNullOrWhiteSpace(searchNumber) && !audience.Number.Substring(0, audience.Number.IndexOf('-')).Contains(searchNumber))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchCorps) && !audience.Number.Substring(audience.Number.IndexOf('-') + 1).Contains(searchCorps))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchCapacity) && !audience.Capacity.ToString().Contains(searchCapacity))
                    return false;
            }
            return true;
        }

        public void AudienceAdd_Method(object paramert)
        {
            if (IsValid(ValidatesAddProperties, out AddErrors))
            {
                MessageBox.Show(audienceModel.Add(AudienceModel.GetAudienceObject(AddName, int.Parse(AddCapacity))), "Результат добавления");
                FilterAudience();
            }
            else
                MessageBox.Show("Заполните поля корректно!", "Результат добавления");
        }

        private void AudienceChange_Method(object parametr)
        {
            if(editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    if (audienceModel.IsExist(AudienceModel.GetAudienceObject(ChangeName, int.Parse(changeCapacity))))
                    {
                        EditButtonContent = "Сохранить";
                        CancelVisibility = Visibility.Visible;
                        changedAudience = AudienceModel.GetAudienceObject(ChangeName, int.Parse(changeCapacity));
                        return;
                    }
                    else
                        MessageBox.Show("Изменяемого объекта не существует", "Результат изменения");
                    
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат Изменения");
                
            }
            else if(editButtonContent.Equals("Сохранить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    MessageBox.Show(audienceModel.Change(changedAudience, AudienceModel.GetAudienceObject(ChangeName, int.Parse(changeCapacity))), "Результат изменения");
                    EditButtonContent = "Изменить";
                    CancelVisibility = Visibility.Collapsed;
                    FilterAudience();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат изменения");
            }
        }

        private void AudienceCancel_Method(object parametr)
        {
            EditButtonContent = "Изменить";
            CancelVisibility = Visibility.Collapsed;
        }

        private void AudienceRemove_Method(object parametr)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    ChangeErrors = audienceModel.Remove(AudienceModel.GetAudienceObject(ChangeName, int.Parse(changeCapacity)));
                    MessageBox.Show(ChangeErrors, "Результат удаления");
                    FilterAudience();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат удаления");
            }
            else
                MessageBox.Show("Для удаления отмените операцию \"Изменение\" нажав на кнопку\"Отмена\"", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        #endregion

        #region Validte
        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }

        static readonly string[] ValidatesAddProperties =
        {
            "AddNumber",
            "AddCorps",
            "AddCapacity"
        };

        static readonly string[] ValidatesChangeProperties =
        {
            "ChangeNumber",
            "ChangeCorps",
            "ChangeCapacity"
        };
        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                return error = Validation(propertyName);
            }
        }
        private bool IsValid(string[] validatingStrings, out string Error)
        {
            Error = null;
            foreach (var str in validatingStrings)
                Error += Validation(str);
            return Error.Equals("");
        }
        private string Validation(String propertyName)
        {
            switch (propertyName)
            {
                case "AddNumber":

                    if (string.IsNullOrWhiteSpace(AddNumber))
                        return EmptyString();
                    else if (!regexAuditorium.IsMatch(AddNumber))
                        return "Неправильный формат.\\d{0,3}[aб]? - Для умных";
                    else if (AddNumber.Length > 4)
                        return "Максимум 4 символа.";
                    else
                        return null;

                case "ChangeNumber":

                    if (string.IsNullOrWhiteSpace(changeNumber))
                        return EmptyString();
                    else
                        return null;

                case "AddCorps":

                    if (string.IsNullOrWhiteSpace(AddCorps))
                        return EmptyString();
                    else
                        return null;

                case "ChangeCorps":

                    if (string.IsNullOrWhiteSpace(changeCorps))
                        return EmptyString();
                    else
                        return null;

                case "AddCapacity":

                    return ResultForIntValues(AddCapacity, 1, 1000);

                case "ChangeCapacity":

                    return ResultForIntValues(changeCapacity, 1, 1000);

                default: return null;
            }
        }
        #endregion
    }
}
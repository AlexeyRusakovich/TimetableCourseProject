using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CourseProject.Models;
using System.Runtime.CompilerServices;
using System.Windows;
using CourseProjectTimetable.ViewModel.Commands;
using System.Collections.Specialized;
using CourseProjectTimetable;

namespace CourseProjectTimetable.ViewModel
{
    public class PulpitsViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public PulpitsViewModel()
        {
            this.context = new TimetableCourseProject();
            PulpitDatabase = context.Pulpits.Local;
            Pulpit = new ObservableCollection<Pulpits>(PulpitDatabase);
            pulpitModel = new PulpitsModel();
        }

        #region Properties

        #region Other

        private TimetableCourseProject context;
        private PulpitsModel pulpitModel;

        private Pulpits changedPulpit;
        private ObservableCollection<Pulpits> PulpitDatabase { get; set; }

        private ObservableCollection<Pulpits> pulpit;
        public ObservableCollection<Pulpits> Pulpit
        {
            get { return pulpit; }
            set
            {
                pulpit = value;
                OnPropertyChanged();
            }
        }

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

        private Pulpits currentDataGridItem;
        public Pulpits CurrentDataGridItem
        {
            set
            {
                currentDataGridItem = value;
                OnPropertyChanged();
                if (currentDataGridItem != null)
                {
                    ChangeShortName = currentDataGridItem.ShortName;
                    ChangeFullName = currentDataGridItem.FullName;
                    ChangeShortFacultyName = currentDataGridItem.ShortFacultyName;
                }
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

        private string AddErrors;
        public string AddShortName { get; set; }
        public string AddFullName { get; set; }
        public string AddShortFacultyName { get; set; }

        #endregion

        #region Change

        private string ChangeErrors;

        private string changeShortName;
        private string changeShortFacultyName;
        private string changeFullName;

        public string ChangeShortName
        {
            get { return changeShortName; }
            set
            {
                changeShortName = value;
                OnPropertyChanged();
            }
        }
        public string ChangeFullName
        {
            get { return changeFullName; }
            set
            {
                changeFullName = value;
                OnPropertyChanged();
            }
        }
        public string ChangeShortFacultyName
        {
            get { return changeShortFacultyName; }
            set
            {
                changeShortFacultyName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Search

        private string searchShortName;
        private string searchShortFacultyName;
        private string searchFullName;

        public string SearchShortName
        {
            get { return searchShortName; }
            set
            {
                searchShortName = value;
                OnPropertyChanged();
                FilterPulpits();
            }
        }
        public string SearchFullName
        {
            get { return searchFullName; }
            set
            {
                searchFullName = value;
                OnPropertyChanged();
                FilterPulpits();
            }
        }
        public string SearchShortFacultyName
        {
            get { return searchShortFacultyName; }
            set
            {
                searchShortFacultyName = value;
                OnPropertyChanged();
                FilterPulpits();
            }
        }

        #endregion

        #endregion

        #region Commands

        private Command filter;
        public Command Filter
        {
            get
            {
                if (filter == null)
                    filter = new Command((obj) => FilterPulpits());
                return filter;
            }
        }
        
        private Command pulpitAdd;
        public Command PulpitAdd
        {
            get
            {
                if (pulpitAdd == null)
                    pulpitAdd = new Command(pulpitAdd_Method);
                return pulpitAdd;
            }
        }

        private Command pulpitRemove;
        public Command PulpitRemove
        {
            get
            {
                if (pulpitRemove == null)
                    pulpitRemove = new Command(pulpitRemove_Method);
                return pulpitRemove;
            }
        }

        private Command pulpitChange;
        public Command PulpitChange
        {
            get
            {
                if (pulpitChange == null)
                    pulpitChange = new Command(pulpitChange_Method);
                return pulpitChange;
            }
        }

        private Command pulpitCancel;
        public Command PulpitCancel
        {
            get
            {
                if (pulpitCancel == null)
                    pulpitCancel = new Command(pulpitCancel_Method);
                return pulpitCancel;
            }
        }
        #endregion

        #region Methods
        private void FilterPulpits()
        {
            if(Pulpit != null)
            { 
                Pulpit.Clear();
                foreach (var obj in PulpitDatabase)
                if (FilterPulpits(obj))
                    Pulpit.Add(obj);
            }
        }
        private bool FilterPulpits(object obj)
        {
            Pulpits pulpit = obj as Pulpits;
            if (pulpit != null)
            {
                if (!string.IsNullOrWhiteSpace(searchShortName) && !(pulpit.ShortName.Contains(searchShortName)))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchFullName) && !pulpit.FullName.Contains(searchFullName))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchShortFacultyName) && !pulpit.ShortFacultyName.Contains(searchShortFacultyName))
                    return false;
            }
            return true;
        }
        private void pulpitAdd_Method(object obj)
        {
            if (IsValid(ValidatesAddProperties, out AddErrors))
            {
                AddErrors += pulpitModel.Add(PulpitsModel.GetPulpitObject(AddShortName, AddFullName, AddShortFacultyName));
                MessageBox.Show(AddErrors, "Результат добавления");
                FilterPulpits();
            }
            else
                MessageBox.Show("Заполните поля корректно!", "Результат добавления");
        }
        private void pulpitChange_Method(object obj)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    if (pulpitModel.IsExist(PulpitsModel.GetPulpitObject(changeShortName, changeFullName, changeShortFacultyName)))
                    {
                        EditButtonContent = "Сохранить";
                        CancelVisibility = Visibility.Visible;
                        changedPulpit = PulpitsModel.GetPulpitObject(changeShortName, changeFullName, changeShortFacultyName);
                        return;
                    }
                    else
                        MessageBox.Show("Изменяемого объекта не существует", "Результат изменения");

                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат Изменения");

            }
            else if (editButtonContent.Equals("Сохранить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    ChangeErrors += pulpitModel.Change(changedPulpit,PulpitsModel.GetPulpitObject(changeShortName, changeFullName, changeShortFacultyName));
                    MessageBox.Show(ChangeErrors, "Результат изменения");
                    EditButtonContent = "Изменить";
                    CancelVisibility = Visibility.Collapsed;
                    FilterPulpits();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат изменения");
            }
        }
        private void pulpitCancel_Method(object obj)
        {
            EditButtonContent = "Изменить";
            CancelVisibility = Visibility.Collapsed;
        }
        private void pulpitRemove_Method(object obj)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    ChangeErrors = pulpitModel.Remove(PulpitsModel.GetPulpitObject(changeShortName, changeFullName, changeShortFacultyName));
                    MessageBox.Show(ChangeErrors, "Результат удаления");
                    FilterPulpits();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат удаления");
            }
            else
                MessageBox.Show("Для удаления отмените операцию \"Изменение\" нажав на кнопку\"Отмена\"", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        #endregion

        #region Validate

        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }

        static readonly string[] ValidatesAddProperties =
        {
            "AddShortName",
            "AddShortFacultyName",
            "AddFullName"
        };

        static readonly string[] ValidatesChangeProperties =
        {
            "ChangeShortName",
            "ChangeShortFacultyName",
            "ChangeFullName"
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
                case "AddShortName":

                    return ResultForStringValues(AddShortName, 10);

                case "ChangeShortName":

                    return ResultForStringValues(changeShortName, 10);

                case "AddShortFacultyName":

                    if (String.IsNullOrWhiteSpace(AddShortFacultyName))
                        return EmptyString();
                    return null;

                case "ChangeShortFacultyName":

                    if (String.IsNullOrWhiteSpace(changeShortFacultyName))
                        return EmptyString();
                    return null;

                case "AddFullName":

                    return ResultForStringValues(AddShortName, 100);

                case "ChangeFullName":

                    return ResultForStringValues(changeFullName, 100);

                default: return null;
            }
        }

        #endregion
    }
}

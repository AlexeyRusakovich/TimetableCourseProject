using CourseProject.Models;
using CourseProjectTimetable.ViewModel.Commands;
using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProjectTimetable.ViewModel
{
    public class SpecialitiesViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {        
        public SpecialitiesViewModel()
        {
            this.context = new TimetableCourseProject();
            context.Specialities.Load();
            SpecialitiesDatabase = context.Specialities.Local;
            Specialities = new ObservableCollection<Specialities>(SpecialitiesDatabase);
            specialityModel = new SpecialityModel();
        }

        #region Properties

        #region Other

        private TimetableCourseProject context;
        private SpecialityModel specialityModel;

        private Specialities changedSpeciality;

        private ObservableCollection<Specialities> SpecialitiesDatabase { get; set; }

        private ObservableCollection<Specialities> specialities;
        public ObservableCollection<Specialities> Specialities
        {
            get { return specialities; }
            set
            {
                specialities = value;
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

        private Specialities currentDataGridItem;
        public Specialities CurrentDataGridItem
        {
            set
            {
                currentDataGridItem = value;
                OnPropertyChanged();
                if (currentDataGridItem != null)
                {
                    ChangeCode = currentDataGridItem.Code;
                    ChangeFullName = currentDataGridItem.FullName;
                    ChangeShortName = currentDataGridItem.ShortName;
                    ChangeQualification = currentDataGridItem.Qualification;
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
        public string AddCode { get; set; }
        public string AddShortFacultyName { get; set; }
        public string AddQualification { get; set; }

        #endregion
        
        #region Change

        private string ChangeErrors;

        private string changeCode;
        private string changeShortFacultyName;
        private string changeShortName;
        private string changeFullName;
        private string changeQualification;

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
        public string ChangeCode
        {
            get { return changeCode; }
            set
            {
                changeCode = value;
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
        public string ChangeQualification
        {
            get { return changeQualification; }
            set
            {
                changeQualification = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Search

        private string searchCode;
        private string searchShortFacultyName;
        private string searchShortName;
        private string searchFullName;
        private string searchQualification;

        public string SearchShortName
        {
            set
            {
                searchShortName = value;
                OnPropertyChanged();
                FilterSpecialities();
            }
        }
        public string SearchFullName
        {
            set
            {
                searchFullName = value;
                OnPropertyChanged();
                FilterSpecialities();
            }
        }
        public string SearchCode
        {
            set
            {
                searchCode = value;
                OnPropertyChanged();
                FilterSpecialities();
            }
        }
        public string SearchShortFacultyName
        {
            set
            {
                searchShortFacultyName = value;
                OnPropertyChanged();
                FilterSpecialities();
            }
        }
        public string SearchQualification
        {
            set
            {
                searchQualification = value;
                OnPropertyChanged();
                FilterSpecialities();
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
                    filter = new Command((obj) => FilterSpecialities());
                return filter;
            }
        }

        private Command specialityAdd;
        public Command SpecialityAdd
        {
            get
            {
                if (specialityAdd == null)
                    specialityAdd = new Command(SpecialityAdd_Method);
                return specialityAdd;
            }
        }

        private Command specialityRemove;
        public Command SpecialityRemove
        {
            get
            {
                if (specialityRemove == null)
                    specialityRemove = new Command(SpecialityRemove_Method);
                return specialityRemove;
            }
        }

        private Command specialityChange;
        public Command SpecialityChange
        {
            get
            {
                if (specialityChange == null)
                    specialityChange = new Command(SpecialityChange_Method);
                return specialityChange;
            }
        }

        private Command specialityCancel;
        public Command SpecialityCancel
        {
            get
            {
                if (specialityCancel == null)
                    specialityCancel = new Command(SpecialityCancel_Method);
                return specialityCancel;
            }
        }

        #endregion

        #region Methods
        private void FilterSpecialities()
        {
            TimetableCourseProject Context = new TimetableCourseProject();
            SpecialitiesDatabase = new ObservableCollection<Specialities>(Context.Specialities.ToList());
            context.Specialities.Load();
            if (Specialities != null)
            {
                Specialities.Clear();
                foreach (var obj in SpecialitiesDatabase)
                    if (FilterSpecialities(obj))
                        Specialities.Add(obj);
            }
                
        }

        private bool FilterSpecialities(object obj)
        {
            if (obj is Specialities speciality)
            {
                if (!string.IsNullOrWhiteSpace(searchCode) && !(speciality.Code.Contains(searchCode)))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchShortName) && !speciality.ShortName.Contains(searchShortName))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchFullName) && !speciality.FullName.Contains(searchFullName))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchShortFacultyName) && !speciality.ShortFacultyName.Contains(searchShortFacultyName))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchQualification) && !speciality.Qualification.Contains(searchQualification))
                    return false;
            }
            return true;
        }

        private void SpecialityAdd_Method(object obj)
        {
            if (IsValid(ValidatesAddProperties, out AddErrors))
            {
                AddErrors += specialityModel.Add(SpecialityModel.getSpecialityObject(AddCode,AddShortName,AddFullName,AddShortFacultyName, AddQualification));
                MessageBox.Show(AddErrors, "Результат добавления");
                FilterSpecialities();
            }
            else
                MessageBox.Show("Заполните поля корректно!", "Результат добавления");
        }

        private void SpecialityChange_Method(object obj)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    if (specialityModel.IsExist(SpecialityModel.getSpecialityObject(changeCode, changeShortName, changeFullName, changeShortFacultyName, changeQualification)))
                    {
                        EditButtonContent = "Сохранить";
                        CancelVisibility = Visibility.Visible;
                        changedSpeciality = SpecialityModel.getSpecialityObject(changeCode, changeShortName, changeFullName, changeShortFacultyName, changeQualification);
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
                    ChangeErrors += specialityModel.Change(changedSpeciality, SpecialityModel.getSpecialityObject(changeCode, changeShortName, changeFullName, changeShortFacultyName, changeQualification));
                    MessageBox.Show(ChangeErrors, "Результат изменения");
                    EditButtonContent = "Изменить";
                    CancelVisibility = Visibility.Collapsed;
                    FilterSpecialities();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат изменения");
            }
        }

        private void SpecialityCancel_Method(object obj)
        {
            EditButtonContent = "Изменить";
            CancelVisibility = Visibility.Collapsed;
        }

        private void SpecialityRemove_Method(object obj)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    ChangeErrors = specialityModel.Remove(SpecialityModel.getSpecialityObject(changeCode, changeShortName, changeFullName, changeShortFacultyName, changeQualification));
                    MessageBox.Show(ChangeErrors, "Результат удаления");
                    FilterSpecialities();
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
            "AddCode",
            "AddShortFacultyName",
            "AddShortName",
            "AddFullName",
            "AddQualification",
        };

        static readonly string[] ValidatesChangeProperties =
        {
            "ChangeCode",
            "ChangeShortFacultyName",
            "ChangeShortName",
            "ChangeFullName",
            "ChangeQualification",
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
                case "AddCode":

                    if (String.IsNullOrWhiteSpace(AddCode))
                        return EmptyString();
                    else if (AddCode.Length > 20)
                        return AllowableLength(20);
                    return null;

                case "ChangeCode":

                    if (String.IsNullOrWhiteSpace(changeCode))
                        return EmptyString();
                    else if (changeCode.Length > 20)
                        return AllowableLength(20);
                    return null;

                case "AddShortFacultyName":

                    if (String.IsNullOrWhiteSpace(AddShortFacultyName))
                        return EmptyString();
                    return null;

                case "ChangeShortFacultyName":

                    if (String.IsNullOrWhiteSpace(changeShortFacultyName))
                        return EmptyString();
                    return null;

                case "AddShortName":

                    return ResultForStringValues(AddShortName, 10);

                case "ChangeShortName":

                    return ResultForStringValues(changeShortName, 10);

                case "AddFullName":

                    return ResultForStringValues(AddFullName, 100);

                case "ChangeFullName":

                    return ResultForStringValues(changeFullName, 100);

                case "AddQualification":

                    return ResultForStringValues(AddQualification, 40);

                case "ChangeQualification":

                    return ResultForStringValues(changeQualification, 40);

                default: return null;
            }
        }
        #endregion        
    }
}

using CourseProject.Models;
using CourseProjectTimetable.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using CourseProjectTimetable;

namespace CourseProjectTimetable.ViewModel
{
    public class FacultiesViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public FacultiesViewModel()
        {
            this.context = new TimetableCourseProject();
            FacultiesDatabase = context.Faculties.Local;
            Faculty = new ObservableCollection<Faculties>(FacultiesDatabase);
            facultyModel = new FacultiesModel();
        }

        #region Properties

        #region Other

        private TimetableCourseProject context;

        private FacultiesModel facultyModel;

        private Faculties changedFaculty;
        private ObservableCollection<Faculties> FacultiesDatabase { get; set; }

        private ObservableCollection<Faculties> faculty;
        public ObservableCollection<Faculties> Faculty
        {
            get { return faculty; }
            set
            {
                faculty = value;
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

        private Faculties currentDataGridItem;
        public Faculties CurrentDataGridItem
        {
            set
            {
                currentDataGridItem = value;
                OnPropertyChanged();
                ChangeFullName = currentDataGridItem.FullName ?? "";
                ChangeShortName = currentDataGridItem.ShortName ?? "";
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

        #endregion

        #region Change

        private string ChangeErrors;

        private string changeShortName;
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

        #endregion
        
        #region Search

        private string searchShortName;
        private string searchFullName;

        public string SearchShortName
        {
            set
            {
                searchShortName = value;
                OnPropertyChanged();
                FilterFaculties();
            }
        }
        public string SearchFullName
        {
            set
            {
                searchFullName = value;
                OnPropertyChanged();
                FilterFaculties();
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
                    filter = new Command((obj) => FilterFaculties());
                return filter;
            }
        }

        private Command facultyAdd;
        public Command FacultyAdd
        {
            get
            {
                if (facultyAdd == null)
                    facultyAdd = new Command(FacultyAdd_Method);
                return facultyAdd;
            }
        }

        private Command facultyRemove;
        public Command FacultyRemove
        {
            get
            {
                if (facultyRemove == null)
                    facultyRemove = new Command(FacultyRemove_Method);
                return facultyRemove;
            }
        }

        private Command facultyChange;
        public Command FacultyChange
        {
            get
            {
                if (facultyChange == null)
                    facultyChange = new Command(FacultyChange_Method);
                return facultyChange;
            }
        }

        private Command facultyCancel;
        public Command FacultyCancel
        {
            get
            {
                if (facultyCancel == null)
                    facultyCancel = new Command(FacultyCancel_Method);
                return facultyCancel;
            }
        }

        #endregion

        #region Methods
        private void FilterFaculties()
        {
            if (Faculty != null)
            {
                Faculty.Clear();
                if (FacultiesDatabase.Count != 0)
                    foreach (var obj in FacultiesDatabase)
                        if (FilterFaculties(obj))
                            Faculty.Add(obj);
            }
        }
        private bool FilterFaculties(object obj)
        {
            if (obj is Faculties faculty)
            {
                if (!string.IsNullOrWhiteSpace(searchShortName) && !faculty.ShortName.Contains(searchShortName))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchFullName) && !faculty.FullName.Contains(searchFullName))
                    return false;
            }
            return true;
        }
        private void FacultyAdd_Method(object obj)
        {
            if (IsValid(ValidatesAddProperties, out AddErrors))
            {
                AddErrors += facultyModel.Add(FacultiesModel.GetFacultyObject(AddShortName, AddFullName ));
                MessageBox.Show(AddErrors, "Результат добавления");
                FilterFaculties();
            }
            else
                MessageBox.Show("Заполните поля корректно!", "Результат добавления");
        }
        private void FacultyChange_Method(object obj)
        {
            if(editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    if (facultyModel.IsExist(FacultiesModel.GetFacultyObject(changeShortName, changeFullName)))
                    {
                        EditButtonContent = "Сохранить";
                        CancelVisibility = Visibility.Visible;
                        changedFaculty = FacultiesModel.GetFacultyObject(changeShortName, changeFullName);
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
                    ChangeErrors += facultyModel.Change(changedFaculty, FacultiesModel.GetFacultyObject(changeShortName, changeFullName));
                    MessageBox.Show(ChangeErrors, "Результат изменения");
                    EditButtonContent = "Изменить";
                    CancelVisibility = Visibility.Collapsed;
                    FilterFaculties();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат изменения");
            }
        }
        private void FacultyCancel_Method(object obj)
        {
            EditButtonContent = "Изменить";
            CancelVisibility = Visibility.Collapsed;
        }
        private void FacultyRemove_Method(object obj)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    ChangeErrors = facultyModel.Remove(FacultiesModel.GetFacultyObject(changeShortName, changeFullName));
                    MessageBox.Show(ChangeErrors, "Результат удаления");
                    FilterFaculties();
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
            "AddFullName",
            "AddShortName"
        };

        static readonly string[] ValidatesChangeProperties =
        {
            "ChangeFullName",
            "ChangeShortName"
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
                case "AddFullName":

                    return ResultForStringValues(AddFullName, 50);

                case "ChangeFullName":

                    return ResultForStringValues(changeFullName, 50);

                case "AddShortName":

                    return ResultForStringValues(AddShortName, 10);

                case "ChangeShortName":

                    return ResultForStringValues(changeShortName, 10);

                default: return null;
            }
        }

        #endregion        
    }
}

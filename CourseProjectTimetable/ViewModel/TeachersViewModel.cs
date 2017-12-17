using CourseProject.Models;
using CourseProjectTimetable.ViewModel.Commands;
using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProjectTimetable.ViewModel
{
    public class TeachersViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public TeachersViewModel()
        {
            this.context = new TimetableCourseProject();
            TeachersDatabase = context.Teachers.Local;
            Teachers = new ObservableCollection<Teachers>(TeachersDatabase);
            teachersModel = new TeachersModel();
        }

        #region Properties

        #region Other

        private TimetableCourseProject context;

        private TeachersModel teachersModel;

        private ObservableCollection<Teachers> TeachersDatabase { get; set; }

        private ObservableCollection<Teachers> teachers;
        public ObservableCollection<Teachers> Teachers
        {
            get { return teachers; }
            set
            {
                teachers = value;
                OnPropertyChanged();
            }
        }

        private Teachers changedTeacher;

        private Teachers currentDataGridItem;
        public Teachers CurrentDataGridItem
        {
            set
            {
                currentDataGridItem = value;
                OnPropertyChanged();
                if (currentDataGridItem != null)
                {
                    ChangeName = currentDataGridItem.Name;
                    ChangeSurname = currentDataGridItem.Surname;
                    ChangePatronymic = currentDataGridItem.Patronymic;
                    ChangeShortPulpitName = currentDataGridItem.ShortPulpitName;
                }
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

        public string AddName { get; set; }
        public string AddSurname { get; set; }
        public string AddPatronymic { get; set; }
        public string AddShortPulpitName { get; set; }

        #endregion
        
        #region Change

        private string ChangeErrors;

        private string changeName;
        private string changeSurname;
        private string changePatronymic;
        private string changeShortPulpitName;

        public string ChangeName
        {
            get { return changeName; }
            set
            {
                changeName = value;
                OnPropertyChanged();
            }
        }
        public string ChangeSurname
        {
            get { return changeSurname; }
            set
            {
                changeSurname = value;
                OnPropertyChanged();
            }
        }
        public string ChangePatronymic
        {
            get { return changePatronymic; }
            set
            {
                changePatronymic = value;
                OnPropertyChanged();
            }
        }
        public string ChangeShortPulpitName
        {
            get { return changeShortPulpitName; }
            set
            {
                changeShortPulpitName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Search

        private string searchName;
        private string searchSurname;
        private string searchPatronymic;
        private string searchShortPulpitName;

        public string SearchName
        {
            set
            {
                searchName = value;
                OnPropertyChanged();
                FilterTeachers();
            }
        }
        public string SearchSurname
        {
            set
            {
                searchSurname = value;
                OnPropertyChanged();
                FilterTeachers();
            }
        }
        public string SearchPatronymic
        {
            set
            {
                searchPatronymic = value;
                OnPropertyChanged();
                FilterTeachers();
            }
        }
        public string SearchShortPulpitName
        {
            set
            {
                searchShortPulpitName = value;
                OnPropertyChanged();
                FilterTeachers();
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
                    filter = new Command((obj) => FilterTeachers());
                return filter;
            }
        }

        private Command teacherAdd;
        public Command TeacherAdd
        {
            get
            {
                if (teacherAdd == null)
                    teacherAdd = new Command(teacherAdd_Method);
                return teacherAdd;
            }
        }

        private Command teacherRemove;
        public Command TeacherRemove
        {
            get
            {
                if (teacherRemove == null)
                    teacherRemove = new Command(teacherRemove_Method);
                return teacherRemove;
            }
        }

        private Command teacherChange;
        public Command TeacherChange
        {
            get
            {
                if (teacherChange == null)
                    teacherChange = new Command(teacherChange_Method);
                return teacherChange;
            }
        }

        private Command teacherCancel;
        public Command TeacherCancel
        {
            get
            {
                if (teacherCancel == null)
                    teacherCancel = new Command(teacherCancel_Method);
                return teacherCancel;
            }
        }

        #endregion

        #region Methods

        private void CollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            FilterTeachers();
        }
        private void FilterTeachers()
        {
            if (Teachers != null)
            {
                Teachers.Clear();
                if (TeachersDatabase.Count != 0)
                    foreach (var obj in TeachersDatabase)
                        if (FilterTeachers(obj))
                            Teachers.Add(obj);
            }
        }

        public bool FilterTeachers(object obj)
        {
            Teachers teacher = obj as Teachers;
            if (teacher != null)
            {
                if (!String.IsNullOrWhiteSpace(searchName) && !teacher.Name.Contains(searchName))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchSurname) && !teacher.Surname.Contains(searchSurname))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchPatronymic) && !teacher.Patronymic.Contains(searchPatronymic))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchShortPulpitName) && !teacher.ShortPulpitName.Contains(searchShortPulpitName))
                    return false;
            }
            return true;
        }

        public void teacherAdd_Method(object paramert)
        {
            if (IsValid(ValidatesAddProperties, out AddErrors))
            {
                AddErrors += teachersModel.Add(TeachersModel.getTeacherObject(AddName,AddSurname,AddPatronymic, AddShortPulpitName));
                MessageBox.Show(AddErrors, "Результат добавления");
                FilterTeachers();
            }
            else
                MessageBox.Show("Заполните поля корректно!", "Результат добавления");
        }

        private void teacherChange_Method(object parametr)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    if (teachersModel.IsExist(TeachersModel.getTeacherObject(changeName, changeSurname, changePatronymic, changeShortPulpitName)))
                    {
                        EditButtonContent = "Сохранить";
                        CancelVisibility = Visibility.Visible;
                        changedTeacher = TeachersModel.getTeacherObject(changeName, changeSurname, changePatronymic, changeShortPulpitName);
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
                    MessageBox.Show(teachersModel.Change(changedTeacher, TeachersModel.getTeacherObject(changeName, changeSurname, changePatronymic, changeShortPulpitName)), "Результат изменения");
                    EditButtonContent = "Изменить";
                    CancelVisibility = Visibility.Collapsed;
                    FilterTeachers();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат изменения");
            }
        }

        private void teacherCancel_Method(object parametr)
        {
            EditButtonContent = "Изменить";
            CancelVisibility = Visibility.Collapsed;
        }

        private void teacherRemove_Method(object parametr)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    ChangeErrors = teachersModel.Remove(TeachersModel.getTeacherObject(changeName, changeSurname, changePatronymic, changeShortPulpitName));
                    MessageBox.Show(ChangeErrors, "Результат удаления");
                    FilterTeachers();
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
            "AddName",
            "AddSurname",
            "AddPatronymic",
            "AddShortPulpitName"
        };

        static readonly string[] ValidatesChangeProperties =
        {
            "ChangeName",
            "ChangeSurname",
            "ChangePatronymic",
            "ChangeShortPulpitName"
        };

        private bool IsValid(string[] validatingStrings, out string Error)
        {
            Error = null;
            foreach (var str in validatingStrings)
                Error += Validation(str);
            return Error.Equals("");
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                return error = Validation(propertyName);
            }
        }

        private string Validation(String propertyName)
        {
            switch (propertyName)
            {
                case "AddName":

                    return ResultForStringValues(AddName, 20);

                case "ChangeName":

                    return ResultForStringValues(changeName, 20);

                case "AddSurname":

                    return ResultForStringValues(AddSurname, 20);

                case "ChangeSurname":

                    return ResultForStringValues(changeSurname, 20);

                case "AddPatronymic":

                    return ResultForStringValues(AddPatronymic, 20);

                case "ChangePatronymic":

                    return ResultForStringValues(changePatronymic, 20);

                case "AddShortPulpitName":

                    if (string.IsNullOrWhiteSpace(AddShortPulpitName))
                        return EmptyString();
                    else
                        return null;

                case "ChangeShortPulpitName":

                    if (string.IsNullOrWhiteSpace(changeShortPulpitName))
                        return EmptyString();
                    else
                        return null;

                default: return null;
            }
        }
        #endregion   
    }
}

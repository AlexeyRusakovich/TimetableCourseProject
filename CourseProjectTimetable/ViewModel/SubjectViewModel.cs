using CourseProject.Models;
using CourseProjectTimetable.ViewModel.Commands;
using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;

namespace CourseProjectTimetable.ViewModel
{
    public class SubjectViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public SubjectViewModel()
        {
            this.context = new TimetableCourseProject();
            context.Subjects.Load();
            SubjectsDatabase = context.Subjects.Local;
            Subjects = new ObservableCollection<Subjects>(SubjectsDatabase);
            subjectsModel = new SubjectsModel();
        }

        #region Properties

        #region Other

        private TimetableCourseProject context;
        private SubjectsModel subjectsModel;

        private Subjects changedSubject;

        private ObservableCollection<Subjects> SubjectsDatabase { get; set; }

        private ObservableCollection<Subjects> subjects;
        public ObservableCollection<Subjects> Subjects
        {
            get { return subjects; }
            set
            {
                subjects = value;
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

        private Subjects currentDataGridItem;
        public Subjects CurrentDataGridItem
        {
            set
            {
                currentDataGridItem = value;
                OnPropertyChanged();
                if (currentDataGridItem != null)
                {
                    ChangeFullName = currentDataGridItem.FullName;
                    ChangeShortName = currentDataGridItem.ShortName;
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
                FilterSubjects();
            }
        }
        public string SearchFullName
        {
            set
            {
                searchFullName = value;
                OnPropertyChanged();
                FilterSubjects();
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
                    filter = new Command((obj) => FilterSubjects());
                return filter;
            }
        }

        private Command subjectsAdd;
        public Command SubjectsAdd
        {
            get
            {
                if (subjectsAdd == null)
                    subjectsAdd = new Command(subjectsAdd_Method);
                return subjectsAdd;
            }
        }

        private Command subjectsRemove;
        public Command SubjectsRemove
        {
            get
            {
                if (subjectsRemove == null)
                    subjectsRemove = new Command(subjectsRemove_Method);
                return subjectsRemove;
            }
        }

        private Command subjectsChange;
        public Command SubjectsChange
        {
            get
            {
                if (subjectsChange == null)
                    subjectsChange = new Command(subjectsChange_Method);
                return subjectsChange;
            }
        }

        private Command subjectsCancel;
        public Command SubjectsCancel
        {
            get
            {
                if (subjectsCancel == null)
                    subjectsCancel = new Command(subjectsCancel_Method);
                return subjectsCancel;
            }
        }

        #endregion

        #region Methods
        
        private void FilterSubjects()
        {
            TimetableCourseProject Context = new TimetableCourseProject();
            SubjectsDatabase = new ObservableCollection<Subjects>(Context.Subjects.ToList());
            context.Subjects.Load();
            if (Subjects != null)
            {
                Subjects.Clear();
                foreach (var obj in SubjectsDatabase)
                    if (FilterSubjects(obj))
                        Subjects.Add(obj);
            }
        }

        private bool FilterSubjects(object obj)
        {
            Subjects subject = obj as Subjects;
            if (subject != null)
            {
                if (!string.IsNullOrWhiteSpace(searchFullName) && !(subject.FullName.Contains(searchFullName)))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchShortName) && !subject.ShortName.Contains(searchShortName))
                    return false;
            }
            return true;
        }

        private void subjectsAdd_Method(object obj)
        {
            if (IsValid(ValidatesAddProperties, out AddErrors))
            {
                AddErrors += subjectsModel.Add(SubjectsModel.getSubjectObject(AddShortName,AddFullName));
                MessageBox.Show(AddErrors, "Результат добавления");
                FilterSubjects();
            }
            else
                MessageBox.Show("Заполните поля корректно!", "Результат добавления");
        }

        private void subjectsChange_Method(object obj)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    if (subjectsModel.IsExist(SubjectsModel.getSubjectObject(changeShortName, changeFullName)))
                    {
                        EditButtonContent = "Сохранить";
                        CancelVisibility = Visibility.Visible;
                        changedSubject = SubjectsModel.getSubjectObject(changeShortName, changeFullName);
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
                    ChangeErrors += subjectsModel.Change(changedSubject, SubjectsModel.getSubjectObject(changeShortName, changeFullName));
                    MessageBox.Show(ChangeErrors, "Результат изменения");
                    EditButtonContent = "Изменить";
                    CancelVisibility = Visibility.Collapsed;
                    FilterSubjects();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат изменения");
            }
        }

        private void subjectsCancel_Method(object obj)
        {
            EditButtonContent = "Изменить";
            CancelVisibility = Visibility.Collapsed;
        }

        private void subjectsRemove_Method(object obj)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    ChangeErrors = subjectsModel.Remove(SubjectsModel.getSubjectObject(changeShortName, changeFullName));
                    MessageBox.Show(ChangeErrors, "Результат удаления");
                    FilterSubjects();
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
            "AddFullName"
        };

        static readonly string[] ValidatesChangeProperties =
        {
            "ChangeShortName",
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

                    if (string.IsNullOrWhiteSpace(AddShortName))
                        return EmptyString();
                    else if (AddShortName.Length > 10)
                        return AllowableLength(10);
                    return null;

                case "ChangeShortName":

                    if (string.IsNullOrWhiteSpace(ChangeShortName))
                        return EmptyString();
                    else if (ChangeShortName.Length > 10)
                        return AllowableLength(10);
                    return null;

                case "AddFullName":

                    return ResultForStringValues(AddFullName, 100);

                case "ChangeFullName":

                    return ResultForStringValues(changeFullName, 100);

                default: return null;
            }
        }
        #endregion   
    }
}

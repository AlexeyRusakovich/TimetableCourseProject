using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CourseProject.Models;
using CourseProjectTimetable.ViewModel.Commands;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Collections.Specialized;
using CourseProjectTimetable;

namespace CourseProjectTimetable.ViewModel
{
    public class GroupsViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public GroupsViewModel()
        {
            this.context = new TimetableCourseProject();
            GroupsDatabase = context.Groups.Local;
            Groups = new ObservableCollection<Groups>(GroupsDatabase);
            groupModel = new GroupsModel();
        }

        #region Properties

        #region Other

        private TimetableCourseProject context;
        private GroupsModel groupModel;

        private Groups changedGroup;

        private ObservableCollection<Groups> GroupsDatabase { get; set; }

        private ObservableCollection<Groups> groups;
        public ObservableCollection<Groups> Groups
        {
            get { return groups; }
            set
            {
                groups = value;
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

        private Groups currentDataGridItem;
        public Groups CurrentDataGridItem
        {
            set
            {
                currentDataGridItem = value;
                OnPropertyChanged();
                if (currentDataGridItem != null)
                {
                    ChangeCount = currentDataGridItem.Count.ToString();
                    ChangeCourse = currentDataGridItem.Course.ToString();
                    ChangeNumber = currentDataGridItem.Number.ToString();
                    ChangeId = currentDataGridItem.Id;
                    ChangeSpeciality = currentDataGridItem.Specialities.ShortName;
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

        public string AddId
        {
            get { return AddSpeciality + ' ' + AddCourse + '-' + AddNumber; }
        }
        public string AddSpeciality { get; set; }
        public string AddCourse { get; set; }
        public string AddNumber { get; set; }
        public string AddCount { get; set; }

        #endregion

        #region Change

        private string ChangeErrors;

        private string changeId;
        private string changeSpeciality;
        private string changeCourse;
        private string changeNumber;
        private string changeCount;

        public string ChangeId
        {
            get { return changeSpeciality + ' ' + changeCourse + '-' + changeNumber; }
            set
            {
                changeId = value;
                OnPropertyChanged();
            }
        }
        public string ChangeSpeciality
        {
            get { return changeSpeciality; }
            set
            {
                changeSpeciality = value;
                OnPropertyChanged();
            }
        }
        public string ChangeCourse
        {
            get { return changeCourse; }
            set
            {
                changeCourse = value;
                OnPropertyChanged();
            }
        }
        public string ChangeNumber
        {
            get { return changeNumber; }
            set
            {
                changeNumber = value;
                OnPropertyChanged();
            }
        }
        public string ChangeCount
        {
            get { return changeCount; }
            set
            {
                changeCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Search

        private string searchSpeciality;
        private string searchCourse;
        private string searchNumber;
        private string searchCount;

        public string SearchSpeciality
        {
            set
            {
                searchSpeciality = value;
                OnPropertyChanged();
                FilterGroups();
            }
        }
        public string SearchCourse
        {
            set
            {
                searchCourse = value;
                OnPropertyChanged();
                FilterGroups();
            }
        }
        public string SearchNumber
        {
            set
            {
                searchNumber = value;
                OnPropertyChanged();
                FilterGroups();
            }
        }
        public string SearchCount
        {
            set
            {
                searchCount = value;
                OnPropertyChanged();
                FilterGroups();
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
                    filter = new Command((obj) => FilterGroups());
                return filter;
            }
        }

        private Command groupAdd;
        public Command GroupAdd
        {
            get
            {
                if (groupAdd == null)
                    groupAdd = new Command(groupAdd_Method);
                return groupAdd;
            }
        }

        private Command groupRemove;
        public Command GroupRemove
        {
            get
            {
                if (groupRemove == null)
                    groupRemove = new Command(groupRemove_Method);
                return groupRemove;
            }
        }

        private Command groupChange;
        public Command GroupChange
        {
            get
            {
                if (groupChange == null)
                    groupChange = new Command(groupChange_Method);
                return groupChange;
            }
        }

        private Command groupCancel;
        public Command GroupCancel
        {
            get
            {
                if (groupCancel == null)
                    groupCancel = new Command(groupCancel_Method);
                return groupCancel;
            }
        }
        #endregion

        #region Methods
        private void FilterGroups()
        {
            if(Groups != null)
            { 
                Groups.Clear();
                if(GroupsDatabase.Count != 0)
                foreach (var obj in GroupsDatabase)
                    if (FilterGroups(obj))
                        Groups.Add(obj);
            }
        }

        private bool FilterGroups(object obj)
        {
            Groups group = obj as Groups;
            if (group != null)
            {
                if (!string.IsNullOrWhiteSpace(searchSpeciality) && !(context.Specialities.Where(g=> g.Code.Equals(group.SpecialityCode)).First().ShortName.Contains(searchSpeciality)))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchCount) && !group.Count.ToString().Contains(searchCount))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchCourse) && !group.Course.ToString().Contains(searchCourse))
                    return false;
                if (!string.IsNullOrWhiteSpace(searchNumber) && !group.Number.ToString().Contains(searchNumber))
                    return false;
            }
            return true;
        }

        private void groupAdd_Method(object obj)
        {
            if (IsValid(ValidatesAddProperties, out AddErrors))
            {
                AddErrors += groupModel.Add(GroupsModel.getGroupObject(AddId,
                                        context.Specialities.Where(s => s.ShortName.Equals(AddSpeciality)).First().Code,
                                        int.Parse(AddCourse), int.Parse(AddNumber), int.Parse(AddCount)));
                MessageBox.Show(AddErrors, "Результат добавления");
                FilterGroups();
            }
            else
                MessageBox.Show("Заполните поля корректно!", "Результат добавления");
        }

        private void groupChange_Method(object obj)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    if (groupModel.IsExist(GroupsModel.getGroupObject(ChangeId,
                                        context.Specialities.Where(s => s.ShortName.Equals(changeSpeciality)).First().Code,
                                        int.Parse(changeCourse), int.Parse(changeNumber), int.Parse(changeCount))))
                    {
                        EditButtonContent = "Сохранить";
                        CancelVisibility = Visibility.Visible;
                        changedGroup = GroupsModel.getGroupObject(ChangeId,
                                        context.Specialities.Where(s => s.ShortName.Equals(changeSpeciality)).First().Code,
                                        int.Parse(changeCourse), int.Parse(changeNumber), int.Parse(changeCount));
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
                    ChangeErrors += groupModel.Change(changedGroup, GroupsModel.getGroupObject(ChangeId,
                                        context.Specialities.Where(s => s.ShortName.Equals(changeSpeciality)).First().Code,
                                        int.Parse(changeCourse), int.Parse(changeNumber), int.Parse(changeCount)));
                    MessageBox.Show(ChangeErrors, "Результат изменения");
                    EditButtonContent = "Изменить";
                    CancelVisibility = Visibility.Collapsed;
                    FilterGroups();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат изменения");
            }
        }

        private void groupCancel_Method(object obj)
        {
            EditButtonContent = "Изменить";
            CancelVisibility = Visibility.Collapsed;
        }

        private void groupRemove_Method(object obj)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    ChangeErrors = groupModel.Remove(GroupsModel.getGroupObject(changeId,
                                        context.Specialities.Where(s => s.ShortName.Equals(changeSpeciality)).First().Code,
                                        int.Parse(changeCourse), int.Parse(changeNumber), int.Parse(changeCount)));
                    MessageBox.Show(ChangeErrors, "Результат удаления");
                    FilterGroups();
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
            "AddSpeciality",
            "AddCourse",
            "AddNumber",
            "AddCount"
        };

        static readonly string[] ValidatesChangeProperties =
        {
            "ChangeSpeciality",
            "ChangeCourse",
            "ChangeNumber",
            "ChangeCount"
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
            switch(propertyName)
            {
                case "AddSpeciality":
                    if (String.IsNullOrWhiteSpace(AddSpeciality))
                        return EmptyString();
                    return null;
                case "ChangeSpeciality":

                    if (String.IsNullOrWhiteSpace(changeSpeciality))
                        return EmptyString();
                    return null;

                case "AddCourse":

                    return ResultForIntValues(AddCourse, 1, 6);

                case "ChangeCourse":

                    return ResultForIntValues(changeCourse, 1, 6);

                case "AddNumber":

                    return ResultForIntValues(AddNumber, 1, 30);

                case "ChangeNumber":

                    return ResultForIntValues(changeNumber, 1, 30);

                case "AddCount":

                    return ResultForIntValues(AddCount, 1, 40);

                case "ChangeCount":

                    return ResultForIntValues(changeCount, 1, 40);

                default: return null;
            }
        }
        
        #endregion
    }
}

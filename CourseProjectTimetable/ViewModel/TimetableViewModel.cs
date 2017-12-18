using CourseProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CourseProjectTimetable.ViewModel.Commands;
using System.Windows;
using System.Collections.Specialized;
using CourseProjectTimetable;
using System.Data.Entity;

namespace CourseProjectTimetable.ViewModel
{
    public class TimetableViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public TimetableViewModel()
        {
            this.Context = new TimetableCourseProject();
            context.Timetable.Load();
            TimetableDatabase = Context.Timetable.Local;
            Timetable = new ObservableCollection<Timetable>(TimetableDatabase);
            timetableModel = new TimetableModel();
        }

        #region Properties

        #region Other

        private TimetableCourseProject context;

        private TimetableModel timetableModel;

        private ObservableCollection<Timetable> TimetableDatabase { get; set; }

        private ObservableCollection<Timetable> timetable;
        public ObservableCollection<Timetable> Timetable
        {
            get { return timetable; }
            set
            {
                timetable = value;
                OnPropertyChanged();
            }
        }

        private Timetable changedTimetable;

        private Timetable currentDataGridItem;
        public Timetable CurrentDataGridItem
        {
            set
            {
                currentDataGridItem = value;
                OnPropertyChanged();
                if (currentDataGridItem != null)
                {
                    ChangeAudienceNumber = currentDataGridItem.AudienceNumber;
                    ChangeDayNumber = currentDataGridItem.DayNumber;
                    ChangeGroupId = currentDataGridItem.GroupId;
                    ChangePairNumber = currentDataGridItem.PairNumber.ToString();
                    ChangeShortPairtypeName = currentDataGridItem.ShortPairtypeName;
                    ChangeShortSubjectName = currentDataGridItem.ShortSubjectName;
                    ChangeSubgroup = currentDataGridItem.Subgroup == null ? "Нету" : currentDataGridItem.Subgroup;
                    ChangeTeacherId = currentDataGridItem.TeacherId;
                    ChangeWeekNumber = currentDataGridItem.WeekNumber == null ? "По обеим" : currentDataGridItem.WeekNumber;
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

        public string AddDayNumber { get; set; }
        public string AddPairNumber { get; set; }
        public string AddWeekNumber { get; set; }
        public string AddGroupId { get; set; }
        public string AddSubgroup { get; set; }
        public string AddShortSubjectName { get; set; }
        public string AddAudienceNumber { get; set; }
        public string AddTeacherId { get; set; }
        public string AddShortPairtypeName { get; set; }

        #endregion

        #region Change

        private string ChangeErrors;

        private string changeDayNumber;
        private string changePairNumber;
        private string changeWeekNumber;
        private string changeGroupId;
        private string changeSubgroup;
        private string changeShortSubjectName;
        private string changeAudienceNumber;
        private string changeTeacherId;
        private string changeShortPairtypeName;

        public string ChangeDayNumber
        {
            get { return changeDayNumber; }
            set
            {
                changeDayNumber = value;
                OnPropertyChanged();
            }
        }
        public string ChangePairNumber
        {
            get { return changePairNumber; }
            set
            {
                changePairNumber = value;
                OnPropertyChanged();
            }
        }
        public string ChangeWeekNumber
        {
            get { return changeWeekNumber; }
            set
            {
                changeWeekNumber = value;
                OnPropertyChanged();
            }
        }
        public string ChangeGroupId
        {
            get { return changeGroupId; }
            set
            {
                changeGroupId = value;
                OnPropertyChanged();
            }
        }
        public string ChangeSubgroup
        {
            get { return changeSubgroup; }
            set
            {
                changeSubgroup = value;
                OnPropertyChanged();
            }
        }
        public string ChangeShortSubjectName
        {
            get { return changeShortSubjectName; }
            set
            {
                changeShortSubjectName = value;
                OnPropertyChanged();
            }
        }
        public string ChangeAudienceNumber
        {
            get { return changeAudienceNumber; }
            set
            {
                changeAudienceNumber = value;
                OnPropertyChanged();
            }
        }
        public string ChangeTeacherId
        {
            get { return changeTeacherId; }
            set
            {
                changeTeacherId = value;
                OnPropertyChanged();
            }
        }
        public string ChangeShortPairtypeName
        {
            get { return changeShortPairtypeName; }
            set
            {
                changeShortPairtypeName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Search

        private string searchDayNumber;
        private string searchPairNumber;
        private string searchWeekNumber;
        private string searchGroupId;
        private string searchSubgroup;
        private string searchShortSubjectName;
        private string searchAudienceNumber;
        private string searchTeacherId;
        private string searchShortPairtypeName;

        public string SearchDayNumber
        {
            set
            {
                searchDayNumber = value;
                OnPropertyChanged();
                FilterTimetable();
            }
        }
        public string SearchPairNumber
        {
            set
            {
                searchPairNumber = value;
                OnPropertyChanged();
                FilterTimetable();
            }
        }
        public string SearchWeekNumber
        {
            set
            {
                searchWeekNumber = value;
                OnPropertyChanged();
                FilterTimetable();
            }
        }
        public string SearchGroupId
        {
            set
            {
                searchGroupId = value;
                OnPropertyChanged();
                FilterTimetable();
            }
        }
        public string SearchSubgroup
        {
            set
            {
                searchSubgroup = value;
                OnPropertyChanged();
                FilterTimetable();
            }
        }
        public string SearchShortSubjectName
        {
            set
            {
                searchShortSubjectName = value;
                OnPropertyChanged();
                FilterTimetable();
            }
        }
        public string SearchAudienceNumber
        {
            set
            {
                searchAudienceNumber = value;
                OnPropertyChanged();
                FilterTimetable();
            }
        }
        public string SearchTeacherId
        {
            set
            {
                searchTeacherId = value;
                OnPropertyChanged();
                FilterTimetable();
            }
        }
        public string SearchShortPairtypeName
        {
            set
            {
                searchShortPairtypeName = value;
                OnPropertyChanged();
                FilterTimetable();
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
                    filter = new Command((obj) => FilterTimetable());
                return filter;
            }
        }

        private Command timetableAdd;
        public Command TimetableAdd
        {
            get
            {
                if (timetableAdd == null)
                    timetableAdd = new Command(timetableAdd_Method);
                return timetableAdd;
            }
        }

        private Command timetableRemove;
        public Command TimetableRemove
        {
            get
            {
                if (timetableRemove == null)
                    timetableRemove = new Command(timetableRemove_Method);
                return timetableRemove;
            }
        }

        private Command timetableChange;
        public Command TimetableChange
        {
            get
            {
                if (timetableChange == null)
                    timetableChange = new Command(timetableChange_Method);
                return timetableChange;
            }
        }

        private Command timetableCancel;
        public Command TimetableCancel
        {
            get
            {
                if (timetableCancel == null)
                    timetableCancel = new Command(timetableCancel_Method);
                return timetableCancel;
            }
        }

        #endregion

        #region Methods
        private void FilterTimetable()
        {
            TimetableCourseProject Context = new TimetableCourseProject();
            TimetableDatabase = new ObservableCollection<Timetable>(Context.Timetable.ToList());
            if (Timetable != null)
            {
                Timetable.Clear();
                if (TimetableDatabase.Count != 0)
                    foreach (var obj in TimetableDatabase)
                        if (FilterTimetable(obj))
                            Timetable.Add(obj);
            }
        }

        public bool FilterTimetable(object obj)
        {
            Timetable timetable = obj as Timetable;
            if (timetable != null)
            {
                if (!String.IsNullOrWhiteSpace(searchAudienceNumber) && !timetable.AudienceNumber.Contains(searchAudienceNumber))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchPairNumber) && !timetable.PairNumber.ToString().Contains(searchPairNumber))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchGroupId) && !timetable.GroupId.Contains(searchGroupId))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchTeacherId) && !timetable.TeacherId.Contains(searchTeacherId))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchShortPairtypeName) && !timetable.ShortPairtypeName.Contains(searchShortPairtypeName))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchShortSubjectName) && !timetable.ShortSubjectName.Contains(searchShortSubjectName))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchWeekNumber) && !timetable.WeekNumber.Contains(searchWeekNumber))
                    return false;
                if (!String.IsNullOrWhiteSpace(searchDayNumber) && !timetable.DayNumber.Contains(searchDayNumber))
                    return false;
                if(!String.IsNullOrWhiteSpace(searchSubgroup) && !timetable.Subgroup.Contains(searchSubgroup))
                    return false;
            }
            return true;
        }

        public void timetableAdd_Method(object paramert)
        {
            if (IsValid(ValidatesAddProperties, out AddErrors))
            {
                MessageBox.Show(timetableModel.Add(TimetableModel.getTimetableObject(AddAudienceNumber,AddGroupId, AddSubgroup,
                AddTeacherId,AddShortPairtypeName, int.Parse(AddPairNumber), AddWeekNumber, AddShortSubjectName, AddDayNumber)), "Результат добавления");
                FilterTimetable();
            }
            else
                MessageBox.Show("Заполните поля корректно!", "Результат добавления");
        }

        private void timetableChange_Method(object parametr)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    if (timetableModel.IsExist(TimetableModel.getTimetableObject(changeAudienceNumber, changeGroupId, changeSubgroup,
                        changeTeacherId, changeShortPairtypeName, int.Parse(changePairNumber), changeWeekNumber, changeShortSubjectName, changeDayNumber)))
                    {
                        EditButtonContent = "Сохранить";
                        CancelVisibility = Visibility.Visible;
                        changedTimetable = TimetableModel.getTimetableObject(changeAudienceNumber, changeGroupId, changeSubgroup,
                        changeTeacherId, changeShortPairtypeName, int.Parse(changePairNumber), changeWeekNumber, changeShortSubjectName, changeDayNumber);
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
                    MessageBox.Show(timetableModel.Change(changedTimetable, TimetableModel.getTimetableObject(changeAudienceNumber, changeGroupId,
                    changeSubgroup, changeTeacherId, changeShortPairtypeName, int.Parse(changePairNumber), changeWeekNumber, changeShortSubjectName, changeDayNumber)), "Результат изменения");
                    EditButtonContent = "Изменить";
                    CancelVisibility = Visibility.Collapsed;
                    FilterTimetable();
                }
                else
                    MessageBox.Show("Заполните корректно поля!", "Результат изменения");
            }
        }

        private void timetableCancel_Method(object parametr)
        {
            EditButtonContent = "Изменить";
            CancelVisibility = Visibility.Collapsed;
        }

        private void timetableRemove_Method(object parametr)
        {
            if (editButtonContent.Equals("Изменить"))
            {
                if (IsValid(ValidatesChangeProperties, out ChangeErrors))
                {
                    MessageBox.Show(timetableModel.Remove(TimetableModel.getTimetableObject(changeAudienceNumber, 
                    changeGroupId, changeSubgroup, changeTeacherId, changeShortPairtypeName, int.Parse(changePairNumber), changeWeekNumber, changeShortSubjectName, changeDayNumber)), "Результат удаления");
                    FilterTimetable();
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

        public TimetableCourseProject Context { get => context; set => context = value; }

        string[] ValidatesAddProperties =
        {
            "AddDayNumber",
            "AddPairNumber",
            "AddWeekNumber",
            "AddGroupId",
            "AddSubgroup",
            "AddShortSubjectName",
            "AddAudienceNumber",
            "AddTeacherId",
            "AddShortPairtypeName"
        };

        string[] ValidatesChangeProperties =
        {
            "ChangeDayNumber",
            "ChangePairNumber",
            "ChangeWeekNumber",
            "ChangeGroupId",
            "ChangeSubgroup",
            "ChangeShortSubjectName",
            "ChangeAudienceNumber",
            "ChangeTeacherId",
            "ChangeShortPairtypeName"
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
                case "AddDayNumber":

                    if (string.IsNullOrWhiteSpace(AddDayNumber))
                        return EmptyString();
                    return null;

                case "ChangeDayNumber":

                    if (string.IsNullOrWhiteSpace(changeDayNumber))
                        return EmptyString();
                    return null;

                case "AddPairNumber":

                    if (string.IsNullOrWhiteSpace(AddPairNumber))
                        return EmptyString();
                    return null;

                case "ChangePairNumber":

                    if (string.IsNullOrWhiteSpace(changePairNumber))
                        return EmptyString();
                    return null;

                case "AddWeekNumber":

                    if (string.IsNullOrWhiteSpace(AddWeekNumber))
                        return EmptyString();
                    return null;

                case "ChangeWeekNumber":

                    if (string.IsNullOrWhiteSpace(changeWeekNumber))
                        return EmptyString();
                    return null;

                case "AddGroupId":

                    if (string.IsNullOrWhiteSpace(AddGroupId))
                        return EmptyString();
                    return null;

                case "ChangeGroupId":

                    if (string.IsNullOrWhiteSpace(changeGroupId))
                        return EmptyString();
                    return null;

                case "AddSubgroup":

                    if (string.IsNullOrWhiteSpace(AddSubgroup))
                        return EmptyString();
                    return null;

                case "ChangeSubgroup":

                    if (string.IsNullOrWhiteSpace(changeSubgroup))
                        return EmptyString();
                    return null;

                case "AddShortSubjectName":

                    if (string.IsNullOrWhiteSpace(AddShortSubjectName))
                        return EmptyString();
                    return null;

                case "ChangeShortSubjectName":

                    if (string.IsNullOrWhiteSpace(changeShortSubjectName))
                        return EmptyString();
                    return null;

                case "AddAudienceNumber":

                    if (string.IsNullOrWhiteSpace(AddAudienceNumber))
                        return EmptyString();
                    return null;

                case "ChangeAudienceNumber":

                    if (string.IsNullOrWhiteSpace(changeAudienceNumber))
                        return EmptyString();
                    return null;

                case "AddTeacherId":

                    if (string.IsNullOrWhiteSpace(AddTeacherId))
                        return EmptyString();
                    return null;

                case "ChangeTeacherId":

                    if (string.IsNullOrWhiteSpace(changeTeacherId))
                        return EmptyString();
                    return null;

                case "AddShortPairtypeName":

                    if (string.IsNullOrWhiteSpace(AddShortPairtypeName))
                        return EmptyString();
                    return null;

                case "ChangeShortPairtypeName":

                    if (string.IsNullOrWhiteSpace(changeShortPairtypeName))
                        return EmptyString();
                    return null;

                default: return null;
            }
        }
        #endregion 
    }
}

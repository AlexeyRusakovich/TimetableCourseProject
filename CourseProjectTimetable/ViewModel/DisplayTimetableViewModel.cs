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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Reflection;

namespace CourseProjectTimetable.ViewModel
{
    public class DisplayTimetableViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public DisplayTimetableViewModel()
        {
            this.context = new TimetableCourseProject();
            DayNumber = new ObservableCollection<DayNumbers>(context.DayNumbers.Select( d => d).ToList());
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
            validator = new BaseViewModel();
        }

        #region Properties

        #region Other

        BaseViewModel validator;

        Font font;

        ObservableCollection<DayNumbers> DayNumber;

        Uri baseUri = new Uri(Assembly.GetEntryAssembly().Location);
        public Uri Uri { get; set; }
        private object documentSource;
        public object DocumentSource
        {
            get { return documentSource; }
            set
            {
                documentSource = value;
                OnPropertyChanged();
            }
        }

        private TimetableCourseProject context;

        #endregion // Other

        #region StreamProp

        private string selectedCourse;
        private string selectedFaculty;
        private Visibility courseVisibility = Visibility.Collapsed;
        private ObservableCollection<int> filteredCourses;

        public ObservableCollection<int> FilteredCourses
        {
            get { return filteredCourses; }
            set
            {
                filteredCourses = value;
                OnPropertyChanged();
            }
        }

        public Visibility CourseVisibility
        {
            get { return courseVisibility; }
            set
            {
                courseVisibility = value;
                OnPropertyChanged();
            }
        }
        public string SelectedCourse
        {
            get => selectedCourse;
            set
            {
                selectedCourse = value;
                OnPropertyChanged();
            }
        }
        public string SelectedFaculty
        {
            get => selectedFaculty;
            set
            {
                selectedFaculty = value;
                OnPropertyChanged();
                CourseVisibility = ElementVisibility(selectedFaculty, selectedCourse);

                FilteredCourses = new ObservableCollection<int>(
                                    (from groups in context.Groups
                                     where groups.Specialities.ShortFacultyName.Equals(selectedFaculty)
                                     select groups.Course).Distinct());
            }
        }

        #endregion // StreamProp

        #region GroupProp

        private Visibility specialityVisibility_ = Visibility.Collapsed;
        private Visibility courseVisibility_ = Visibility.Collapsed;
        private Visibility groupsViibility = Visibility.Collapsed;
        private ObservableCollection<int> filteredGroups;
        private ObservableCollection<Specialities> filteredSpecialities_;
        private ObservableCollection<int> filteredCourses_;

        public ObservableCollection<int> FilteredGroups
        {
            get { return filteredGroups; }
            set
            {
                filteredGroups = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Specialities> FilteredSpecialities_
        {
            get { return filteredSpecialities_; }
            set
            {
                filteredSpecialities_ = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<int> FilteredCourses_
        {
            get { return filteredCourses_; }
            set
            {
                filteredCourses_ = value;
                OnPropertyChanged();
            }
        }
        public Visibility SpecialityVisibility_
        {
            get { return specialityVisibility_; }
            set
            {
                specialityVisibility_ = value;
                OnPropertyChanged();
            }
        }
        public Visibility CourseVisibility_
        {
            get { return courseVisibility_; }
            set
            {
                courseVisibility_ = value;
                OnPropertyChanged();
            }
        }
        public Visibility GroupsViibility
        {
            get { return groupsViibility; }
            set
            {
                groupsViibility = value;
                OnPropertyChanged();
            }
        }

        private string selectedGroupCourse;
        private string selectedGroupSpeciality;
        private string selectedGroupFaculty;
        private string selectedGroupNumber;

        public string SelectedGroupCourse
        {
            get => selectedGroupCourse;
            set
            {
                selectedGroupCourse = value;
                OnPropertyChanged();

                GroupsViibility = ElementVisibility(selectedGroupCourse, null);

                FilteredGroups = new ObservableCollection<int>(
                    (from groups in context.Groups
                     where groups.Specialities.ShortName.Equals(selectedGroupSpeciality)
                     select groups.Number));
            }
        }
        public string SelectedGroupSpeciality
        {
            get => selectedGroupSpeciality;
            set
            {
                selectedGroupSpeciality = value;
                OnPropertyChanged();

                CourseVisibility_ = ElementVisibility(selectedGroupSpeciality, selectedGroupCourse);

                FilteredCourses_ = new ObservableCollection<int>(
                                    (from groups in context.Groups
                                     where groups.Specialities.ShortName.Equals(selectedGroupSpeciality)
                                     select groups.Course).Distinct());
            }
        }
        public string SelectedGroupFaculty
        {
            get => selectedGroupFaculty;
            set
            {
                selectedGroupFaculty = value;
                OnPropertyChanged();

                SpecialityVisibility_ = ElementVisibility(selectedGroupFaculty, selectedGroupSpeciality);
                FilteredSpecialities_ = new ObservableCollection<Specialities>(

                    from specialities in context.Specialities
                    where specialities.ShortFacultyName.Equals(selectedGroupFaculty)
                    select specialities);
            }
        }
        public string SelectedGroupNumber
        {
            get => selectedGroupNumber;
            set
            {
                selectedGroupNumber = value;
                OnPropertyChanged();
            }
        }
        #endregion //GroupsProp

        #region TeacherProp

        private string selectedPulpit;

        public string SelectedPulpit
        {
            get => selectedPulpit;
            set
            {
                selectedPulpit = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion // TeacherProp

        #endregion

        #region Commands

        private Command generateForStream;
        public Command GenerateForStream
        {
            get
            {
                if (generateForStream == null)
                    generateForStream = new Command(GenerateForStream_Method);
                return generateForStream;
            }
        }

        private Command generateForTeachers;
        public Command GenerateForTeachers
        {
            get
            {
                if (generateForTeachers == null)
                    generateForTeachers = new Command(GenerateForTeachers_Method);
                return generateForTeachers;
            }
        }

        private Command generateForGroup;
        public Command GenerateForGroup
        {
            get
            {
                if (generateForGroup == null)
                    generateForGroup = new Command(GenerateForGroup_Method);
                return generateForGroup;
            }
        }

        #endregion // Commands

        #region Methods
        public Visibility ElementVisibility(string someString = "", string oneMoreString = "")
        {
            if (string.IsNullOrWhiteSpace(someString))
            {
                oneMoreString = "";
                return Visibility.Collapsed;
            }
            else
                return Visibility.Visible;
        }

        #region StreamTimetable
        private void GenerateForStream_Method(object obj)
        {
            IQueryable<Groups> SelectedGroup;

            if (IsValid(ValidatesStreamProperties))
            {
                if ((SelectedGroup = context.Groups.Where(g => g.Specialities.ShortFacultyName.Equals(selectedFaculty) &&
                                                                g.Course.ToString().Equals(selectedCourse)).OrderBy(g => g.Number)).Count() != 0)
                {

                    int groupsCount = SelectedGroup.Count();
                    bool isDayNumberWritten = false;

                    using (var doc = new Document())
                    using (var stream = new FileStream("Stream.pdf", FileMode.Create))
                    {
                        doc.SetPageSize(SetPageSize(groupsCount));

                        PdfWriter.GetInstance(doc, stream);
                        doc.Open();

                        PrintGroupsTitle(doc, groupsCount, SelectedGroup);

                        PdfPTable t = new PdfPTable(groupsCount * 2 + 4);
                        t.SetTotalWidth(ReturnTotalWidthGroups(groupsCount));


                        foreach (var dayNumber in DayNumber)
                        {
                            if (IsGroupHavePair(dayNumber.Id, SelectedGroup))
                            {
                                AddDayNumberGroup(dayNumber, SelectedGroup, t);

                                foreach (var pairNumber in context.PairsNumber)

                                    if (IsAnyGroupHavePair(dayNumber.Id, pairNumber.Number, SelectedGroup))
                                    {
                                        AddPairNumber(pairNumber, t);

                                        foreach (var group in SelectedGroup)
                                            AddCellFor1stWeek(group, t, dayNumber.Id, pairNumber.Number);

                                        AddPairNumber(pairNumber, t);

                                        if (!isDayNumberWritten)
                                        {
                                            AddDayNumberGroup(dayNumber, SelectedGroup, t);
                                            isDayNumberWritten = true;
                                        }
                                        foreach (var group in SelectedGroup)
                                            AddCellFor2ndWeek(group, t, dayNumber.Id, pairNumber.Number);
                                    }

                                isDayNumberWritten = false;
                            }
                        }

                        t.HorizontalAlignment = 1;
                        doc.Add(t);
                        doc.Close();
                    }
                    Uri = new Uri(baseUri, Path.GetFullPath("Stream.pdf"));
                    DocumentSource = Uri;
                }
                else
                    MessageBox.Show("В базе данных нет групп факультета: " + selectedFaculty
                                    + ", " + selectedGroupNumber + " курса");
            }
            else
                MessageBox.Show("Заполните корректно поля!");
        }
        private bool IsAnyGroupHavePair(int dayNumber, int pairNumber, IQueryable<Groups> groups)
        {
            foreach (var group in groups)
                foreach (var t in context.Timetable)
                    if (t.DayNumberId.Equals(dayNumber) && t.GroupId.Equals(group.Id) && t.PairNumber == pairNumber)
                        return true;
            return false;
        }
        private Rectangle SetPageSize(int count)
        {
            if (count < 2)
                return PageSize.A4;
            else if (count == 2)
                return PageSize.A3;
            else if (count < 5)
                return PageSize.A2;
            else if (count < 7)
                return PageSize.A1;
            else
                return PageSize.A0;
        }
        private void PrintGroupsTitle(Document doc, int groupsCount, IQueryable<Groups> selectedGroup)
        {
            Paragraph title = new Paragraph("Расписание занятий для группы\n факультета: " + selectedFaculty + ", "
                                                                        + selectedCourse + " курса", font)
            {
                Alignment = 1
            };
            doc.Add(title);
            doc.AddAuthor("Alexey Rusakovich");
            doc.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(groupsCount * 2 + 4);
            table.SetTotalWidth(ReturnTotalWidthGroups(groupsCount));
            table.AddCell(new PdfPCell(new Phrase("Дни", font)) { HorizontalAlignment = 1 });
            table.AddCell(new PdfPCell(new Phrase("Часы", font)) { HorizontalAlignment = 1 });

            foreach (var group in selectedGroup)
            {
                table.AddCell(new PdfPCell(new Phrase(group.Id, font))
                {
                    Colspan = 2,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            }

            table.AddCell(new PdfPCell(new Phrase("Часы", font)) { HorizontalAlignment = 1 });
            table.AddCell(new PdfPCell(new Phrase("Дни", font)) { HorizontalAlignment = 1 });

            doc.Add(table);
        }
        private float[] ReturnTotalWidthGroups(int count)
        {
            List<float> list = new List<float>() { 2, 4 };
            for (int i = 0; i < count * 2; i++)
                list.Add(6);
            list.Add(4);
            list.Add(2);
            return list.ToArray<float>();
        }

        #endregion

        #region TeachersTimetable 
        private void GenerateForTeachers_Method(object obj)
        {
            IQueryable<Teachers> SelectedTeachers;

            if (IsValid(ValidatesTeachersProperties))
            {
                if ((SelectedTeachers = context.Teachers.Where(t => t.ShortPulpitName.Equals(selectedPulpit))).Count() != 0)
                {

                    int teachersCount = SelectedTeachers.Count();
                    bool isDayNumberWritten = false;

                    using (var doc = new Document())
                    using (var stream = new FileStream("Teachers.pdf", FileMode.Create))
                    {

                        SetPageSize(doc, teachersCount);

                        PdfWriter.GetInstance(doc, stream);
                        doc.Open();

                        PrintTeachersTitle(doc, teachersCount);

                        PdfPTable t = new PdfPTable(teachersCount + 4);
                        t.SetTotalWidth(ReturnTotalWidth(teachersCount));


                        foreach (var dayNumber in DayNumber)
                        {
                            if (IsAnyTeacherHavePair(dayNumber.Id, SelectedTeachers))
                            {
                                AddDayNumber(dayNumber, SelectedTeachers, t);

                                foreach (var pairNumber in context.PairsNumber)

                                    if (IsAnyTeacherHavePair(dayNumber.Id, pairNumber.Number, SelectedTeachers))
                                    {
                                        AddPairNumber(pairNumber, t);

                                        foreach (var teacher in SelectedTeachers)
                                            AddCellFor1stWeek(teacher, t, dayNumber.Id, pairNumber.Number);

                                        AddPairNumber(pairNumber, t);

                                        if (!isDayNumberWritten)
                                        {
                                            AddDayNumber(dayNumber, SelectedTeachers, t);
                                            isDayNumberWritten = true;
                                        }

                                        foreach (var teacher in SelectedTeachers)
                                            AddCellFor2ndWeek(teacher, t, dayNumber.Id, pairNumber.Number);
                                    }

                                isDayNumberWritten = false;
                            }
                        }

                        t.HorizontalAlignment = 1;
                        doc.Add(t);
                        doc.Close();
                    }
                    Uri = new Uri(baseUri, Path.GetFullPath("Teachers.pdf"));
                    DocumentSource = Uri;
                }
                else
                    MessageBox.Show("В базе данных нет преподавателей кафедры " + selectedPulpit + " !");
            }
            else
                MessageBox.Show("Заполните корректно поля!");
        }
        private void SetPageSize(Document doc, int teachersCount)
        {
            if (teachersCount < 3)
                doc.SetPageSize(PageSize.A4);
            else if (teachersCount < 5)
                doc.SetPageSize(PageSize.A3.Rotate());
            else if (teachersCount < 10)
                doc.SetPageSize(PageSize.A2.Rotate());
            else
                doc.SetPageSize(PageSize.A1.Rotate());
        }
        private void PrintTeachersTitle(Document doc, int teachersCount)
        {
            Paragraph title = new Paragraph("Расписание занятий для преподавателей кафедры " + selectedPulpit + "\nБелорусского государственного технологического университета\n", font)
            {
                Alignment = 1
            };
            doc.Add(title);
            doc.AddAuthor("Alexey Rusakovich");
            doc.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(teachersCount + 4);
            table.SetTotalWidth(ReturnTotalWidth(teachersCount));
            table.AddCell(new PdfPCell(new Phrase("Дни", font)) { HorizontalAlignment = 1 });
            table.AddCell(new PdfPCell(new Phrase("Часы", font)) { HorizontalAlignment = 1 });

            foreach (var teacher in context.Teachers.Where(t => t.ShortPulpitName.Equals(selectedPulpit)))
            {
                table.AddCell(new PdfPCell(new Phrase(teacher.Id, font)) { HorizontalAlignment = 1 });
            }

            table.AddCell(new PdfPCell(new Phrase("Часы", font)) { HorizontalAlignment = 1 });
            table.AddCell(new PdfPCell(new Phrase("Дни", font)) { HorizontalAlignment = 1 });

            doc.Add(table);
        }
        private bool IsAnyTeacherHavePair(int dayNumber, int pairNumber, IQueryable<Teachers> selectedTeachers)
        {
            foreach (var teacher in selectedTeachers)
            {
                if (context.Timetable.Where(t => t.TeacherId.Equals(teacher.Id) &&
                                                 t.PairNumber == pairNumber &&
                                                 t.DayNumberId.Equals(dayNumber)).Count() != 0)
                    return true;
            }
            return false;
        }
        private bool IsAnyTeacherHavePair(int dayNumber, IQueryable<Teachers> selectedTeachers)
        {
            foreach (var teacher in selectedTeachers)
                if (context.Timetable.Where(t => t.TeacherId.Equals(teacher.Id) &&
                 t.DayNumberId.Equals(dayNumber)).OrderBy(t => t.PairNumber).Count() > 0)
                    return true;
            return false;
        }
        private void AddCellFor1stWeek(Teachers teacher, PdfPTable table, int DayNumber, int pairNumber)
        {

            IQueryable<Timetable> query = context.Timetable.Where(t => t.DayNumberId.Equals(DayNumber) &&
                                                          t.PairNumber == pairNumber &&
                                                          t.TeacherId.Equals(teacher.Id));
            Timetable timetable;

            int pairCount = query.GroupBy(t => t.WeekNumber).Count();

            if (pairCount == 2)
            {
                timetable = query.Where(t => t.WeekNumber.Equals("I")).First();
                table.AddCell(new PdfPCell(new Phrase(
                        timetable.WeekNumber + ' ' +
                        timetable.ShortPairtypeName + ' ' +
                        timetable.AudienceNumber + ' ' +
                        timetable.ShortSubjectName, font))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
            }
            else if (pairCount == 1)
            {
                timetable = query.First();

                if (timetable.WeekNumber == null)
                    table.AddCell(new PdfPCell(new Phrase(
                        timetable.ShortPairtypeName + ' ' +
                        timetable.AudienceNumber + ' ' +
                        timetable.ShortSubjectName, font))
                    {
                        Rowspan = 2,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });

                else
                    table.AddCell(new PdfPCell(new Phrase(
                        timetable.WeekNumber + ' ' +
                        timetable.ShortPairtypeName + ' ' +
                        timetable.AudienceNumber + ' ' +
                        timetable.ShortSubjectName, font))
                    {
                        Rowspan = 2,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
            }
            else
                table.AddCell(new PdfPCell() { Rowspan = 2 });
        }
        private void AddCellFor2ndWeek(Teachers teacher, PdfPTable table, int DayNumber, int pairNumber)
        {
            IQueryable<Timetable> query = context.Timetable.Where(t => t.DayNumberId.Equals(DayNumber) &&
                                                          t.PairNumber == pairNumber &&
                                                          t.TeacherId.Equals(teacher.Id)
                                                    );
            Timetable timetable;

            

            int pairCount = query.GroupBy(t => t.WeekNumber).Count();

            if (pairCount == 2)
            {

                timetable = query.Where(t => t.WeekNumber.Equals("II")).First();
                table.AddCell(new PdfPCell(new Phrase(
                        timetable.WeekNumber + ' ' +
                        timetable.ShortPairtypeName + ' ' +
                        timetable.AudienceNumber + ' ' +
                        timetable.ShortSubjectName, font))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
            }
        }
        private void AddDayNumber(DayNumbers dayNumber, IQueryable<Teachers> SelectedTeachers, PdfPTable t)
        {
            t.AddCell(new PdfPCell(new Phrase(dayNumber.DayNumberName, font))
            {
                Rowspan = 2 * MaxPairsInADay(dayNumber.Id, SelectedTeachers),
                Rotation = 90,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER,

            });
        }
        private void AddPairNumber(PairsNumber pairNumber, PdfPTable t)
        {
            t.AddCell(new PdfPCell(new Phrase(pairNumber.Start + '-' + '\n' + pairNumber.End, font))
            {
                Rowspan = 2,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });
        }
        private int MaxPairsInADay(int dayNumber, IQueryable<Teachers> selectedTeachers)
        {
            var query = from teacher in selectedTeachers
                        join pair in context.Timetable
                        on teacher.Id equals pair.TeacherId
                        where pair.DayNumberId.Equals(dayNumber)
                        select pair;
            return query.GroupBy(t => t.PairNumber).Count();
        }
        private float[] ReturnTotalWidth(int teachersCount)
        {
            List<float> list = new List<float>
            {
                1,
                1.2f
            };
            if(teachersCount < 3)
                for (int i = 0; i < teachersCount; i++)
                    list.Add(10 / teachersCount);
            else
                for (int i = 0; i < teachersCount; i++)
                    list.Add(20 / teachersCount);

            list.Add(1.2f);
            list.Add(1);
            return list.ToArray<float>();
        }
        #endregion

        #region GroupsTimetable

        private void GenerateForGroup_Method(object obj)
        {
            IQueryable<Groups> SelectedGroup;

            if (IsValid(ValidatesGroupProperties))
            {
                if ((SelectedGroup = context.Groups.Where(g => g.Specialities.ShortFacultyName.Equals(selectedGroupFaculty) &&
                                                                g.Specialities.ShortName.Equals(selectedGroupSpeciality) &&
                                                                g.Course.ToString().Equals(selectedGroupCourse) &&
                                                                g.Number.ToString().Equals(selectedGroupNumber))).Count() != 0)
                {

                    int groupsCount = SelectedGroup.Count();
                    bool isDayNumberWritten = false;

                    using (var doc = new Document())
                    using (var stream = new FileStream("Group.pdf", FileMode.Create))
                    {
                        doc.SetPageSize(PageSize.A3);

                        PdfWriter.GetInstance(doc, stream);
                        doc.Open();

                        PrintGroupTitle(doc, groupsCount, SelectedGroup);

                        PdfPTable t = new PdfPTable(groupsCount * 2 + 4);
                        t.SetTotalWidth(ReturnTotalWidthGroup());


                        foreach (var dayNumber in DayNumber)
                        {
                            if (IsGroupHavePair(dayNumber.Id, SelectedGroup))
                            {
                                AddDayNumberGroup(dayNumber, SelectedGroup, t);

                                foreach (var pairNumber in context.PairsNumber)

                                    if (IsAnySubgroupHavePair(dayNumber.Id, pairNumber.Number, SelectedGroup.First()))
                                    {
                                        AddPairNumber(pairNumber, t);

                                        AddCellFor1stWeek(SelectedGroup.First(), t, dayNumber.Id, pairNumber.Number);

                                        AddPairNumber(pairNumber, t);

                                        if (!isDayNumberWritten)
                                        {
                                            AddDayNumberGroup(dayNumber, SelectedGroup, t);
                                            isDayNumberWritten = true;
                                        }

                                        AddCellFor2ndWeek(SelectedGroup.First(), t, dayNumber.Id, pairNumber.Number);
                                    }

                                isDayNumberWritten = false;
                            }
                        }

                        t.HorizontalAlignment = 1;
                        doc.Add(t);
                        doc.Close();
                    }
                    Uri = new Uri(baseUri, Path.GetFullPath("Group.pdf"));
                    DocumentSource = Uri;
                }
                else
                    MessageBox.Show("В базе данных нет группы факультета: " + selectedGroupFaculty + ", специальности: "
                                    + selectedGroupSpeciality + ", " + selectedGroupNumber + " курса, " + selectedGroupNumber + " группы");
            }
            else
                MessageBox.Show("Заполните корректно поля!");
        }
        private float[] ReturnTotalWidthGroup()
        {
            return new float[] { 1, 2, 5, 5, 2, 1 };
        }
        private void AddCellFor1stWeek(Groups group, PdfPTable table, int dayNumber, int pairNumber)
        {
            IQueryable<Timetable> timetable = context.Timetable.Where(t => t.DayNumberId.Equals(dayNumber) &&
                                               t.GroupId.Equals(group.Id) &&
                                               t.PairNumber == pairNumber);

            IQueryable<Timetable> noSubgroupNumber = timetable.Where(t => t.Subgroup == null);
            IQueryable<Timetable> noWeekNumber = timetable.Where(t => t.WeekNumber == null);
            IQueryable<Timetable> tmtbl;
            Timetable tbl;
            
            if (noWeekNumber.Count() == 2) // If subgroups have in 2 Weeks different pairs
            {
                tbl = noWeekNumber.Where(t => t.Subgroup.Equals("I")).First();
                table.AddCell(new PdfPCell(new Phrase(
                    tbl.ShortPairtypeName + ' ' +
                   tbl.AudienceNumber + ' ' +
                   tbl.ShortSubjectName + ", " +
                    tbl.TeacherId, font
                    ))
                {
                    Rowspan = 2,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
                tbl = noWeekNumber.Where(t => t.Subgroup.Equals("II")).First();
                table.AddCell(new PdfPCell(new Phrase(
                    tbl.ShortPairtypeName + ' ' +
                    tbl.AudienceNumber + ' ' +
                    tbl.ShortSubjectName + ", " +
                    tbl.TeacherId, font

                    ))
                {
                    Rowspan = 2,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
                return;
            }
            else if (noWeekNumber.Count() == 1) // If one of subgroup have one pair in 2 weeks or 2 subgroups have joint pair
            {
                if (noWeekNumber.Where(t => t.Subgroup == null && t.WeekNumber == null).Count() == 1) // If 2 subgroups have joint(совместное) pair
                {
                    tbl = noWeekNumber.Where(t => t.Subgroup == null && t.WeekNumber == null).First();
                    table.AddCell(new PdfPCell(new Phrase(
                        tbl.ShortPairtypeName + ' ' +
                        tbl.AudienceNumber + ' ' +
                        tbl.ShortSubjectName + ", " +
                        tbl.TeacherId, font
                        ))
                    {
                        Rowspan = 2,
                        Colspan = 2,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                    return;
                }
                else if (noWeekNumber.Where(t => t.Subgroup.Equals("I")).Count() == 1) // If 1st subgroup have pair in 2 weeks
                {
                    table.AddCell(new PdfPCell(new Phrase(
                                                            noWeekNumber.First().ShortPairtypeName + ' ' +
                                                            noWeekNumber.First().AudienceNumber + ' ' +
                                                            noWeekNumber.First().ShortSubjectName + ", " +
                                                            noWeekNumber.First().TeacherId, font
                                                         ))
                    {
                        Rowspan = 2,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                    if ((tmtbl = timetable.Where(t => t.Subgroup.Equals("II"))).Count() == 2)
                        table.AddCell(new PdfPCell(new Phrase(
                            tmtbl.First().WeekNumber + ' ' +
                            tmtbl.First().ShortPairtypeName + ' ' +
                            tmtbl.First().AudienceNumber + ' ' +
                            tmtbl.First().ShortSubjectName + ", " +
                            tmtbl.First().TeacherId, font))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    else if ((tmtbl = timetable.Where(t => t.Subgroup.Equals("II"))).Count() == 1)

                        if (tmtbl.Where(t => t.WeekNumber.Equals("I")).Count() == 1)
                        {
                            table.AddCell(new PdfPCell(new Phrase(
                            tmtbl.First().WeekNumber + ' ' +
                            tmtbl.First().ShortPairtypeName + ' ' +
                            tmtbl.First().AudienceNumber + ' ' +
                            tmtbl.First().ShortSubjectName + ", " +
                            tmtbl.First().TeacherId, font))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                VerticalAlignment = Element.ALIGN_MIDDLE
                            });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase("")));

                    else
                        table.AddCell(new PdfPCell(new Phrase("")) { Rowspan = 2 });

                }
                else if (noWeekNumber.Where(t => t.Subgroup.Equals("II")).Count() == 1) // If 2nd subgroup have pair in 2 weeks
                {

                    if ((tmtbl = timetable.Where(t => t.Subgroup.Equals("I"))).Count() == 2)  // If 1st subgrop have pair in 1st week
                        table.AddCell(new PdfPCell(new Phrase(
                          tmtbl.First().WeekNumber + ' ' +
                          tmtbl.First().ShortPairtypeName + ' ' +
                          tmtbl.First().AudienceNumber + ' ' +
                          tmtbl.First().ShortSubjectName + ", " +
                          tmtbl.First().TeacherId, font
                          ))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    else if ((tmtbl = timetable.Where(t => t.Subgroup.Equals("I"))).Count() == 1)
                    {
                        if (tmtbl.Where(t => t.WeekNumber.Equals("I")).Count() == 1)
                            table.AddCell(new PdfPCell(new Phrase(
                              tmtbl.First().WeekNumber + ' ' +
                              tmtbl.First().ShortPairtypeName + ' ' +
                              tmtbl.First().AudienceNumber + ' ' +
                              tmtbl.First().ShortSubjectName + ", " +
                              tmtbl.First().TeacherId, font
                              ))
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    VerticalAlignment = Element.ALIGN_MIDDLE
                                });
                        else
                            table.AddCell(new PdfPCell(new Phrase("")));
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase()) { Rowspan = 2 });

                    tbl = noWeekNumber.Where(t => t.Subgroup.Equals("II")).First();

                    table.AddCell(new PdfPCell(new Phrase(
                          tbl.WeekNumber + ' ' +
                          tbl.ShortPairtypeName + ' ' +
                          tbl.AudienceNumber + ' ' +
                          tbl.ShortSubjectName + ", " +
                          tbl.TeacherId, font
                          ))
                    {
                        Rowspan = 2,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }

            }
            else if (noSubgroupNumber.Count() != 0)
            {
                if (noSubgroupNumber.Where(t => t.WeekNumber.Equals("I")).Count() != 0)
                {
                    tbl = noSubgroupNumber.Where(t => t.WeekNumber.Equals("I")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                              tbl.WeekNumber + ' ' +
                              tbl.ShortPairtypeName + ' ' +
                              tbl.AudienceNumber + ' ' +
                              tbl.ShortSubjectName + ", " +
                              tbl.TeacherId, font
                              ))
                    {
                        Colspan = 2,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
            }
            else if (timetable.Where(t => t.WeekNumber.Equals("I")).Count() == 0 &&
                     timetable.Where(t => t.WeekNumber.Equals("II")).Count() == 0)
            {
                table.AddCell(new PdfPCell(new Phrase("")) { Rowspan = 2, Colspan = 2 });
            }
            else if (timetable.Where(t => t.Subgroup.Equals("I")).Count() == 0)
            {
                tmtbl = timetable.Where(t => t.Subgroup.Equals("II"));
                table.AddCell(new PdfPCell(new Phrase("")) { Rowspan = 2 });
                if (tmtbl.Count() == 2)
                {
                    tbl = tmtbl.Where(t => t.WeekNumber.Equals("I")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                        tbl.WeekNumber + ' ' +
                        tbl.ShortPairtypeName + ' ' +
                        tbl.AudienceNumber + ' ' +
                        tbl.ShortSubjectName + ", " +
                        tbl.TeacherId, font
                        ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
                else if (tmtbl.Count() == 1)
                {
                    if ((tmtbl = tmtbl.Where(t => t.WeekNumber.Equals("I"))).Count() == 1)
                    {
                        tbl = tmtbl.First();
                        table.AddCell(new PdfPCell(new Phrase(
                            tbl.WeekNumber + ' ' +
                            tbl.ShortPairtypeName + ' ' +
                            tbl.AudienceNumber + ' ' +
                            tbl.ShortSubjectName + ", " +
                            tbl.TeacherId, font
                            ))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase("")));

                }
            }
            else if (timetable.Where(t => t.Subgroup.Equals("II")).Count() == 0)
            {
                tmtbl = timetable.Where(t => t.Subgroup.Equals("I"));
                if (tmtbl.Count() == 2)
                {
                    tbl = tmtbl.Where(t => t.WeekNumber.Equals("I")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                        tbl.WeekNumber + ' ' +
                        tbl.ShortPairtypeName + ' ' +
                        tbl.AudienceNumber + ' ' +
                        tbl.ShortSubjectName + ", " +
                        tbl.TeacherId, font
                        ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
                else if (tmtbl.Count() == 1)
                {
                    if (tmtbl.Where(t => t.WeekNumber.Equals("I")).Count() == 1)
                    {
                        tbl = tmtbl.First();
                        table.AddCell(new PdfPCell(new Phrase(
                            tbl.WeekNumber + ' ' +
                            tbl.ShortPairtypeName + ' ' +
                            tbl.AudienceNumber + ' ' +
                            tbl.ShortSubjectName + ", " +
                            tbl.TeacherId, font
                            ))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase("")));
                }

                table.AddCell(new PdfPCell(new Phrase("")) { Rowspan = 2 });

            }
            else if ((tmtbl = timetable.Where(t => t.WeekNumber.Equals("I"))).Count() == 2)
            {
                tbl = tmtbl.Where(t => t.Subgroup.Equals("I")).First();
                table.AddCell(new PdfPCell(new Phrase(
                            tbl.WeekNumber + ' ' +
                            tbl.ShortPairtypeName + ' ' +
                            tbl.AudienceNumber + ' ' +
                            tbl.ShortSubjectName + ", " +
                            tbl.TeacherId, font
                            ))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
                tbl = tmtbl.Where(t => t.Subgroup.Equals("II")).First();
                table.AddCell(new PdfPCell(new Phrase(
                            tbl.WeekNumber + ' ' +
                            tbl.ShortPairtypeName + ' ' +
                            tbl.AudienceNumber + ' ' +
                            tbl.ShortSubjectName + ", " +
                            tbl.TeacherId, font
                            ))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });

            }
            else if ((tmtbl = timetable.Where(t => t.WeekNumber.Equals("I"))).Count() == 1)
            {
                if (tmtbl.Where(t => t.Subgroup.Equals("I")).Count() == 1)
                {
                    tbl = tmtbl.Where(t => t.Subgroup.Equals("I")).First();

                    table.AddCell(new PdfPCell(new Phrase(
                                tbl.WeekNumber + ' ' +
                                tbl.ShortPairtypeName + ' ' +
                                tbl.AudienceNumber + ' ' +
                                tbl.ShortSubjectName + ", " +
                                tbl.TeacherId, font
                                ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });

                    table.AddCell(new PdfPCell(new Phrase("")));

                }
                else
                {
                    table.AddCell(new PdfPCell(new Phrase("")));

                    tbl = tmtbl.Where(t => t.Subgroup.Equals("II")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                                tbl.WeekNumber + ' ' +
                                tbl.ShortPairtypeName + ' ' +
                                tbl.AudienceNumber + ' ' +
                                tbl.ShortSubjectName + ", " +
                                tbl.TeacherId, font
                                ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }

            }
            else
            {
                table.AddCell(new PdfPCell(new Phrase("")) { Colspan = 2 });
            }
        }
        private void AddCellFor2ndWeek(Groups group, PdfPTable table, int dayNumber, int pairNumber)
        {
            IQueryable<Timetable> timetable = context.Timetable.Where(t => t.DayNumberId.Equals(dayNumber) &&
                                               t.GroupId.Equals(group.Id) &&
                                               t.PairNumber == pairNumber);
            IQueryable<Timetable> noWeekNumber = timetable.Where(t => t.WeekNumber == null);
            IQueryable<Timetable> noSubgroupNumber = timetable.Where(t => t.Subgroup == null);
            IQueryable<Timetable> tmtbl;
            Timetable tbl;

            if (noWeekNumber.Count() == 2) // If subgroups have in 2 Weeks different pairs
            {
                return;
            }
            else if (noWeekNumber.Count() == 1) // If one of subgroup have one pair in 2 weeks or 2 subgroups have joint pair
            {
                if (noWeekNumber.Where(t => t.Subgroup.Equals("I")).Count() == 1) // If 1st subgroup have pair in 2 weeks
                {

                    if ((tmtbl = timetable.Where(t => t.Subgroup.Equals("II"))).Count() == 2)
                    {
                        tbl = tmtbl.Where(t => t.WeekNumber.Equals("II")).First();
                        table.AddCell(new PdfPCell(new Phrase(
                            tbl.WeekNumber + ' ' +
                            tbl.ShortPairtypeName + ' ' +
                            tbl.AudienceNumber + ' ' +
                            tbl.ShortSubjectName + ", " +
                            tbl.TeacherId, font))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    }
                    else if ((tmtbl = timetable.Where(t => t.Subgroup.Equals("II"))).Count() == 1)

                        if (tmtbl.Where(t => t.WeekNumber.Equals("II")).Count() == 1)
                        {
                            table.AddCell(new PdfPCell(new Phrase(
                            tmtbl.First().WeekNumber + ' ' +
                            tmtbl.First().ShortPairtypeName + ' ' +
                            tmtbl.First().AudienceNumber + ' ' +
                            tmtbl.First().ShortSubjectName + ", " +
                            tmtbl.First().TeacherId, font))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                VerticalAlignment = Element.ALIGN_MIDDLE
                            });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase("")));
                    

                }
                else if (noWeekNumber.Where(t => t.Subgroup.Equals("II")).Count() == 1) // If 2nd subgroup have pair in 2 weeks
                {

                    if ((tmtbl = timetable.Where(t => t.Subgroup.Equals("I"))).Count() == 2)  // If 1st subgrop have pair in 1st week
                        table.AddCell(new PdfPCell(new Phrase(
                          tmtbl.First().WeekNumber + ' ' +
                          tmtbl.First().ShortPairtypeName + ' ' +
                          tmtbl.First().AudienceNumber + ' ' +
                          tmtbl.First().ShortSubjectName + ", " +
                          tmtbl.First().TeacherId, font
                          ))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    else if ((tmtbl = timetable.Where(t => t.Subgroup.Equals("I"))).Count() == 1)
                    {
                        if (tmtbl.Where(t => t.WeekNumber.Equals("II")).Count() == 1)
                            table.AddCell(new PdfPCell(new Phrase(
                              tmtbl.First().WeekNumber + ' ' +
                              tmtbl.First().ShortPairtypeName + ' ' +
                              tmtbl.First().AudienceNumber + ' ' +
                              tmtbl.First().ShortSubjectName + ", " +
                              tmtbl.First().TeacherId, font
                              ))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                VerticalAlignment = Element.ALIGN_MIDDLE
                            });
                        else
                            table.AddCell(new PdfPCell(new Phrase("")));
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase()) { Rowspan = 2 });
                }

            }
            else if (noSubgroupNumber.Count() != 0)
            {
                if (noSubgroupNumber.Where(t => t.WeekNumber.Equals("II")).Count() != 0)
                {
                    tbl = noSubgroupNumber.Where(t => t.WeekNumber.Equals("II")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                              tbl.WeekNumber + ' ' +
                              tbl.ShortPairtypeName + ' ' +
                              tbl.AudienceNumber + ' ' +
                              tbl.ShortSubjectName + ", " +
                              tbl.TeacherId, font
                              ))
                    {
                        Colspan = 2,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
                else if((tmtbl = timetable.Where(t => t.WeekNumber.Equals("II"))).Count() == 2)
                {
                    tbl = tmtbl.Where(t => t.Subgroup.Equals("I")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                              tbl.WeekNumber + ' ' +
                              tbl.ShortPairtypeName + ' ' +
                              tbl.AudienceNumber + ' ' +
                              tbl.ShortSubjectName + ", " +
                              tbl.TeacherId, font
                              ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });

                    tbl = tmtbl.Where(t => t.Subgroup.Equals("II")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                              tbl.WeekNumber + ' ' +
                              tbl.ShortPairtypeName + ' ' +
                              tbl.AudienceNumber + ' ' +
                              tbl.ShortSubjectName + ", " +
                              tbl.TeacherId, font
                              ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
                else if ((tmtbl = timetable.Where(t => t.WeekNumber.Equals("II"))).Count() == 1)
                {
                    if((tmtbl = timetable.Where(t=> t.WeekNumber.Equals("II") && t.Subgroup.Equals("I"))).Count() == 1)
                    {
                        tbl = tmtbl.First();
                        table.AddCell(new PdfPCell(new Phrase(
                                  tbl.WeekNumber + ' ' +
                                  tbl.ShortPairtypeName + ' ' +
                                  tbl.AudienceNumber + ' ' +
                                  tbl.ShortSubjectName + ", " +
                                  tbl.TeacherId, font
                                  ))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });

                        table.AddCell(new PdfPCell(new Phrase("")));
                    }
                    else if((tmtbl = timetable.Where(t => t.WeekNumber.Equals("II") && t.Subgroup.Equals("II"))).Count() == 1)
                    {
                        table.AddCell(new PdfPCell(new Phrase("")));

                        tbl = tmtbl.First();
                        table.AddCell(new PdfPCell(new Phrase(
                                  tbl.WeekNumber + ' ' +
                                  tbl.ShortPairtypeName + ' ' +
                                  tbl.AudienceNumber + ' ' +
                                  tbl.ShortSubjectName + ", " +
                                  tbl.TeacherId, font
                                  ))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase("")) { Colspan = 2});
                }
            }
            else if (timetable.Where(t => t.WeekNumber.Equals("I")).Count() == 0 &&
                     timetable.Where(t => t.WeekNumber.Equals("II")).Count() == 0)
            {
                return;
            }
            else if (timetable.Where(t => t.Subgroup.Equals("I")).Count() == 0)
            {
                tmtbl = timetable.Where(t => t.Subgroup.Equals("II"));

                if (tmtbl.Count() == 2)
                {
                    tbl = tmtbl.Where(t => t.WeekNumber.Equals("II")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                        tbl.WeekNumber + ' ' +
                        tbl.ShortPairtypeName + ' ' +
                        tbl.AudienceNumber + ' ' +
                        tbl.ShortSubjectName + ", " +
                        tbl.TeacherId, font
                        ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
                else if (tmtbl.Count() == 1)
                {
                    if (tmtbl.Where(t => t.WeekNumber.Equals("II")).Count() == 1)
                    {
                        tbl = tmtbl.First();
                        table.AddCell(new PdfPCell(new Phrase(
                            tbl.WeekNumber + ' ' +
                            tbl.ShortPairtypeName + ' ' +
                            tbl.AudienceNumber + ' ' +
                            tbl.ShortSubjectName + ", " +
                            tbl.TeacherId, font
                            ))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase("")));

                }
            }
            else if (timetable.Where(t => t.Subgroup.Equals("II")).Count() == 0)
            {
                tmtbl = timetable.Where(t => t.Subgroup.Equals("I"));
                if (tmtbl.Count() == 2)
                {
                    tbl = tmtbl.Where(t => t.WeekNumber.Equals("II")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                        tbl.WeekNumber + ' ' +
                        tbl.ShortPairtypeName + ' ' +
                        tbl.AudienceNumber + ' ' +
                        tbl.ShortSubjectName + ", " +
                        tbl.TeacherId, font
                        ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
                else if (tmtbl.Count() == 1)
                {
                    if (tmtbl.Where(t => t.WeekNumber.Equals("II")).Count() == 1)
                    {
                        tbl = tmtbl.First();
                        table.AddCell(new PdfPCell(new Phrase(
                            tbl.WeekNumber + ' ' +
                            tbl.ShortPairtypeName + ' ' +
                            tbl.AudienceNumber + ' ' +
                            tbl.ShortSubjectName + ", " +
                            tbl.TeacherId, font
                            ))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase("")));
                }

            }
            else if ((tmtbl = timetable.Where(t => t.WeekNumber.Equals("II"))).Count() == 2)
            {
                tbl = tmtbl.Where(t => t.Subgroup.Equals("I")).First();
                table.AddCell(new PdfPCell(new Phrase(
                            tbl.WeekNumber + ' ' +
                            tbl.ShortPairtypeName + ' ' +
                            tbl.AudienceNumber + ' ' +
                            tbl.ShortSubjectName + ", " +
                            tbl.TeacherId, font
                            ))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
                tbl = tmtbl.Where(t => t.Subgroup.Equals("II")).First();
                table.AddCell(new PdfPCell(new Phrase(
                            tbl.WeekNumber + ' ' +
                            tbl.ShortPairtypeName + ' ' +
                            tbl.AudienceNumber + ' ' +
                            tbl.ShortSubjectName + ", " +
                            tbl.TeacherId, font
                            ))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });

            }
            else if ((tmtbl = timetable.Where(t => t.WeekNumber.Equals("II"))).Count() == 1)
            {
                if (tmtbl.Where(t => t.Subgroup.Equals("I")).Count() == 1)
                {
                    tbl = tmtbl.Where(t => t.Subgroup.Equals("I")).First();

                    table.AddCell(new PdfPCell(new Phrase(
                                tbl.WeekNumber + ' ' +
                                tbl.ShortPairtypeName + ' ' +
                                tbl.AudienceNumber + ' ' +
                                tbl.ShortSubjectName + ", " +
                                tbl.TeacherId, font
                                ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });

                    table.AddCell(new PdfPCell(new Phrase("")));

                }
                else
                {
                    table.AddCell(new PdfPCell(new Phrase("")));

                    tbl = tmtbl.Where(t => t.Subgroup.Equals("II")).First();
                    table.AddCell(new PdfPCell(new Phrase(
                                tbl.WeekNumber + ' ' +
                                tbl.ShortPairtypeName + ' ' +
                                tbl.AudienceNumber + ' ' +
                                tbl.ShortSubjectName + ", " +
                                tbl.TeacherId, font
                                ))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }

            }
            else
            {
                table.AddCell(new PdfPCell(new Phrase("")) { Colspan = 2 });
            }
        }
        private bool IsAnySubgroupHavePair(int dayNumber, int pairNumber, Groups SelectedGroup)
        {
            return context.Timetable.Where(t => t.DayNumberId.Equals(dayNumber) &&
                                                t.GroupId.Equals(SelectedGroup.Id) &&
                                                t.PairNumber == pairNumber).Count() != 0;
        }
        private void AddDayNumberGroup(DayNumbers dayNumber, IQueryable<Groups> selectedGroup, PdfPTable t)
        {
            t.AddCell(new PdfPCell(new Phrase(dayNumber.DayNumberName, font))
            {
                Rowspan = 2 * MaxPairsInADayGroup(dayNumber.Id, selectedGroup),
                Rotation = 90,
                HorizontalAlignment = 1,
                VerticalAlignment = 1
            });
        }
        private int MaxPairsInADayGroup(int dayNumber, IQueryable<Groups> groups)
        {
            int maxCount = 0;
            foreach (var group in groups)
                maxCount = maxCount < context.Timetable.Where(t => t.GroupId.Equals(group.Id)
                                                && t.DayNumberId.Equals(dayNumber)).
                                                GroupBy(t => t.PairNumber).Count() ?

                                                context.Timetable.Where(t => t.GroupId.Equals(group.Id)
                                                && t.DayNumberId.Equals(dayNumber)).
                                                GroupBy(t => t.PairNumber).Count() :

                                                maxCount;
            return maxCount;
        }
        private void PrintGroupTitle(Document doc, int groupsCount, IQueryable<Groups> selectedGroup)
        {
            Paragraph title = new Paragraph("Расписание занятий для группы\n факультета: " + selectedGroupFaculty + ", специальности: "
                                    + selectedGroupSpeciality + ", " + selectedGroupCourse + " курса, " + selectedGroupNumber + " группы", font)
            {
                Alignment = 1
            };
            doc.Add(title);
            doc.AddAuthor("Alexey Rusakovich");
            doc.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(groupsCount + 1 + 4);
            table.SetTotalWidth(ReturnTotalWidthGroup());
            table.AddCell(new PdfPCell(new Phrase("Дни", font)) { HorizontalAlignment = 1 });
            table.AddCell(new PdfPCell(new Phrase("Часы", font)) { HorizontalAlignment = 1 });

            foreach (var group in selectedGroup)
            {
                table.AddCell(new PdfPCell(new Phrase(group.Id, font))
                {
                    Colspan = 2,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            }

            table.AddCell(new PdfPCell(new Phrase("Часы", font)) { HorizontalAlignment = 1 });
            table.AddCell(new PdfPCell(new Phrase("Дни", font)) { HorizontalAlignment = 1 });

            doc.Add(table);
        }
        private bool IsGroupHavePair(int DayNumber, IQueryable<Groups> selectedGroup)
        {
            foreach (var group in selectedGroup)
                foreach (var timetable in context.Timetable)
                    if (timetable.GroupId.Equals(group.Id) && timetable.DayNumberId.Equals(DayNumber))
                        return true;
            return false;
        }

        #endregion // GroupsTimetable

        #endregion // Methods

        #region Validate

        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }

        static readonly string[] ValidatesStreamProperties =
        {
            "SelectedCourse",
            "SelectedFaculty"
        };

        static readonly string[] ValidatesGroupProperties =
        {
            "SelectedGroupCourse",
            "SelectedGroupSpeciality",
            "SelectedGroupFaculty",
            "SelectedGroupNumber"
        };

        static readonly string[] ValidatesTeachersProperties =
        {
            "SelectedPulpit"
        };

        private bool IsValid(string[] validatingStrings)
        {
            string Error = null;
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
                case "SelectedCourse":

                    if (String.IsNullOrWhiteSpace(selectedCourse))
                        return EmptyString();
                    return null;

                case "SelectedFaculty":

                    if (String.IsNullOrWhiteSpace(selectedFaculty))
                        return EmptyString();
                    return null;
                case "SelectedGroupCourse":

                    if (String.IsNullOrWhiteSpace(selectedGroupCourse))
                        return EmptyString();
                    return null;
                case "SelectedGroupSpeciality":

                    if (String.IsNullOrWhiteSpace(selectedGroupSpeciality))
                        return EmptyString();
                    return null;
                case "SelectedGroupFaculty":

                    if (String.IsNullOrWhiteSpace(SelectedGroupFaculty))
                        return EmptyString();
                    return null;

                case "SelectedGroupNumber":

                    if (String.IsNullOrWhiteSpace(selectedGroupNumber))
                        return EmptyString();
                    return null;

                case "SelectedPulpit":

                    if (String.IsNullOrWhiteSpace(selectedPulpit))
                        return EmptyString();
                    return null;

                default: return null;
            }
        }
        #endregion // Validate
    }
}
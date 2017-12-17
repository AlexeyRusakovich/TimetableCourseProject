using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProjectTimetable.ViewModel.Commands;
using System.Windows.Input;
using System.Windows;
using System.Data;
using System.Runtime.CompilerServices;
using CourseProjectTimetable.View;
using System.Collections.ObjectModel;
using CourseProject.Models;
using System.Windows.Data;
using CourseProjectTimetable;

namespace CourseProjectTimetable.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public MainViewModel()
        {
            context = new TimetableCourseProject();
            context.Audience.Load();
            context.Timetable.Load();
            context.Subjects.Load();
            context.Groups.Load();
            context.Audience.Load();
            context.Teachers.Load();
            context.PairTypes.Load();
            context.Specialities.Load();
            context.Faculties.Load();
            context.Pulpits.Load();
            context.PairsNumber.Load();
            context.PairTypes.Load();

            DayNumber = new ObservableCollection<string>(new List<string>() { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" });
            Subgroup = new ObservableCollection<string>(new List<string>() { "I", "II", "Нету" });
            WeekNumber = new ObservableCollection<string>(new List<string>() { "I", "II", "По обеим" });
            Corpses = new ObservableCollection<string>(new List<string>() { "1", "2", "3", "3а", "4", "5" });
            PairNumber = context.PairsNumber.Local;
            Timetable = context.Timetable.Local;
            Subjects = context.Subjects.Local;           
            Groups = context.Groups.Local;
            AudienceNumber = context.Audience.Local;
            TeacherId = context.Teachers.Local;
            ShortPairtypeName = context.PairTypes.Local;            
            Specialities = context.Specialities.Local;
            Faculties = context.Faculties.Local;
            Pulpits = context.Pulpits.Local;
        }

        #region Properties

        private ObservableCollection<string> dayNumber;
        private ObservableCollection<PairsNumber> pairNumber;
        private ObservableCollection<string> weekNumber;
        private ObservableCollection<Groups> groups;
        private ObservableCollection<string> subgroup;
        private ObservableCollection<Subjects> subjects;
        private ObservableCollection<Audience> audienceNumber;
        private ObservableCollection<Teachers> teacherId;
        private ObservableCollection<PairTypes> shortPairtypeName;
        private ObservableCollection<Specialities> specialities;
        private ObservableCollection<Faculties> faculties;
        private ObservableCollection<Pulpits> pulpits;
        private ObservableCollection<string> corpses;
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
        public ObservableCollection<string> Corpses
        {
            get { return corpses; }
            set
            {
                corpses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Pulpits> Pulpits
        {
            get { return pulpits; }
            set
            {
                pulpits = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Specialities> Specialities
        {
            get { return specialities; }
            set
            {
                specialities = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Faculties> Faculties
        {
            get { return faculties; }
            set
            {
                faculties = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> DayNumber
        {
            get { return dayNumber; }
            set
            {
                dayNumber = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PairsNumber> PairNumber
        {
            get { return pairNumber; }
            set
            {
                pairNumber = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> WeekNumber
        {
            get { return weekNumber;  }
            set
            {
                weekNumber = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Groups> Groups
        {
            get { return groups;  }
            set
            {
                groups = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Subgroup
        {
            get { return subgroup; }
            set
            {
                subgroup = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Subjects> Subjects
        {
            get { return subjects; }
            set
            {
                subjects = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Audience> AudienceNumber
        {
            get { return audienceNumber; }
            set
            {
                audienceNumber = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Teachers> TeacherId
        {
            get { return teacherId; }
            set
            {
                teacherId = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PairTypes> ShortPairtypeName
        {
            get { return shortPairtypeName; }
            set
            {
                shortPairtypeName = value;
                OnPropertyChanged();
            }
        }


        TimetableCourseProject context;
        AudienceViewModel audienceViewModel;
        FacultiesViewModel facultiesViewModel;
        GroupsViewModel groupsViewModel;
        PulpitsViewModel pulpitsViewModel;
        SpecialitiesViewModel specialitiesViewModel;
        SubjectViewModel subjectsViewModel;
        TeachersViewModel teachersViewModel;
        TimetableViewModel timetableViewModel;

        public AudienceViewModel AudienceViewModel
        {
            get { return audienceViewModel; }
            set
            {
                audienceViewModel = value;
                OnPropertyChanged();
            }
        }
        public FacultiesViewModel FacultiesViewModel
        {
            get { return facultiesViewModel; }
            set
            {
                facultiesViewModel = value;
                OnPropertyChanged();
            }
        }
        public GroupsViewModel GroupsViewModel
        {
            get { return groupsViewModel; }
            set
            {
                groupsViewModel = value;
                OnPropertyChanged();
            }
        }
        public PulpitsViewModel PulpitsViewModel
        {
            get { return pulpitsViewModel; }
            set
            {
                pulpitsViewModel = value;
                OnPropertyChanged();
            }
        }
        public SpecialitiesViewModel SpecialitiesViewModel
        {
            get { return specialitiesViewModel; }
            set
            {
                specialitiesViewModel = value;
                OnPropertyChanged();
            }
        }
        public SubjectViewModel SubjectsViewModel
        {
            get { return subjectsViewModel; }
            set
            {
                subjectsViewModel = value;
                OnPropertyChanged();
            }
        }
        public TeachersViewModel TeachersViewModel
        {
            get { return teachersViewModel; }
            set
            {
                teachersViewModel = value;
                OnPropertyChanged();
            }
        }
        public TimetableViewModel TimetableViewModel
        {
            get { return timetableViewModel; }
            set
            {
                timetableViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion
        
        #region Methods


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        
        #endregion 
    }
}

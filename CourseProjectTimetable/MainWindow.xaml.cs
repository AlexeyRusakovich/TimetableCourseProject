using CourseProjectTimetable.ViewModel;
using CourseProjectTimetable.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProjectTimetable
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel mainModel = new MainViewModel();
            OutputView.Content = timetable;
        }

        TeachersView teachers = new TeachersView();
        GroupsView groups = new GroupsView();
        AudienceView audience = new AudienceView();
        SpecialityView speciality = new SpecialityView();
        SubjectsView subjects = new SubjectsView();
        FacultyView faculty = new FacultyView();
        TimetableView timetable = new TimetableView();
        PulpitView pulpit = new PulpitView();

        private void DatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseTables.Visibility = DatabaseTables.Visibility == Visibility.Visible
                                        ? Visibility.Collapsed
                                        : Visibility.Visible;
        }
        private void TeachersButton_Click(object sender, RoutedEventArgs e)
        {
            OutputView.Content = teachers;
        }
        private void PulpitButton_Click(object sender, RoutedEventArgs e)
        {
            OutputView.Content = pulpit;
        }
        private void GroupsButton_Click(object sender, RoutedEventArgs e)
        {
            OutputView.Content = groups;
        }
        private void AudienceButton_Click(object sender, RoutedEventArgs e)
        {
            OutputView.Content = audience;
        }
        private void SpecialityButton_Click(object sender, RoutedEventArgs e)
        {
            OutputView.Content = speciality;
        }
        private void SubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            OutputView.Content = subjects;
        }
        private void FacultyButton_Click(object sender, RoutedEventArgs e)
        {
            OutputView.Content = faculty;
        }
        private void TimetableButton_Click(object sender, RoutedEventArgs e)
        {
            OutputView.Content = timetable;
        }

        private void AppClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void MaxNormalWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }
        private void TurnApp_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Header_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }
        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

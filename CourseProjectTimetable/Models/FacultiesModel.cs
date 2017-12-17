using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject.Models
{
    class FacultiesModel
    {
        private TimetableCourseProject context;

        public FacultiesModel()
        {
            this.context = new TimetableCourseProject();
        }

        public string Add(Faculties faculties)
        {
            if (!IsExist(faculties))
            {
                context.Faculties.Add(faculties);

                if (IsContextChanged())
                {
                    return "Объект успешно добавлен!";
                }
                else
                    return "Произошла ошибка добавления в EF!\n";
            }
            else
                return "Данный объект уже существует!";
        }

        public string Change(Faculties changedFaculty, Faculties newFaculty)
        {
            if (IsExist(changedFaculty))
            {
                var faculty = ReturnFaculty(changedFaculty).First();
                faculty.FullName = newFaculty.FullName;
                if (IsContextChanged())
                {
                    return "Изменение произошло успешно!";
                }
                return "Произошла ошибка изменения в EF!\nВозможно вы не изменили данные!";
            }
            return "Изменяемого объекта не существует!";
        }

        public string Remove(Faculties faculties)
        {
            if (IsExist(faculties))
            {
                if (IsRemoved(faculties))
                {
                    if (IsContextChanged())
                    {
                        return "Удаление произошло успешно!";
                    }
                    return "Произошла ошибка удаления в EF!";
                }
                return "Данный объект не был удален!";
            }
            return "Данный объект не существует!";
        }

        public bool IsExist(Faculties faculty)
        {
            return (ReturnFaculty(faculty).Count() != 0);
           
        }

        public IQueryable<Faculties> ReturnFaculty(Faculties faculty)
        {
            return context.Faculties.Where(f => f.ShortName.Equals(faculty.ShortName) &&
                                                f.FullName.Equals(faculty.FullName));
        }

        public bool IsContextChanged()
        {
            return context.SaveChanges() != 0; ;
        }

        public bool IsRemoved(Faculties faculty)
        {
            switch (MessageBox.Show("При удалении факультета все специальности, группы, занятия,\nкафедры этого факультета, преподаватели будут удалены.",
                    null, MessageBoxButton.OKCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.OK:
                    foreach(var pair in context.Timetable.Local)
                        foreach(var teacher in context.Teachers.Where(t => t.Pulpits.ShortFacultyName.Equals(faculty.ShortName)))
                               if(pair.TeacherId.Equals(teacher.Id))
                                context.Timetable.Remove(pair);
                    return context.Faculties.Remove(ReturnFaculty(faculty).First()) != null;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }

        public static Faculties GetFacultyObject(string ShortName, string FullName)
        {
            Faculties faculty = new Faculties()
            {
                ShortName = ShortName,
                FullName = FullName
            };
            return faculty;
        }
    }
}
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
                context.InsertFaculties(faculties.ShortName, faculties.FullName);

                return "Объект успешно добавлен!";
            }
            else
                return "Данный объект уже существует!";
        }

        public string Change(Faculties changedFaculty, Faculties newFaculty)
        {
            if (IsExist(changedFaculty))
            {
                context.UpdateFaculty(changedFaculty.ShortName, newFaculty.FullName);

                return "Изменение произошло успешно!";
            }
            return "Изменяемого объекта не существует!";
        }

        public string Remove(Faculties faculties)
        {
            if (IsExist(faculties))
            {
                if (IsRemoved(faculties))
                {
                    return "Удаление произошло успешно!";
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

                    context.DeleteFaculty(faculty.ShortName);
                    return true;

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
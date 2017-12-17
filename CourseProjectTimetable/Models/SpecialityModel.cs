using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject.Models
{
    class SpecialityModel
    {
        private TimetableCourseProject context;

        public SpecialityModel()
        {
            this.context = new TimetableCourseProject();
        }

        public string Add(Specialities speciality)
        {
            if (!IsExist(speciality))
            {
                context.Specialities.Add(speciality);

                if (IsContextChanged())
                {
                    return "Объект успешно добавлен!";
                }
                else
                    return "Произошла ошибка добавления в EF!";
            }
            else
                return "Данный объект уже существует!";
        }

        public string Change(Specialities changedSpeciality, Specialities newSpeciality)
        {
            if (IsExist(changedSpeciality))
            {
                Specialities spec = ReturnSpeciality(changedSpeciality).First();
                spec.ShortName = newSpeciality.ShortName;
                spec.FullName = newSpeciality.FullName;
                spec.ShortFacultyName = newSpeciality.ShortFacultyName;
                spec.Qualification = newSpeciality.Qualification;
                if (IsContextChanged())
                {
                    return "Объект успешно изменен!";
                }
                else
                    return "Произошла ошибка добавления в EF!\nВозможно вы никак не поменяли!";
            }
            else
                return "Данного объекта не существует!";
        }

        public string Remove(Specialities speciality)
        {
            if (IsExist(speciality))
            {
                if (IsRemoved(speciality))
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

        public bool IsExist(Specialities speciality)
        {
            return (ReturnSpeciality(speciality)).Count() != 0;
        }

        private IQueryable<Specialities> ReturnSpeciality(Specialities speciality)
        {
            return context.Specialities.Where(s => s.Code.Equals(speciality.Code));
        }

        public static Specialities getSpecialityObject(string Code, string ShortName, string FullName, string shortFacultyName, string qualification)
        {
            Specialities spec = new Specialities();
            spec.Code = Code;
            spec.ShortFacultyName = shortFacultyName;
            spec.ShortName = ShortName;
            spec.FullName = FullName;
            spec.Qualification = qualification;
            return spec;
        }

        public bool IsContextChanged()
        {
            return context.SaveChanges() != 0;
        }

        public bool IsRemoved(Specialities speciality)
        {
            switch (MessageBox.Show("При удалении специальности, все ее группы и занятия будут удалены!",
                    null, MessageBoxButton.OKCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.OK:
                    return context.Specialities.Remove(ReturnSpeciality(speciality).First()) != null;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}

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
                context.InsertSpeciality(speciality.Code, speciality.ShortFacultyName, speciality.ShortName, speciality.FullName, speciality.Qualification);
                
                return "Объект успешно добавлен!";
            }
            else
                return "Данный объект уже существует!";
        }

        public string Change(Specialities changedSpeciality, Specialities newSpeciality)
        {
            if (IsExist(changedSpeciality))
            {
                context.UpdateSpeciality(changedSpeciality.Code, newSpeciality.ShortFacultyName, newSpeciality.ShortName, newSpeciality.FullName, newSpeciality.Qualification);

                return "Объект успешно изменен!";
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
                        return "Удаление произошло успешно!";
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
                    context.DeleteSpeciality(speciality.Code);
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}

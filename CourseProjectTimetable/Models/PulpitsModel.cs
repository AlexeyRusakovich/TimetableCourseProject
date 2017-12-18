using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject.Models
{
    class PulpitsModel
    {
        private TimetableCourseProject context;

        public PulpitsModel()
        {
            this.context = new TimetableCourseProject();
        }

        public string Add(Pulpits pulpits)
        {
            if (!IsExist(pulpits))
            {
                context.InsertPulpit(pulpits.ShortName, pulpits.FullName, pulpits.ShortFacultyName);
                
                return "Объект успешно добавлен!";
            }
            else
                return "Данный объект уже существует!";
        }

        public string Change(Pulpits changedPulpit, Pulpits newPulpit)
        {
            if (IsExist(changedPulpit))
            {
                context.UpdatePulpit(changedPulpit.ShortName, newPulpit.FullName, newPulpit.ShortFacultyName);

                return "Изменение произошло успешно!";

            }
            return "Данный объект не существует!";
        }

        public string Remove(Pulpits pulpits)
        {
            if (IsExist(pulpits))
            {
                if (IsRemoved(pulpits))
                {
                        return "Удаление произошло успешно!";
                }
                return "Данный объект не был удален!";
            }
            return "Данный объект не существует!";
        }

        public bool IsExist(Pulpits pulpit)
        {
            return ReturnPulpit(pulpit).Count() != 0;
        }

        public bool IsContextChanged()
        {
            return context.SaveChanges() != 0;
        }

        

        public static Pulpits GetPulpitObject(string shortName, string fullName, string shortFacultyName)
        {
            Pulpits pulpit = new Pulpits()
            {
                ShortName = shortName,
                FullName = fullName,
                ShortFacultyName = shortFacultyName
            };
            return pulpit;
        }

        private IQueryable<Pulpits> ReturnPulpit(Pulpits pulpit)
        {
            return context.Pulpits.Where(p => p.ShortName.Equals(pulpit.ShortName) &&
                                               p.FullName.Equals(pulpit.FullName));
        }

        public bool IsRemoved(Pulpits pulpit)
        {
            switch (MessageBox.Show("При удалении кафедры, все ее преподаватели\nи и их занятия тоже будут удалены.",
                    null, MessageBoxButton.OKCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.OK:
                    context.DeletePulpit(pulpit.ShortName);
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}

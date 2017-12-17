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
                context.Pulpits.Add(pulpits);

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

        public string Change(Pulpits changedPulpit, Pulpits newPulpit)
        {
            if (IsExist(changedPulpit))
            {
                var pulpit = ReturnPulpit(changedPulpit).First();
                pulpit.FullName = newPulpit.FullName;
                pulpit.ShortFacultyName = newPulpit.ShortFacultyName;
                if (IsContextChanged())
                {
                    return "Изменение произошло успешно!";
                }
                return "Произошла ошибка удаления в EF!\nИли вы никак не изменили данные!";
            }
            return "Данный объект не существует!";
        }

        public string Remove(Pulpits pulpits)
        {
            if (IsExist(pulpits))
            {
                if (IsRemoved(pulpits))
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
                    foreach (var pair in context.Timetable)
                        foreach(var teacher in context.Teachers.Where( t=> t.ShortPulpitName.Equals(pulpit.ShortName)))
                        if (pair.TeacherId.Equals(teacher.Id))
                            context.Timetable.Remove(pair);
                    return context.Pulpits.Remove(ReturnPulpit(pulpit).First()) != null;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}

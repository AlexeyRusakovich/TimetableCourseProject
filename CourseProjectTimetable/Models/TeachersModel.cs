using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject.Models
{
    class TeachersModel
    {
        private TimetableCourseProject context;

        public TeachersModel()
        {
            this.context = new TimetableCourseProject();
        }

        public String Add(Teachers teachers)
        {
            if (!IsExist(teachers))
            {
                context.Teachers.Add(teachers);

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

        public String Change(Teachers changedTeacher, Teachers newTeacher)
        {
            if (IsExist(changedTeacher))
            {
                changedTeacher = ReturnTeacher(changedTeacher).First();
                changedTeacher.ShortPulpitName = newTeacher.ShortPulpitName;
                if (IsContextChanged())
                {
                    return "Объект успешно изменен!";
                }
                else
                    return "Произошла ошибка добавления в EF!";
            }
            else
                return "Данный объект уже существует!";
        }

        public String Remove(Teachers teachers)
        {
            if (IsExist(teachers))
            {
                if (IsRemoved(teachers))
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

        public static Teachers getTeacherObject(string Name, string Surname, string Patronymic, string ShortPulpitName)
        {
            Teachers t = new Teachers();
            t.Id = Surname + ' ' + Name.First() + '.' + Patronymic.First() + '.';
            t.Name = Name;
            t.Surname = Surname;
            t.Patronymic = Patronymic;
            t.ShortPulpitName = ShortPulpitName;
            return t;
        }

        public bool IsExist(Teachers teachers)
        {
            return ReturnTeacher(teachers).Count() != 0;
        }

        private IQueryable<Teachers> ReturnTeacher(Teachers teacher)
        {
            return context.Teachers.Where(t =>  t.Name.Equals(teacher.Name) &&
                                                t.Surname.Equals(teacher.Surname) &&
                                                t.Patronymic.Equals(teacher.Patronymic) &&
                                                t.ShortPulpitName.Equals(teacher.ShortPulpitName));
        }

        public bool IsContextChanged()
        {
            return context.SaveChanges() != 0;
        }

        public bool IsRemoved(Teachers teachers)
        {
            switch (MessageBox.Show("При удалении записи преподавателя, все занятия данного преподавателя будут удалены!",
                    null, MessageBoxButton.OKCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.OK:
                    foreach (var timetable in context.Timetable.Where(t => t.TeacherId.Equals(teachers.Id)))
                        context.Timetable.Remove(timetable);
                    return context.Teachers.Remove(ReturnTeacher(teachers).First()) != null;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}

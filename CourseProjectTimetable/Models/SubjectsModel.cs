using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject.Models
{
    class SubjectsModel
    {
        private TimetableCourseProject context;

        public SubjectsModel()
        {
            this.context = new TimetableCourseProject();
        }

        public String Add(Subjects subjects)
        {
            if (!IsExist(subjects))
            {
                context.Subjects.Add(subjects);

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

        public String Remove(Subjects subjects)
        {
            if (IsExist(subjects))
            {
                if (IsRemoved(subjects))
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

        public String Change(Subjects changedSubjects, Subjects newSubject)
        {
            if (IsExist(changedSubjects))
            {
                Subjects sub = ReturnSubject(changedSubjects).First();
                sub.FullName = newSubject.FullName;
                if (IsContextChanged())
                {
                    return "Изменение произошло успешно!";
                }
                return "Произошла ошибка изменения в EF!\nВозможны вы никак не изменили данные!";
            }
            return "Данный объект не существует!";
        }

        public bool IsExist(Subjects subjects)
        {
            return ReturnSubject(subjects).Count() != 0;
        }

        private IQueryable<Subjects> ReturnSubject(Subjects sub)
        {
            return context.Subjects.Where(s => s.FullName.Equals(sub.FullName) &&
                                                s.ShortName.Equals(sub.ShortName));
        }

        public static Subjects getSubjectObject(string shortName, string fullName)
        {
            Subjects sub = new Subjects();
            sub.ShortName = shortName;
            sub.FullName = fullName;
            return sub;
        }

        public bool IsContextChanged()
        {
            return context.SaveChanges() != 0;
        }

        public bool IsRemoved(Subjects subjects)
        {
            switch (MessageBox.Show("При удалении предмета, все занятия данного предмета будут удалены!",
                    null, MessageBoxButton.OKCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.OK:
                    return context.Subjects.Remove(ReturnSubject(subjects).First()) != null;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
        
    }
}

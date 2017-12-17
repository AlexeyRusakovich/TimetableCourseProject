using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CourseProjectTimetable;

namespace CourseProject.Models
{
    class AudienceModel
    {
        private TimetableCourseProject context;

        public AudienceModel()
        {
            this.context = new TimetableCourseProject();
        }
        public string Add(Audience audience)
        {
            if (!IsExist(audience))
            {
                context.Audience.Add(audience);

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
        public static Audience GetAudienceObject(string Number, int Capacity)
        {
            Audience audience = new Audience()
            {
                Number = Number,
                Capacity = Capacity
            };
            return audience;
        }
        public string Remove(Audience audience)
        {
            if (IsExist(audience))
            {
                if (IsRemoved(audience))
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
        public string Change(Audience changedAudience, Audience newAudience)
        {
            if(IsExist(changedAudience))
            {
                changedAudience = ReturnAudience(changedAudience).First();
                changedAudience.Capacity = newAudience.Capacity;
                if (IsContextChanged())
                {
                    context.SaveChanges();

                    return "Изменение произошло успешно!";
                }
                return "Произошла ошибка изменения в EF!\nВозможно вы не изменили данные!";
            }
            return "Изменяемого объекта не существует!";
        }                   
        public bool IsExist(Audience audience)
        {
            return (ReturnAudience(audience).Count() != 0);
        }
        public IQueryable<Audience> ReturnAudience(Audience audience)
        {
            return context.Audience.Where(a => a.Number.Equals(audience.Number) &&
                               a.Capacity == audience.Capacity);
        }
        public bool IsContextChanged()
        {
            return context.SaveChanges() != 0;
        }
        public bool IsRemoved(Audience audience)
        {
            switch(MessageBox.Show("При удалении аудитории все занятия в ней будут удалены.",
                    null, MessageBoxButton.OKCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.OK:
                    return context.Audience.Remove(ReturnAudience(audience).First()) != null;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;           
        }
    }
}

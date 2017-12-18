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
                context.InsertAudience(audience.Number,audience.Capacity);

                return "Объект успешно добавлен!";

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
                    return "Удаление произошло успешно!";
                }
                return "Данный объект не был удален!";
            }
            return "Данный объект не существует!";
        }
        public string Change(Audience changedAudience, Audience newAudience)
        {
            if(IsExist(changedAudience))
            {
                context.UpdateAudience(changedAudience.Number, newAudience.Capacity);
                context.SaveChanges();

                return "Изменение произошло успешно!";
            }
            return "Изменяемого объекта не существует!";
        }                   
        public bool IsExist(Audience audience)
        {
            return (ReturnAudience(audience).Count() != 0);
        }
        public IQueryable<Audience> ReturnAudience(Audience audience)
        {
            return context.Audience.Where(a => a.Number.Equals(audience.Number));
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
                    context.DeleteAudience(audience.Number);
                        return true;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;           
        }
    }
}

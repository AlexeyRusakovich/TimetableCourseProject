using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject.Models
{
    class TimetableModel
    {
        private TimetableCourseProject context;

        public TimetableModel()
        {
            this.context = new TimetableCourseProject();
        }
        public String Add(Timetable timetable)
        {
            if (!IsExist(timetable))
            {
                string result = null;
                if ((result = IsCorrectTimetable(timetable)) == null)
                {
                    context.Timetable.Add(timetable);

                    if (IsContextChanged())
                    {
                        return "Объект успешно добавлен!";
                    }
                    else
                        return "Произошла ошибка добавления в EF!";
                }
                else
                    return result;
            }
            else
                return "Данный объект уже существует!";
        }
        public String Change(Timetable cht,Timetable nt)
        {
            if (IsExist(cht))
            {
                string result;
                Timetable t = new Timetable();
                t = ReturnTimetable(cht).First();
                nt.Id = t.Id;

                if ((result = IsCorrectTimetable(nt)) == null)
                {                    

                    t.PairNumber = nt.PairNumber;
                    t.ShortPairtypeName = nt.ShortPairtypeName;
                    t.DayNumber = nt.DayNumber;
                    t.TeacherId = nt.TeacherId;
                    t.ShortSubjectName = nt.ShortSubjectName;
                    t.GroupId = nt.GroupId;
                    t.Subgroup = nt.Subgroup;
                    t.WeekNumber = nt.WeekNumber;
                    t.AudienceNumber = nt.AudienceNumber;

                    if (IsContextChanged())
                    {
                        return "Объект успешно изменен!";
                    }
                    else
                        return "Произошла ошибка изменения в EF!\nВозмоджно вы никак не изменили данные!";
                }
                    return result;
            }
            else
                return "Изменяемого объекта не существует!";
        }
        public String Remove(Timetable timetable)
        {
            if (IsExist(timetable))
            {
                if (IsRemoved(timetable))
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
        public bool IsExist(Timetable timetable)
        {
            return ReturnTimetable(timetable).Count() != 0;
        }
        private IQueryable<Timetable> ReturnTimetable(Timetable timetable)
        {
            return context.Timetable.Where(a =>
                    a.PairNumber == timetable.PairNumber &&
                    a.AudienceNumber.Equals(timetable.AudienceNumber) &&
                    a.DayNumber.Equals(timetable.DayNumber) &&
                    a.GroupId.Equals(timetable.GroupId) &&
                    a.Subgroup.Equals(timetable.Subgroup) &&
                    a.TeacherId.Equals(timetable.TeacherId) &&
                    a.WeekNumber.Equals(timetable.WeekNumber) &&
                    a.ShortPairtypeName.Equals(timetable.ShortPairtypeName) &&
                    a.ShortSubjectName.Equals(timetable.ShortSubjectName));
        }
        public static Timetable getTimetableObject(string audienceNumber, string group, string subgroup,
            string teacher, string pairType, int pairNumber, string weekNumber, string subject, string dayNumber)
        {
            Timetable t = new Timetable();
            t.AudienceNumber = audienceNumber;
            t.GroupId = group;
            t.Subgroup = subgroup.Equals("Нету") ? null : subgroup;
            t.TeacherId = teacher;
            t.ShortPairtypeName = pairType;
            t.PairNumber = pairNumber;
            t.WeekNumber = weekNumber.Equals("По обеим") ? null : weekNumber;
            t.ShortSubjectName = subject;
            t.DayNumber = dayNumber;
            return t;
        }
        public bool IsContextChanged()
        {
            return context.SaveChanges() != 0;
        }
        public bool IsRemoved(Timetable timetable)
        {
            return context.Timetable.Remove(ReturnTimetable(timetable).First()) != null;
        }
        public string IsCorrectTimetable(Timetable t2)
        {
            if (context.Timetable.Where(t => !(t.Id == t2.Id) &&
                                             t.GroupId.Equals(t2.GroupId)                   &&

                                             t.DayNumber.Equals(t2.DayNumber)               &&
                                             
                                             t.WeekNumber.Equals(t2.WeekNumber)             &&

                                             t.PairNumber == t2.PairNumber                  &&

                                             !(t.AudienceNumber.Equals(t2.AudienceNumber)) &&

                                             ((t.Subgroup == null && t2.Subgroup != null)   ||
                                             (t.Subgroup == null && t2.Subgroup == null)    ||
                                              (t.Subgroup != null && t2.Subgroup == null)   ||
                                              t.Subgroup.Equals(t2.Subgroup))

                ).Count() != 0)
                return "Ошибка!\nУ данной (группы, подгруппы или подгруппы и группы) не может\n быть в одно и то же время пара в разных аудиториях.";

            if (context.Timetable.Where(t => !(t.Id == t2.Id) &&
                                            t.GroupId.Equals(t2.GroupId)  &&

                                             t.DayNumber.Equals(t2.DayNumber) &&

                                             t.WeekNumber.Equals(t2.WeekNumber) &&

                                             t.PairNumber == t2.PairNumber &&

                                             t2.AudienceNumber.Equals(t2.AudienceNumber) &&

                                             (t.Subgroup != null && t2.Subgroup != null &&
                                             t.Subgroup.Equals(t2.Subgroup))

                ).Count() != 0)
                return "Ошибка!\nВозможно вы хотите поместить другую подгруппу добавляемой группы в ту же аудиторию\n" +
                        "Если одна группа занимается в одной и той же аудитории, не должен быть указан номер подгруппы изначально!.";

            if (context.Timetable.Where(t => !(t.Id == t2.Id) &&
                                            ((t.GroupId.Equals(t2.GroupId))                  &&

                                            (t.DayNumber.Equals(t2.DayNumber))               &&

                                            (t.PairNumber == t2.PairNumber)                  &&

                                           (t.WeekNumber == null && t2.WeekNumber != null   ||
                                            t.WeekNumber != null && t2.WeekNumber == null   ||
                                            t.WeekNumber.Equals(t2.WeekNumber))             &&

                                            (t.Subgroup == null && t2.Subgroup != null      ||
                                            t.Subgroup != null && t2.Subgroup == null       ||
                                            t.Subgroup == null && t2.Subgroup == null       ||
                                            t.Subgroup == t2.Subgroup))
            ).Count() != 0)
                return "Ошибка!\nУ данной (группы, подгруппы или подгруппы и группы)\n" +
                    "не может быть пара а уже занятое время\n(Возможно ошибка в номере недели)\n";


            //У преподавателя уже есть пара либо по обеим неделям, либо по 1-ой, либо по 2-ой(не важно в какой аудитории и какой группе)
            if (context.Timetable.Where(t => !(t.Id == t2.Id) &&
                                            (t.TeacherId.Equals(t2.TeacherId)               &&

                                            t.DayNumber.Equals(t2.DayNumber)                &&

                                            t.PairNumber == t2.PairNumber                   &&

                                            !t.ShortSubjectName.Equals("Физ-ра")            &&
                                            !t2.ShortSubjectName.Equals("Физ-ра")           &&

                                            !t.ShortPairtypeName.Equals("лк")               &&
                                            !t2.ShortPairtypeName.Equals("лк")              && 
 
                                            ((t.WeekNumber == null && t2.WeekNumber == null) ||
                                            (t.WeekNumber == null && t2.WeekNumber != null) ||
                                            (t.WeekNumber != null && t2.WeekNumber == null) ||
                                            (t.WeekNumber != null && t2.WeekNumber != null &&
                                            t.WeekNumber.Equals(t2.WeekNumber))))
            ).Count() != 0)
                return "Ошибка!\nВозможно вы хотите поместить пару у преподавателя в уже занятое время!";

            if (context.Timetable.Where(t => !(t.Id == t2.Id) &&
                                            t.TeacherId.Equals(t2.TeacherId) &&

                                            t.DayNumber.Equals(t2.DayNumber) &&

                                            t.PairNumber == t2.PairNumber &&

                                            t.AudienceNumber.Equals(t2.AudienceNumber) &&

                                            t.GroupId.Equals(t2.GroupId)        &&

                                            t.ShortSubjectName.Equals(t2.ShortSubjectName) &&

                                            (t.WeekNumber == null && t2.WeekNumber == null ||
                                            t.WeekNumber != null && t2.WeekNumber != null &&
                                            t.WeekNumber.Equals(t2.WeekNumber))
            ).Count() != 0)
                return "Ошибка!\nВозможно вы хотите поместить пару у преподавателя по разным\n" +
                    "неделям в одну и ту же аудиторию. Если преподаватель ведет\n" +
                    "предмет в одной и той же аудитории у одной и той же группы по обеим неделям" +
                    "то надо изначально указывать пустую строку недели!";

            if (context.Timetable.Where(t =>    (t.Id != t2.Id) &&
                                                !(t.TeacherId.Equals(t2.TeacherId)) &&

                                                 t.DayNumber.Equals(t2.DayNumber) &&

                                                 t.PairNumber == t2.PairNumber &&

                                                !t.ShortSubjectName.Equals("Физ-ра") &&
                                                !t2.ShortSubjectName.Equals("Физ-ра") &&

                                                 t.AudienceNumber.Equals(t2.AudienceNumber) &&

                                                 ((t.WeekNumber == null && t2.WeekNumber == null) ||
                                                 (t.WeekNumber == null && t2.WeekNumber != null) ||
                                                 (t.WeekNumber != null && t2.WeekNumber == null) ||
                                                 (t.WeekNumber != null && t2.WeekNumber != null &&
                                                 t.WeekNumber.Equals(t2.WeekNumber)))

            ).Count() != 0)
                return "Ошибка!\nВозможно вы хотите поместить пару одного преподавателя\n" +
                    "в аудиторию занятую другим преподавателем";

            if (context.Timetable.Where(t => (t.Id != t2.Id) &&
                                            t.DayNumber.Equals(t2.DayNumber) &&

                                             t.PairNumber == t2.PairNumber &&

                                             t.AudienceNumber.Equals(t2.AudienceNumber) &&

                                             ((t.WeekNumber == null && t2.WeekNumber == null) ||
                                            (t.WeekNumber == null && t2.WeekNumber != null) ||
                                            (t.WeekNumber != null && t2.WeekNumber == null) ||
                                            (t.WeekNumber != null && t2.WeekNumber != null &&
                                            t.WeekNumber.Equals(t2.WeekNumber))) &&

                                             t.ShortPairtypeName.Equals("лк") &&
                                             t2.ShortPairtypeName.Equals("лк") &&
                                             
                                             !t.ShortSubjectName.Equals(t2.ShortSubjectName)
            ).Count() != 0)
                switch(MessageBox.Show("Вы хотите поместить в одну аудиторию\nгруппу с другим названием предмета." +
                "\nДействительно это сделать?", "?", MessageBoxButton.OKCancel, MessageBoxImage.Information))
                {
                    case MessageBoxResult.OK:
                        return null;
                    case MessageBoxResult.Cancel:
                        return "Вы отменили действие.";
                    default: return "Вы отменили действие.";
                }

            if (context.Timetable.Where(t => (t.Id != t2.Id) &&
                                             t.DayNumber.Equals(t2.DayNumber) &&

                                             t.PairNumber == t2.PairNumber &&

                                             t.AudienceNumber.Equals(t2.AudienceNumber) &&

                                            !t.ShortSubjectName.Equals("Физ-ра") &&
                                            !t2.ShortSubjectName.Equals("Физ-ра") &&

                                             ((t.WeekNumber == null && t2.WeekNumber == null) ||
                                            (t.WeekNumber == null && t2.WeekNumber != null) ||
                                            (t.WeekNumber != null && t2.WeekNumber == null) ||
                                            (t.WeekNumber != null && t2.WeekNumber != null &&
                                            t.WeekNumber.Equals(t2.WeekNumber))) &&

                                             (!t.ShortPairtypeName.Equals("лк") ||
                                             !t2.ShortPairtypeName.Equals("лк"))
            ).Count() != 0)
                return "Ошибка!\nПреподаватель может вести одновременно у\n нескольких групп только лекционные пары и физкультуру!";
            return null;
        }
    }
}

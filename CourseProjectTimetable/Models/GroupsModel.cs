using CourseProjectTimetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject.Models
{
    public class GroupsModel
    {
        private TimetableCourseProject context;

        public GroupsModel()
        {
            this.context = new TimetableCourseProject();
        }

        public String Add(Groups group)
        {
            if (!IsExist(group))
            {
                context.InsertGroups(group.Id, group.SpecialityCode, group.Course, group.Number, group.Count);

                return "Объект успешно добавлен!";
            }
            else
                return "Данный объект уже существует!";
        }

        public String Remove(Groups group)
        {
            if (IsExist(group))
            {
                if (IsRemoved(group))
                {
                        return "Удаление произошло успешно!";
                }
                return "Данный объект не был удален!";
            }
            return "Данный объект не существует!";
        }
        
        public string Change(Groups changedGroup, Groups newGroup)
        {
            if (IsExist(changedGroup))
            {
                var Group = ReturnGroup(changedGroup).First();

                Group.Count = newGroup.Count;

                if (IsContextChanged())
                {
                    return "Данные успешно изменились!";
                }
                else
                    return "Произошла ошибка изменения в EF!\nВозможно вы не изменили данные!";
            }
            else
                return "Данного объекта не существует!";
        }

        public bool IsExist(Groups groups)
        {
            return ReturnGroup(groups).Count() != 0;
        }

        private IQueryable<Groups> ReturnGroup(Groups group)
        {
            return context.Groups.Where(g => g.Id.Equals(group.Id) &&
                                             g.Number == group.Number &&
                                             g.SpecialityCode.Equals(group.SpecialityCode));
        }

        public static Groups getGroupObject(string Id, string Code,int Course, int Number, int Count)
        {
            Groups group = new Groups();
            group.Id = Id;
            group.SpecialityCode = Code;
            group.Count = Count;
            group.Course = Course;
            group.Number = Number;
            return group;
        }

        public bool IsContextChanged()
        {
            return context.SaveChanges() != 0;
        }

        public bool IsRemoved(Groups groups)
        {
            switch (MessageBox.Show("При удалении группы, все ее занятия тоже будут удалены.",
                    null, MessageBoxButton.OKCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.OK:
                    context.DeleteGroup(groups.Id);
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}

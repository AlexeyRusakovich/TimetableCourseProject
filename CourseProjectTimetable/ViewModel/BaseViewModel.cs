using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CourseProjectTimetable.ViewModel
{
    public static class MyExMethods
    {

        // Сводка:
        //     Преобразовывает строковое значение данного экземпляра в эквивалентное ему числовое
        //     представление.
        //
        // Возврат:
        //     Числовое представление значения данного экземпляра если данную строку можно привести
        //     к числовому значению, в другом случае возвращает 0.
        public static int ToInt(this string str)
        {
            try
            {
                int ri = 0;

                int.TryParse(str, out ri);

                return ri;

            }
            catch
            {
                return 0;
            }

        }
    }


    public class BaseViewModel
    {
        protected bool IsRange(string someCount, int from, int to)
        {
            if (int.Parse(someCount) < from || int.Parse(someCount) > to)
                return false;
            else
                return true;
        }
        
        protected string ResultForIntValues(string someString, int from, int to)
        {
            if (string.IsNullOrWhiteSpace(someString))
                return EmptyString();
            else if (int.TryParse(someString, out int someInt))
                if (IsRange(someString, from, to))
                    return null;
                else
                    return AllowableValue(from, to);
            else
                return OnlyNumbers();
        }

        protected string ResultForStringValues(string someString, int length)
        {
            if (string.IsNullOrWhiteSpace(someString))
                return EmptyString();
            else if (someString.Length > length)
                return AllowableLength(length);
            else if (!IsRussian(someString))
                return OnlyRussian();
            else
                return null;
        }

        protected bool IsRussian(string someString)
        {
            return (Regex.IsMatch(someString, "([а-яА-ЯЁё])+") && !Regex.IsMatch(someString, "[0-9]+"));
        }

        protected string AllowableValue(int from, int to)
        {
            return "Допустимое значения : от " + from + " до " + to + ".";
        }

        protected string AllowableLength(int length)
        {
            return "Максимальная длина : " + length + " символов.";
        }

        protected string EmptyString()
        {
            return "Поле должно быть заполнено.";
        }

        protected string OnlyRussian()
        {
            return "Только симолы русского алфавита.";
        }

        protected string OnlyNumbers()
        {
            return "Допустимы только цифры.";
        }
    }
}

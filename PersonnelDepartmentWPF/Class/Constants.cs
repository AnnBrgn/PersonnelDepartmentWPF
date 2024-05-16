using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelDepartmentWPF.Class
{
    public static class Constants
    {
        public static string[] GENDERS
        {
            get; set;
        } = new string[]
        {
            "Мужской", 
            "Женский"
        };

        public static string[] DATE_TYPE
        {
            get; set;
        } = new string[]
{
            "Рабочий",
            "Отпускной",
            "Больничный",
            "Прогул"
};

        public static Action OnListWorkersChanged;
    }
}

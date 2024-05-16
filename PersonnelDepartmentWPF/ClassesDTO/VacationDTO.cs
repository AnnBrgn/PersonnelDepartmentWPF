using System;

namespace PersonnelDepartmentAPI.ClassesDTO
{
    public class VacationDTO
    {
        public int IdWorker { get; set; }

        public int? VacationDay { get; set; }

        public DateTime? Date { get; set; }
    }
}

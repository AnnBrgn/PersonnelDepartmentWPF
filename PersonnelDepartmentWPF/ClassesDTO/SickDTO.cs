using System;

namespace PersonnelDepartmentAPI.ClassesDTO
{
    public class SickDTO
    {
        public int IdWorker { get; set; }

        public int? SickDay { get; set; }

        public string? Description { get; set; }

        public DateTime? Date { get; set; }
    }
}

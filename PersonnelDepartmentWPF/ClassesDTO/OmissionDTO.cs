using System;

namespace PersonnelDepartmentAPI.ClassesDTO
{
    public class OmissionDTO
    {
        public int IdWorker { get; set; }

        public int? OmissionDay { get; set; }

        public string? Description { get; set; }

        public DateTime? Date { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace NTR.Models
{
    public class ScheduleModel
    {
        public int Id { get; set; }
        public required string DoctorId { get; set; }
        public string? PatientId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string? Description { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public ScheduleModel() {}
    }
    public class SchedulePatientViewModel : ScheduleModel
    {
        public string? DoctorName { get; set; }
        public string? Specialization { get; set; }
    }

    public class ScheduleDoctorViewModel : ScheduleModel
    {
        public string? PatientName { get; set; }
    }
}
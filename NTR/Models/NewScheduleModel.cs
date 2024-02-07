using System.ComponentModel.DataAnnotations;

namespace NTR.Models
{
    public class NewScheduleModel
    {
        public required string DoctorId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public TimeSpan WorkDayStart { get; set; }
        

        public TimeSpan WorkDayEnd { get; set; }
        
        public NewScheduleModel() {}
    }
}
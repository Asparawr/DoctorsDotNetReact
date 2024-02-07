using System.ComponentModel.DataAnnotations;

namespace NTR.Models
{
    public class ScheduleDayModel
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public ScheduleDayModel() {}
    }
}
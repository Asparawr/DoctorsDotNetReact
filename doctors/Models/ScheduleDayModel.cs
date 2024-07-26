using System.ComponentModel.DataAnnotations;

namespace doctors.Models
{
    public class ScheduleDateModel
    {
        public required string DoctorId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ScheduleDateModel() {}
    }
}
using System.ComponentModel.DataAnnotations;

namespace doctors.Models
{
    public class ScheduleDescriptionModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public ScheduleDescriptionModel() {}
    }
}
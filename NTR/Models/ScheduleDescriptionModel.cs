using System.ComponentModel.DataAnnotations;

namespace NTR.Models
{
    public class ScheduleDescriptionModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public ScheduleDescriptionModel() {}
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NTR.Models
{
    public class UserModel : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public bool IsAccepted { get; set; }
        public string? Specialization { get; set; }
    }
}
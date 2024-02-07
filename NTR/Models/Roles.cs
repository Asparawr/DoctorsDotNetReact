using Microsoft.AspNetCore.Identity;

namespace NTR.Models
{

    public static class RolesExtensions
    {
        public const string PATIENT = "Patient";
        public const string DOCTOR = "Doctor";
        public const string DIRECTOR = "Director";
        public const string DIRECTOR_DOCOTOR = "Director, Doctor";
    }
}


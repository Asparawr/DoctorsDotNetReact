namespace NTR.Models
{

    public class SpecializationModel
    {
        public string Specialization { get; set; } = default!;
    }

    public enum Specializations
    {
        Cardiologist,
        Dentist,
        Dermatologist,
        Gastroenterologist,
        Gynecologist,
        Neurologist,
        Ophthalmologist,
        Orthopedist,
        Otolaryngologist,
        Pediatrician,
        Psychiatrist,
        Urologist
    }
    public static class SpecializationsExtensions
    {
        public static string GetSpecialization(this Specializations speciality)
        {
            return speciality switch
            {
                Specializations.Cardiologist => "Cardiologist",
                Specializations.Dentist => "Dentist",
                Specializations.Dermatologist => "Dermatologist",
                Specializations.Gastroenterologist => "Gastroenterologist",
                Specializations.Gynecologist => "Gynecologist",
                Specializations.Neurologist => "Neurologist",
                Specializations.Ophthalmologist => "Ophthalmologist",
                Specializations.Orthopedist => "Orthopedist",
                Specializations.Otolaryngologist => "Otolaryngologist",
                Specializations.Pediatrician => "Pediatrician",
                Specializations.Psychiatrist => "Psychiatrist",
                Specializations.Urologist => "Urologist",
                _ => "Unknown"
            };
        }

        public static Specializations GetSpecialization(this string speciality)
        {
            return speciality switch
            {
                "Cardiologist" => Specializations.Cardiologist,
                "Dentist" => Specializations.Dentist,
                "Dermatologist" => Specializations.Dermatologist,
                "Gastroenterologist" => Specializations.Gastroenterologist,
                "Gynecologist" => Specializations.Gynecologist,
                "Neurologist" => Specializations.Neurologist,
                "Ophthalmologist" => Specializations.Ophthalmologist,
                "Orthopedist" => Specializations.Orthopedist,
                "Otolaryngologist" => Specializations.Otolaryngologist,
                "Pediatrician" => Specializations.Pediatrician,
                "Psychiatrist" => Specializations.Psychiatrist,
                "Urologist" => Specializations.Urologist,
                _ => Specializations.Cardiologist
            };
        }

        public static List<string> GetSpecializations()
        {
            return new List<string>
            {
                "Cardiologist",
                "Dentist",
                "Dermatologist",
                "Gastroenterologist",
                "Gynecologist",
                "Neurologist",
                "Ophthalmologist",
                "Orthopedist",
                "Otolaryngologist",
                "Pediatrician",
                "Psychiatrist",
                "Urologist"
            };
        }
    }
}


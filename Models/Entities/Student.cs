using System.ComponentModel.DataAnnotations;

namespace StudentManager.Models.Entities
{
    public class Student
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Nom")]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Phone]
        [Display(Name = "Téléphone")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Abonné")]
        public bool Subscribed { get; set; }
    }
}
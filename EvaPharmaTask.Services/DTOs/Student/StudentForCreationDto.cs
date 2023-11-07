using System.ComponentModel.DataAnnotations;

namespace EvaPharmaTask.Services.DTOs.Student
{
    public class StudentForCreationDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EvaPharmaTask.Services.DTOs.Course
{
    public class CourseForCreationDto
    {
        [Required]
        public string CourseName { get; set; }
    }
}

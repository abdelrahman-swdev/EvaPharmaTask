using System.Collections.Generic;

namespace EvaPharmaTask.Services.DTOs.Student
{
    public class StudentReportToReturnDto
    {
        public StudentReportToReturnDto() 
        {
            Courses = new List<EnrollmentToReturnDto>();
        }

        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<EnrollmentToReturnDto> Courses { get; set; }
    }
}

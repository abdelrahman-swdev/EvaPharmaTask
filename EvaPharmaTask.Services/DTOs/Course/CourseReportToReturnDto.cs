using System.Collections.Generic;

namespace EvaPharmaTask.Services.DTOs.Course
{
    public class CourseReportToReturnDto
    {
        public CourseReportToReturnDto()
        {
            Students = new List<StudentEnrollmentToReturnDto>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public IEnumerable<StudentEnrollmentToReturnDto> Students { get; set; }
    }
}

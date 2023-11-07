namespace EvaPharmaTask.Services.DTOs.Enrollments
{
    public class AssignGradePerStudentPerCourseRequestDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int GradeId { get; set; }
    }
}

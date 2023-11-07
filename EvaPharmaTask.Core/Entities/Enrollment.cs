using EvaPharmaTask.Core.Enums;
using System;

namespace EvaPharmaTask.Core.Entities
{
    public class Enrollment : BaseEntity
    {
        public DateTime EnrollDate { get; set; }
        public CourseCompletionStatus CourseCompletionStatus { get; set; } = CourseCompletionStatus.Completed;
        public int? GradeId { get; set; }
        public Grade Grade { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}

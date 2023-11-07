using System.Collections.Generic;

namespace EvaPharmaTask.Core.Entities
{
    public class Course : BaseEntity
    {
        public string CourseName { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}

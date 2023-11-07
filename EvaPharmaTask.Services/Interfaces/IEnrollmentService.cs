using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.DTOs.Enrollments;
using EvaPharmaTask.Services.DTOs.Grade;
using EvaPharmaTask.Services.DTOs.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaPharmaTask.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<ApiResponse<bool>> AssignGradePerStudentPerCourseAsync(AssignGradePerStudentPerCourseRequestDto requestDto);
        Task<ApiResponse<int>> AssignStudentToCourseAsync(EnrollmentForCreationDto enrollmentDto);
        Task<ApiResponse<List<GradeToReturnDto>>> GetAllGradesAsync();
        Task<ApiResponse<List<StudentToReturnDto>>> GetGradedStudentsPerCourseAsync(int courseId, int gradeId);
    }
}

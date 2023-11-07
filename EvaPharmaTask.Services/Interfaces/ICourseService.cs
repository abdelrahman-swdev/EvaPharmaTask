using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.DTOs.Course;
using System.Threading.Tasks;

namespace EvaPharmaTask.Services.Interfaces
{
    public interface ICourseService
    {
        Task<ApiResponse<CourseReportToReturnDto>> GetCourseReportAsync(int courseId);
        Task<ApiResponse<int>> SaveCourseAsync(CourseForCreationDto courseDto);
    }
}

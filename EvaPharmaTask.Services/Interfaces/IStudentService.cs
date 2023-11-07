using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.DTOs.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaPharmaTask.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ApiResponse<List<StudentReportToReturnDto>>> GetStudentsReportAsync();
        Task<ApiResponse<int>> SaveStudentAsync(StudentForCreationDto studentDto);
    }
}

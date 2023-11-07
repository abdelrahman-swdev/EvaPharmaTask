using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.DTOs.Student;
using EvaPharmaTask.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaPharmaTask.API.Controllers
{
    public class StudentsController : BaseApiController
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse<int>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SaveStudent([FromBody] StudentForCreationDto studentDto)
        {
            var result = await _studentService.SaveStudentAsync(studentDto);
            return !result.Succeeded
                ? StatusCode(result.StatusCode, result)
                : Ok(result);
        }

        [HttpGet("report")]
        [ProducesResponseType(typeof(ApiResponse<List<StudentReportToReturnDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<StudentReportToReturnDto>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse<List<StudentReportToReturnDto>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetStudentsReport()
        {
            var result = await _studentService.GetStudentsReportAsync();
            return !result.Succeeded
                ? StatusCode(result.StatusCode, result)
                : Ok(result);
        }
    }
}

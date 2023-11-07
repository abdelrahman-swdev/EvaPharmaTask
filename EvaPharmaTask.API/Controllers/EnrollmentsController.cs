using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.DTOs.Enrollments;
using EvaPharmaTask.Services.DTOs.Grade;
using EvaPharmaTask.Services.DTOs.Student;
using EvaPharmaTask.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaPharmaTask.API.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost("assign-student-to-course")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse<int>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AssignStudentToCourse([FromBody]EnrollmentForCreationDto enrollmentDto)
        {
            var result = await _enrollmentService.AssignStudentToCourseAsync(enrollmentDto);
            return !result.Succeeded
                ? StatusCode(result.StatusCode, result)
                : Ok(result);
        }

        [HttpPost("assign-grade-to-student")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse<bool>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AssignGradePerStudentPerCourse([FromBody] AssignGradePerStudentPerCourseRequestDto requestDto)
        {
            var result = await _enrollmentService.AssignGradePerStudentPerCourseAsync(requestDto);
            return !result.Succeeded
                ? StatusCode(result.StatusCode, result)
                : Ok(result);
        }

        [HttpGet("grade-report")]
        [ProducesResponseType(typeof(ApiResponse<List<StudentToReturnDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<StudentToReturnDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<List<StudentToReturnDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetGradedStudentsPerCourse([FromQuery] int courseId, [FromQuery] int gradeId)
        {
            var result = await _enrollmentService.GetGradedStudentsPerCourseAsync(courseId, gradeId);
            return !result.Succeeded
                ? StatusCode(result.StatusCode, result)
                : Ok(result);
        }

        [HttpGet("all-grades")]
        [ProducesResponseType(typeof(ApiResponse<List<GradeToReturnDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<GradeToReturnDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllGrades()
        {
            var result = await _enrollmentService.GetAllGradesAsync();
            return !result.Succeeded
                ? StatusCode(result.StatusCode, result)
                : Ok(result);
        }
    }
}

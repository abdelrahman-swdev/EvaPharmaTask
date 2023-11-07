using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.DTOs.Course;
using EvaPharmaTask.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EvaPharmaTask.API.Controllers
{
    public class CoursesController : BaseApiController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse<int>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SaveCourse([FromBody]CourseForCreationDto courseDto)
        {
            var result = await _courseService.SaveCourseAsync(courseDto);
            return !result.Succeeded
                ? StatusCode(result.StatusCode, result)
                : Ok(result);
        }

        [HttpGet("report/{courseId}")]
        [ProducesResponseType(typeof(ApiResponse<CourseReportToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CourseReportToReturnDto>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse<CourseReportToReturnDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetCourseReport([FromRoute] int courseId)
        {
            var result = await _courseService.GetCourseReportAsync(courseId);
            return !result.Succeeded
                ? StatusCode(result.StatusCode, result)
                : Ok(result);
        }
    }
}

using EvaPharmaTask.Core.Entities;
using EvaPharmaTask.Infrastructure.Data.Repositories;
using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.DTOs.Course;
using EvaPharmaTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EvaPharmaTask.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<CourseReportToReturnDto>> GetCourseReportAsync(int courseId)
        {
            try
            {
                var courseReport = await GetCourseReportResult(courseId);
                return ApiResponse<CourseReportToReturnDto>.Success(courseReport);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<CourseReportToReturnDto>.Failure(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<CourseReportToReturnDto> GetCourseReportResult(int courseId)
        {
            return await _unitOfWork.Repository<Course>()
                                .FindBy(s => s.Id == courseId, true)
                                .Select(course => new CourseReportToReturnDto
                                {
                                    CourseId = course.Id,
                                    CourseName = course.CourseName,
                                    Students = course.Enrollments.Select(a => new StudentEnrollmentToReturnDto
                                    {
                                        FirstName = a.Student.FirstName,
                                        LastName = a.Student.LastName,
                                        Grade = a.Grade.GradeLevel,
                                        StudentId = a.StudentId
                                    }),
                                })
                                .FirstOrDefaultAsync();
        }

        public async Task<ApiResponse<int>> SaveCourseAsync(CourseForCreationDto courseDto)
        {
            try
            {
                var course = new Course
                {
                    CourseName = courseDto.CourseName,
                };
                _unitOfWork.Repository<Course>().Add(course);
                var result = await _unitOfWork.Complete();
                if (result <= 0) 
                    return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError);

                return ApiResponse<int>.Success(course.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError);
            }
        }
    }
}

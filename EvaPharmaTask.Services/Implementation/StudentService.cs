using EvaPharmaTask.Core.Entities;
using EvaPharmaTask.Infrastructure.Data.Repositories;
using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.DTOs.Student;
using EvaPharmaTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EvaPharmaTask.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<int>> SaveStudentAsync(StudentForCreationDto studentDto)
        {
            try
            {
                var student = new Student
                {
                    FirstName = studentDto.FirstName,
                    LastName = studentDto.LastName,
                };
                _unitOfWork.Repository<Student>().Add(student);
                var result = await _unitOfWork.Complete();
                if (result <= 0) 
                    return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError);

                return ApiResponse<int>.Success(student.Id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResponse<List<StudentReportToReturnDto>>> GetStudentsReportAsync()
        {
            try
            {
                var studentsReports = await GetGetStudentsReportsResult();
                return ApiResponse<List<StudentReportToReturnDto>>.Success(studentsReports);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<List<StudentReportToReturnDto>>.Failure(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<List<StudentReportToReturnDto>> GetGetStudentsReportsResult()
        {
            return await _unitOfWork.Repository<Student>()
                                .FindBy(s => true, true)
                                .Select(stud => new StudentReportToReturnDto
                                {
                                    FirstName = stud.FirstName,
                                    LastName = stud.LastName,
                                    StudentId = stud.Id,
                                    Courses = stud.Enrollments.Select(a => new EnrollmentToReturnDto
                                    {
                                        CourseId = a.CourseId,
                                        Grade = a.Grade.GradeLevel,
                                        CourseName = a.Course.CourseName
                                    }),
                                })
                                .ToListAsync();
        }
    }
}

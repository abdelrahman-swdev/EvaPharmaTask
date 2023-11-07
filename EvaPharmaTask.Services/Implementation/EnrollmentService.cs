using EvaPharmaTask.Core.Entities;
using EvaPharmaTask.Infrastructure.Data.Repositories;
using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.DTOs.Enrollments;
using EvaPharmaTask.Services.DTOs.Grade;
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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EnrollmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<int>> AssignStudentToCourseAsync(EnrollmentForCreationDto enrollmentDto)
        {
            try
            {
                var enrollmentValidationResult = await ValidateEnrollmentBeforeSaving(enrollmentDto);
                if(!enrollmentValidationResult.Succeeded)
                    return enrollmentValidationResult;

                return await SaveEnrollmentForStudentCourseAndReturnResult(enrollmentDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<ApiResponse<int>> ValidateEnrollmentBeforeSaving(EnrollmentForCreationDto enrollmentDto)
        {
            var isStudentExists = await IsStudentExistsAsync(enrollmentDto.StudentId);
            if (!isStudentExists)
                return ApiResponse<int>.Failure(HttpStatusCode.NotFound, "Student Not Found");

            var isCourseExists = await IsCourseExistsAsync(enrollmentDto.CourseId);
            if (!isCourseExists)
                return ApiResponse<int>.Failure(HttpStatusCode.NotFound, "Course Not Found");

            var isAlreadyEnrolled = await _unitOfWork.Repository<Enrollment>()
                .FindBy(a => a.StudentId == enrollmentDto.StudentId && a.CourseId == enrollmentDto.CourseId)
                .AnyAsync();
            if (isAlreadyEnrolled)
                return ApiResponse<int>.Failure(HttpStatusCode.BadRequest, "Student Already Enrolled In This Course");

            return ApiResponse<int>.Success(0);
        }

        private async Task<ApiResponse<int>> SaveEnrollmentForStudentCourseAndReturnResult(EnrollmentForCreationDto enrollmentDto)
        {
            var enrollmentToSave = new Enrollment
            {
                CourseId = enrollmentDto.CourseId,
                StudentId = enrollmentDto.StudentId,
                EnrollDate = DateTime.UtcNow,
            };
            _unitOfWork.Repository<Enrollment>().Add(enrollmentToSave);
            var result = await _unitOfWork.Complete();
            if (result <= 0)
                return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError);

            return ApiResponse<int>.Success(enrollmentToSave.Id);
        }

        public async Task<ApiResponse<bool>> AssignGradePerStudentPerCourseAsync(AssignGradePerStudentPerCourseRequestDto requestDto)
        {
            try
            {
                var assignGradeValidationResult = await ValidateAssignGradePerStudentPerCourse(requestDto);
                if (!assignGradeValidationResult.Succeeded)
                    return assignGradeValidationResult;

                return await AssignGradePerStudentPerCourseAndReturnResult(requestDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<bool>.Failure(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<ApiResponse<bool>> AssignGradePerStudentPerCourseAndReturnResult(AssignGradePerStudentPerCourseRequestDto requestDto)
        {
            var enrollment = await _unitOfWork.Repository<Enrollment>()
                .FindBy(a => a.StudentId == requestDto.StudentId && a.CourseId == requestDto.CourseId, false)
                .FirstOrDefaultAsync();

            enrollment.GradeId = requestDto.GradeId;
            _unitOfWork.Repository<Enrollment>().Update(enrollment);
            var result = await _unitOfWork.Complete();
            if (result <= 0)
                return ApiResponse<bool>.Failure(HttpStatusCode.InternalServerError);

            return ApiResponse<bool>.Success(true);
        }

        private async Task<ApiResponse<bool>> ValidateAssignGradePerStudentPerCourse(AssignGradePerStudentPerCourseRequestDto requestDto)
        {
            var isStudentExists = await IsStudentExistsAsync(requestDto.StudentId);
            if (!isStudentExists)
                return ApiResponse<bool>.Failure(HttpStatusCode.NotFound, "Student Not Found");

            var isCourseExists = await IsCourseExistsAsync(requestDto.CourseId);
            if (!isCourseExists)
                return ApiResponse<bool>.Failure(HttpStatusCode.NotFound, "Course Not Found");

            var isGradeExists = await IsGradeExistsAsync(requestDto.GradeId);
            if(!isGradeExists)
                return ApiResponse<bool>.Failure(HttpStatusCode.NotFound, "Grade Not Found");

            var isAlreadyEnrolled = await _unitOfWork.Repository<Enrollment>()
                .FindBy(a => a.StudentId == requestDto.StudentId && a.CourseId == requestDto.CourseId)
                .AnyAsync();
            if (!isAlreadyEnrolled)
                return ApiResponse<bool>.Failure(HttpStatusCode.Conflict, "Student Didn't Enroll In This Course");

            return ApiResponse<bool>.Success(false);
        }

        private async Task<bool> IsGradeExistsAsync(int gradeId)
        {
            return await _unitOfWork.Repository<Grade>()
                .FindBy(a => a.Id == gradeId)
                .AnyAsync();
        }
        
        private async Task<bool> IsCourseExistsAsync(int courseId)
        {
            return await _unitOfWork.Repository<Course>()
                .FindBy(a => a.Id == courseId)
                .AnyAsync();
        }
        
        private async Task<bool> IsStudentExistsAsync(int studentId)
        {
            return await _unitOfWork.Repository<Student>()
                .FindBy(a => a.Id == studentId)
                .AnyAsync();
        }

        public async Task<ApiResponse<List<StudentToReturnDto>>> GetGradedStudentsPerCourseAsync(int courseId, int gradeId)
        {
            try
            {
                var isGradeExists = await IsGradeExistsAsync(gradeId);
                if (!isGradeExists)
                    return ApiResponse<List<StudentToReturnDto>>.Failure(HttpStatusCode.NotFound, "Grade Not Found");

                var isCourseExists = await IsCourseExistsAsync(courseId);
                if (!isCourseExists)
                    return ApiResponse<List<StudentToReturnDto>>.Failure(HttpStatusCode.NotFound, "Course Not Found");
                
                return await ReturnStudentsGradedPerCourse(courseId, gradeId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<List<StudentToReturnDto>>.Failure(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<ApiResponse<List<StudentToReturnDto>>> ReturnStudentsGradedPerCourse(int courseId, int gradeId)
        {
            var studentGradedPerCourse = await _unitOfWork.Repository<Enrollment>()
                                .FindBy(a => a.CourseId == courseId && a.GradeId == gradeId)
                                .Select(a => new StudentToReturnDto
                                {
                                    StudentId = a.StudentId,
                                    FirstName = a.Student.FirstName,
                                    LastName = a.Student.LastName,
                                })
                                .ToListAsync();

            return ApiResponse<List<StudentToReturnDto>>.Success(studentGradedPerCourse);
        }

        public async Task<ApiResponse<List<GradeToReturnDto>>> GetAllGradesAsync()
        {
            try
            {
                var grades = await _unitOfWork.Repository<Grade>()
                    .FindBy(a => true)
                    .Select(a => new GradeToReturnDto
                    {
                        GradeLevel = a.GradeLevel,
                        Id = a.Id,
                    })
                    .ToListAsync();

                return ApiResponse<List<GradeToReturnDto>>.Success(grades);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<List<GradeToReturnDto>>.Failure(HttpStatusCode.InternalServerError);
            }
        }
    }
}

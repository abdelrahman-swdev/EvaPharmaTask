using EvaPharmaTask.Infrastructure.Data.Repositories;
using EvaPharmaTask.Infrastructure.Data.Repositories.Implementation;
using EvaPharmaTask.Services.Common;
using EvaPharmaTask.Services.Implementation;
using EvaPharmaTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace EvaPharmaTask.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(e => e.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                    return new BadRequestObjectResult(new ApiValidationErrorResponse<string> { Errors = errors });
                };
            });

            return services;
        }
    }
}

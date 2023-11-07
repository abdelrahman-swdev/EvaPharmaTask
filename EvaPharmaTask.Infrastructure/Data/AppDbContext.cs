using EvaPharmaTask.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EvaPharmaTask.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SeedGradeData(builder);
        }

        private static void SeedGradeData(ModelBuilder builder)
        {
            builder.Entity<Grade>().HasData(new Grade[]
            {
                new Grade { Id = 1, GradeLevel = "A+"},
                new Grade { Id = 2, GradeLevel = "A"},
                new Grade { Id = 3, GradeLevel = "B+"},
                new Grade { Id = 4, GradeLevel = "B"},
                new Grade { Id = 5, GradeLevel = "C+"},
                new Grade { Id = 6, GradeLevel = "C"},
                new Grade { Id = 7, GradeLevel = "D+"},
                new Grade { Id = 8, GradeLevel = "D"},
                new Grade { Id = 9, GradeLevel = "F"},
            });
        }
    }
}

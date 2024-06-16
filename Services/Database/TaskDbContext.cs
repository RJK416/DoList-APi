using Microsoft.EntityFrameworkCore;
using FinalToDoAPI.Models;
using Task = FinalToDoAPI.Models.Task;

namespace FinalToDoAPI.Services.Database
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
        {
        }

        public DbSet<Task> Tasks => this.Set<Task>();
    }
}

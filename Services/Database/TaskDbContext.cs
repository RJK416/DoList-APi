using Microsoft.EntityFrameworkCore;
using FinalDoListAPI.Models;
using Task = FinalDoListAPI.Models.Task;

namespace FinalDoListAPI.Services.Database
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

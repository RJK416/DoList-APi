using Microsoft.EntityFrameworkCore;

namespace FinalToDoAPI.Services.Database
{
    public class ToDoListDbContext : DbContext
    {
        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options)
        : base(options)
        {
        }
    }
}

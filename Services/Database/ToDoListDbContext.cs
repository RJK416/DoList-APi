using Microsoft.EntityFrameworkCore;

namespace FinalDoListAPI.Services.Database
{
    public class ToDoListDbContext : DbContext
    {
        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options)
        : base(options)
        {
        }
    }
}

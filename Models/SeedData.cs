using Microsoft.EntityFrameworkCore;
using FinalToDoAPI.Services.Database;

namespace FinalToDoAPI.Models;

public static class SeedData
{
    public static void EnsureAccount(IApplicationBuilder app)
    {
        AccountDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<AccountDbContext>();

        TaskDbContext TaskContext = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<TaskDbContext>();

        //if (context.Database.GetPendingMigrations().Any())
        //{
        //    context.Database.Migrate();
        //}

        if (!context.Accounts.Any())
        {
            _ = context.Accounts.Add(new AccountModel { AccName = "Ora", Name = "Gega", LastName = "Ora", PasswordHash = "123" });
        }

    //    [Key]
    //    public int Id { get; set; }

    //    [ForeignKey("AccountModel")]
    //    public int AccID { get; set; }

    //public string Name { get; set; } = "default";

    //public string Description { get; set; } = "default";

    //public DateTime CreateDate { get; set; } = DateTime.Now;

    //public DateTime DeadlineDate { get; set; }

    //public bool IsDone { get; set; }

        if (!TaskContext.Tasks.Any())
        {
            _ = TaskContext.Tasks.Add(new Task { Id = 0, AccID = 0, Name = "TestTask", Description = "TestTaskDes", DeadlineDate = DateTime.Today });
        }

        _ = context.SaveChanges();
    }
}

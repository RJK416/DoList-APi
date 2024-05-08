using Microsoft.EntityFrameworkCore;
using FinalDoListAPI.Models;

namespace FinalDoListAPI.Services.Database
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
        {
        }

        public DbSet<AccountModel> Accounts => this.Set<AccountModel>();
    }
}

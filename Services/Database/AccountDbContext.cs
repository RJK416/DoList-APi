using Microsoft.EntityFrameworkCore;
using FinalToDoAPI.Models;

namespace FinalToDoAPI.Services.Database
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

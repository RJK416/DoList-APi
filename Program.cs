using FinalToDoAPI.Models;
using FinalToDoAPI.Models.Repository;
using FinalToDoAPI.Services.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("https://localhost:7049")
                                        .AllowCredentials()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
        });


        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/MyAccount/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        // Set the session cookie
        options.Cookie.IsEssential = true; // Indicates that the cookie is essential for authentication
    });

        builder.Services.AddSession(options =>
        {
            // Configure session options as needed
            options.Cookie.IsEssential = true; // Make the session cookie essential
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c =>
                        (c.Type == ClaimTypes.Role || c.Type == "IsAdmin") && c.Value == "true")));
        });


        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddControllers();

        builder.Services.AddDbContext<AccountDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AccountConnection")));

        builder.Services.AddDbContext<TaskDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("TaskConnection")));

        string accountConnectionString = builder.Configuration.GetConnectionString("TaskConnection");


        builder.Services.AddScoped<ITaskRepository, TaskRepository>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowSpecificOrigin");

        app.UseHttpsRedirection();


        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();

        app.MapControllers();


        SeedData.EnsureAccount(app);

        app.Run();
    }
}
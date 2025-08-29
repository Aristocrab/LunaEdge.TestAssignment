using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Database;
using Service.Tasks;
using Service.Tasks.Repositories;
using Service.Users;
using Service.Users.JwtTokens;
using Service.Users.PasswordHashing;
using Service.Users.Repositories;

namespace Service;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        builder.Services.AddTransient<IPasswordHashingService, PasswordHashingService>();

        builder.Services.AddScoped<IUsersRepository, UsersRepository>();
        builder.Services.AddScoped<IUsersService, IUsersService>();
        
        builder.Services.AddScoped<ITasksRepository, TasksRepository>();
        builder.Services.AddScoped<ITasksService, ITasksService>();
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres:ConnectionString")));

        builder.Services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}
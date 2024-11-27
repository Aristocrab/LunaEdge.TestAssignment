using System.Text;
using FluentValidation;
using LunaEdge.TestAssignment.Application.Database;
using LunaEdge.TestAssignment.Application.Database.Repositories;
using LunaEdge.TestAssignment.Application.Features.Users;
using LunaEdge.TestAssignment.Application.Features.Users.Validators;
using LunaEdge.TestAssignment.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace LunaEdge.TestAssignment.WebApi;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
    {
        // Add controllers
        builder.Services.AddControllers();
        
        // Add services
        builder.Services.Scan(scan => scan
            .FromAssemblyOf<IUsersService>()
            .AddClasses(classes => 
                classes.Where(x => x.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );
        
        // ProblemDetails
        builder.Services.AddProblemDetails();

        // Add database
        builder.AddNpgsqlDbContext<AppDbContext>("postgresDb");
        builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        
        // FluentValidation
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
        
        // Auth
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
                };
            });
        builder.Services.AddAuthorization();
        
        // Add service defaults & Aspire client integrations.
        builder.AddServiceDefaults();

        // Add Serilog with OpenTelemetry
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
                var headers = builder.Configuration["OTEL_EXPORTER_OTLP_HEADERS"]?.Split(',') ?? [];
                foreach (var header in headers)
                {
                    var (key, value) = header.Split('=') switch
                    {
                        [{ } k, { } v] => (k, v),
                        var v => throw new Exception($"Invalid header format {v}")
                    };

                    options.Headers.Add(key, value);
                }
                options.ResourceAttributes.Add("service.name", "apiservice");

                var (otelResourceAttribute, otelResourceAttributeValue) = builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"]?.Split('=') switch
                {
                    [{ } k, { } v] => (k, v),
                    _ => throw new Exception($"Invalid header format {builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"]}")
                };

                options.ResourceAttributes.Add(otelResourceAttribute, otelResourceAttributeValue);
            })
            .CreateLogger();
        builder.Services.AddSerilog(logger);
        
        return builder;
    }
}
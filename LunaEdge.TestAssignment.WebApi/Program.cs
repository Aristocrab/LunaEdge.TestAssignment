using LunaEdge.TestAssignment.Application.Database;
using LunaEdge.TestAssignment.ServiceDefaults;
using LunaEdge.TestAssignment.WebApi;
using LunaEdge.TestAssignment.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddApiServices();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapControllers();
app.MapDefaultEndpoints();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LunaEdge.TestAssignment.WebApi v1");
    c.RoutePrefix = string.Empty;
});

// redirect root to swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
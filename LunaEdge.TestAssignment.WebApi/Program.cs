using LunaEdge.TestAssignment.Application.Database;
using LunaEdge.TestAssignment.ServiceDefaults;
using LunaEdge.TestAssignment.WebApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddApiServices();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

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
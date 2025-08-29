using Serilog;
using Service;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.AddWebApiServices();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
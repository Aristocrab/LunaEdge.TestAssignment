using LunaEdge.TestAssignment.ServiceDefaults;
using LunaEdge.TestAssignment.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.AddWebServices();

var app = builder.Build();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
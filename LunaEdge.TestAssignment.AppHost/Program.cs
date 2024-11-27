using Serilog;

var builder = DistributedApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Services.AddSerilog(logger);

builder.AddProject<Projects.LunaEdge_TestAssignment_WebApi>("apiservice");

builder.Build().Run();

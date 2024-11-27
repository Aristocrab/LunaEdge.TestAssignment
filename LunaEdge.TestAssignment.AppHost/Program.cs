using Serilog;

var builder = DistributedApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Services.AddSerilog(logger);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithPgWeb();

var db = postgres
    .AddDatabase("postgresDb");

builder
    .AddProject<Projects.LunaEdge_TestAssignment_WebApi>("apiservice")
    .WithReference(db)
    .WaitFor(postgres);

builder.Build().Run();

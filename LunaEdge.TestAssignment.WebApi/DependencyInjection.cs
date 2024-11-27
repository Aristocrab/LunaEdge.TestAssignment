using LunaEdge.TestAssignment.ServiceDefaults;
using Serilog;

namespace LunaEdge.TestAssignment.WebApi;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddWebServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        
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
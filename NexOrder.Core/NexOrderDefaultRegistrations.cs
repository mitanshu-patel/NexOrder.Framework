using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NexOrder.Framework.Core.Common;
using NexOrder.Framework.Core.Contracts;
using NexOrder.Framework.Core.Services;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace NexOrder.Framework.Core
{
    public static class NexOrderDefaultRegistrations
    {
        public static void AddNexOrderCustomLogging(this IServiceCollection services, bool isDevelopment, string serviceName, string? loggingConnection)
        {
            services.AddScoped<IMediator, Mediator>();

            services
            .AddOpenTelemetry()
            .ConfigureResource(v => v.AddService(serviceName))
            .UseFunctionsWorkerDefaults()
            .WithTracing(builder => {
                builder
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddAzureMonitorTraceExporter(o => {
                        o.ConnectionString = loggingConnection;
                    });
            });

            services.AddLogging(v =>
            {
                if (isDevelopment)
                {
                    v.AddConsole();
                }
                v.AddOpenTelemetry(options =>
                {
                    options.AddAzureMonitorLogExporter(o => o.ConnectionString = loggingConnection);

                    options.IncludeFormattedMessage = true;
                    options.IncludeScopes = true;
                });
            });
        }

        public static void AddMessageDeliveryService(this IServiceCollection services, Action<ServiceBusOptions> action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            var options = new ServiceBusOptions();
            action(options);
            options.CheckConfiguration();
            services.AddSingleton<IMessageDeliveryService>(serviceProvider =>
            {
                return new MessageDeliveryService(options);
            });
        }
    }
}

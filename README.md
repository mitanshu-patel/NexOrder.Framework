NexOrder.Framework
===================

Core building blocks for NexOrder applications. This repository provides a lightweight core library (packaged for NuGet) that contains message delivery integration, a simple in-process mediator, handler abstractions, and common response/result types.

Key points
---------
- Target framework: `net8.0` (.NET 8)
- Primary library project: `NexOrder.Core` (package id from project: `NexOrder.Framework.Core`, version `1.0.0`)
- Integrations: Azure Service Bus, FluentValidation, OpenTelemetry helpers

Quick install (NuGet)
---------------------

Install the published package:

```
dotnet add package NexOrder.Framework.Core --version 1.0.0
```

Build and pack locally
----------------------

From the repository root you can build and create the NuGet package:

```
dotnet build ./NexOrder.Core/NexOrder.Framework.Core.csproj -c Release
dotnet pack ./NexOrder.Core/NexOrder.Framework.Core.csproj -c Release -o ./nupkgs
```

The produced `.nupkg` will be placed in `nupkgs` when packing locally.

Usage
-----

Register the library services in your application's composition root (example for a typical .NET host):

```csharp
// var builder = WebApplication.CreateBuilder(args);
// var services = builder.Services;
// var configuration = builder.Configuration;

// Example registration helper provided by the library
// NexOrderDefaultRegistrations.AddNexOrderCore(services, configuration);
```

Program.cs registrations
------------------------

Example showing how to register handlers, logging (OpenTelemetry / Azure Monitor) and Service Bus using the helpers in `HandlerRegistration` and `NexOrderDefaultRegistrations`.

```csharp
using System.Reflection;
using NexOrder.Framework.Core;
using NexOrder.Framework.Core.Common;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// 1) Logging / OpenTelemetry / Mediator
services.AddNexOrderCustomLogging(
    isDevelopment: builder.Environment.IsDevelopment(),
    serviceName: "MyService", // friendly service name for telemetry
    loggingConnection: configuration["ApplicationInsights:ConnectionString"]
);

// 2) Register handlers from an assembly (scans for IHandler<,> implementations)
// You can target a specific assembly that contains your handlers.
services.RegisterHandlers(Assembly.GetExecutingAssembly());

// 3) Service Bus registration (message delivery)
services.AddMessageDeliveryService(options =>
{
    options.ServiceBusConnectionString = configuration["ServiceBus:ConnectionString"] ?? string.Empty;
    // optional proxy or other fields
    options.WebProxyAddress = configuration["ServiceBus:WebProxyAddress"] ?? string.Empty;
});

var app = builder.Build();
// ... configure middleware and run
app.Run();
```

Notes:
- Call `AddNexOrderCustomLogging` before `RegisterHandlers` so mediator (registered by the logging helper) is available when handlers are resolved.
- `RegisterHandlers` finds and registers implementations of `IHandler<TRequest, TResponse>` automatically as transient services.
- `AddMessageDeliveryService` expects you to provide a `ServiceBusOptions` configuration via the provided action; it will validate the presence of a connection string at startup.

Configuration
-------------

Service Bus configuration is read via a configuration model (`ServiceBusOptions`). Example `appsettings.json` section:

```json
{
  "ServiceBus": {
    "ConnectionString": "<your-connection-string>",
    "QueueName": "<queue-name>"
  }
}
```

Public surface (high level)
---------------------------
- `IMessageDeliveryService` / `MessageDeliveryService` — send messages to Service Bus
- `IMediator` / `Mediator` — dispatch requests to handlers
- `IHandler<TRequest, TResponse>` / handler base types
- Common response types: `CustomResponse<T>`, `CustomHttpResult`, `MessageResult`
- `NexOrderDefaultRegistrations` and `Extensions` — DI helpers and configuration wiring

Project structure
-----------------
The repository is organized to keep the core library (`NexOrder.Core`) focused and reusable. A high-level view of the main files and folders:

```
NexOrder.Core/
  NexOrder.Framework.Core.csproj        # Project file (NuGet package metadata)
  README.md                            # Package README (NuGet-focused)
  Common/                              # Shared helper types and DTOs
    CustomResponse.cs                   # Response wrapper types
    CustomHttpResult.cs                 # Helper factory for HTTP-like results
    MessageResult.cs                    # Message delivery result model
    ValidationErrorBuilder.cs           # Builder for validation errors
    HandlerRegistration.cs              # Helper to register handlers into DI
    Extensions.cs                       # Extension methods for DI/config
    ServiceBusOptions.cs                # Configuration model for Service Bus
    RequestHandlerBase.cs                # Base class for request handlers
  Contracts/                            # Public interfaces and contracts
    IMessageDeliveryService.cs          # Abstraction for sending messages
    IHandler.cs                          # Generic handler contract
    IMediator.cs                         # Mediator abstraction
  Services/                             # Implementations
    MessageDeliveryService.cs            # Service Bus integration
    Mediator.cs                          # In-process mediator implementation
```

Explanation:
- `Common` contains small, composable types and helpers used across the library (responses, results, configuration models, error builders).
- `Contracts` defines the public interfaces consumers program against (delivery, mediator and handler contracts).
- `Services` contains concrete implementations of the contracts — for example `MessageDeliveryService` implements `IMessageDeliveryService` and wires Azure Service Bus usage.
- `NexOrder.Framework.Core.csproj` is configured to produce the NuGet package; see `NexOrder.Core/README.md` for package-specific guidance.

Contributing
------------

This project accepts issues and pull requests. Please follow repository coding conventions and add tests for new behavior.

License
-------

See repository root for license information.

More information
----------------

See the `NexOrder.Core/README.md` for NuGet-oriented usage notes and examples specific to the core package.

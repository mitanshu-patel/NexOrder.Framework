# NexOrder.Framework.Core

Core library for NexOrder applications providing common building blocks for message delivery, mediator pattern handling, and shared response types. Packaged for NuGet distribution.

## Package

Install from NuGet:

```
dotnet add package NexOrder.Framework.Core --version 1.0.0
```

(Version `1.0.0` matches the package produced by the project file. Confirm the exact package id and version if it was changed.)

## Overview

This package includes:

- `IMessageDeliveryService` / `MessageDeliveryService` — abstractions and implementation for delivering messages (Service Bus integration).
- `IMediator` / `Mediator` — a lightweight mediator for in-process request/response handling.
- `IHandler<TRequest, TResponse>` and `RequestHandlerBase<TRequest, TResponse>` — handler pattern base types.
- `HandlerRegistration` — helper for registering handlers.
- Common result/response types: `CustomHttpResult`, `CustomResponse`, `MessageResult`.
- `ServiceBusOptions` — configuration model for Service Bus settings.
- `NexOrderDefaultRegistrations` and `Extensions` — helpers to register library services into DI and to wire configuration.

## Quick start

1. Add the package to your project (see Installation above).
2. Register the library services in your host application's composition root (Program.cs / Startup):

```
// Example (adapt to your project):
// var builder = WebApplication.CreateBuilder(args);
// var services = builder.Services;
// var configuration = builder.Configuration;

// Call the registration helper provided by the library
// Check `NexOrderDefaultRegistrations` for the exact registration method and overloads.
```

3. Configure Service Bus settings in your configuration (appsettings.json):

```json
{
  "ServiceBus": {
    "ConnectionString": "<your-connection-string>",
    "QueueName": "<queue-name>"
  }
}
```

4. Inject and use:

```
// IMessageDeliveryService for sending messages
// IMediator for request/response dispatch to registered handlers
```

## Usage notes

- See the `NexOrderDefaultRegistrations` implementation for the exact DI registration API and any optional configuration hooks.
- The package targets .NET 8.0 and uses `ServiceBus` and standard Microsoft.Extensions packages.

## Contributing and Support

Report issues or contribute via the repository: `https://github.com/mitanshu-patel/NexOrder.Framework`.

## License

See repository for license information.

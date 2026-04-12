using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using NexOrder.Framework.Core.Common;
using NexOrder.Framework.Core.Contracts;
using System.Net;
using System.Text.Json;

namespace NexOrder.Framework.Core.Services
{
    public class MessageDeliveryService : IMessageDeliveryService
    {
        private ServiceBusOptions serviceBusOptions;

        public MessageDeliveryService(ServiceBusOptions serviceBusOptions)
        {
            this.serviceBusOptions = serviceBusOptions;
        }

        public async Task PublishMessageAsync<T>(T requestBody, string topicName)
        {
            var options = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets,
            };

            if (!string.IsNullOrEmpty(this.serviceBusOptions.WebProxyAddress))
            {
                options.WebProxy = new WebProxy(this.serviceBusOptions.WebProxyAddress, true);
            }

            var client = new ServiceBusClient(this.serviceBusOptions.ServiceBusConnectionString, options);
            var sender = client.CreateSender(topicName);
            var customBody = new
            {
                FullName = requestBody.GetType().FullName,
                Data = requestBody,
            };
            var body = JsonSerializer.Serialize(customBody);
            var message = new ServiceBusMessage(body);
            await sender.SendMessageAsync(message);
        }
    }
}

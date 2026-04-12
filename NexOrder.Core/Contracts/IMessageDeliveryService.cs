namespace NexOrder.Framework.Core.Contracts
{
    public interface IMessageDeliveryService
    {
        Task PublishMessageAsync<T>(T requestBody, string topicName);
    }
}

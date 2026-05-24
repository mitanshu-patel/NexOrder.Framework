using OpenAI.Chat;

namespace NexOrder.Framework.Core.Contracts
{
    public interface IOpenAIService
    {
        public void SetSystemMessage(string systemMessage);

        public Task<string> GenerateResponseAsyc(string userMessage, ChatCompletionOptions? chatCompletionOptions = null);

        public void InitializeOpenAIService();
    }
}

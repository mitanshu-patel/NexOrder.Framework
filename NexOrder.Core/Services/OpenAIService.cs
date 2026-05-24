using Azure;
using Azure.AI.OpenAI;
using NexOrder.Framework.Core.Common;
using NexOrder.Framework.Core.Contracts;
using OpenAI.Chat;

namespace NexOrder.Framework.Core.Services
{
    public class OpenAIService : IOpenAIService
    {
        private string systemMessage = string.Empty;
        private readonly OpenAIOptions openAIOptions;
        private AzureOpenAIClient? azureOpenAIClient = null;
        private ChatClient? chatClient = null;

        public OpenAIService(OpenAIOptions openAIOptions)
        {
            this.openAIOptions = openAIOptions;
        }
        public async Task<string> GenerateResponseAsyc(string userMessage, ChatCompletionOptions? chatCompletionOptions = null)
        {
            ArgumentNullException.ThrowIfNull(this.azureOpenAIClient, nameof(azureOpenAIClient));
            ArgumentNullException.ThrowIfNull(this.chatClient, nameof(chatClient));
            List<ChatMessage> messages =
            [
                new SystemChatMessage(this.systemMessage),
                new UserChatMessage(userMessage),
            ];

            var response = await chatClient.CompleteChatAsync(messages, chatCompletionOptions);
            return response.Value.Content[0].Text ?? string.Empty;
        }

        public void SetSystemMessage(string systemMessage)
        {
            this.systemMessage = systemMessage;
        }

        public void InitializeOpenAIService()
        {
            this.azureOpenAIClient = new(
            new Uri(this.openAIOptions.Url),
            new AzureKeyCredential(this.openAIOptions.ApiKey));
            this.chatClient = this.azureOpenAIClient.GetChatClient(this.openAIOptions.DeploymentName);
        }
    }
}

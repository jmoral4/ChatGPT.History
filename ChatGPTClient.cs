using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;

namespace ChatGPTHistory
{

    public enum ChatModels { GPT4, GPT3Turbo }

    public class ChatGPTClient
    {
        private readonly OpenAIService _api;
        private readonly List<ChatMessage> _conversation;
        private readonly List<string> _history;
        private readonly int MaxTokens;
        ChatModels _model;

        public ChatGPTClient(string apiKey, ChatModels model, int maxTokens)
        {
            _api = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = apiKey
            });
            _model = model;
            MaxTokens= maxTokens;
            _conversation = new List<ChatMessage>();
            _history= new List<string>();
        }



        public void AppendAssistantMessage(string message)
        {
            _conversation.Add(ChatMessage.FromAssistant(message));            
        }

        public void AppendSystemMessage(string message)
        {
            _conversation.Add(ChatMessage.FromSystem(message));            
        }

        public void AppendUserInput(string message)
        {
            _conversation.Add(ChatMessage.FromUser(message));
        }

        public async Task<string> GetResponseFromChatbotAsync()
        {
            
            var completionResult = await _api.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = _conversation,
                Model = Models.ChatGpt3_5Turbo,
                MaxTokens = MaxTokens//optional
            }) ;
            if (completionResult.Successful)
            {
                foreach (var opt in completionResult.Choices)
                {
                    _history.Add($"{opt.Message.Role}:{opt.Message.Content}");
                }

                return completionResult.Choices.First().Message.Content;
            }
            else
            {
                return "Failed to process result.";
            }
        }

        public IReadOnlyList<string> GetMessageHistory()
        {
            return _history;
        }
    }

}

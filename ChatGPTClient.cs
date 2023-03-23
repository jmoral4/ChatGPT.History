using OpenAI_API.Chat;
using OpenAI_API.Models;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTHistory
{
    public class ChatGPTClient
    {
        private readonly OpenAIAPI _api;
        private readonly Conversation _conversation;

        public ChatGPTClient(string apiKey)
        {
            _api = new OpenAIAPI(apiKey);
            _conversation = _api.Chat.CreateConversation();
            _conversation.Model = Model.ChatGPTTurbo;
        }

        public void AppendSystemMessage(string message)
        {
            _conversation.AppendSystemMessage(message);
        }

        public void AppendUserInput(string message)
        {
            _conversation.AppendUserInput(message);
        }

        public async Task<string> GetResponseFromChatbotAsync()
        {
            return await _conversation.GetResponseFromChatbot();
        }

        public IReadOnlyList<ChatMessage> GetMessageHistory()
        {
            return _conversation.Messages;
        }
    }

}

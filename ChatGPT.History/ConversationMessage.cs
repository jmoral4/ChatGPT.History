using System.ComponentModel.DataAnnotations;

namespace ChatGPT.History
{
    public class ConversationMessage
    {
        [Key]
        public int ConversationMessageId { get; set; }
        public int ConversationId { get;set; }
        public string Role { get; set; }   // role of the message
        public string Message { get; set; }  // message text        
        public DateTime DateCreated { get; set; } // timestamp of the message(received from OpenAI)

        public override string ToString()
        {
            return $"{ConversationMessageId}.{ConversationId}.{Role}.{Message}.{DateCreated.ToString("yyyyMMdd:HHmmss")}";
        }
    }
}
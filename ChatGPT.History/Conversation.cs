using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChatGPT.History
{
    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; }     //db primary key        
        public string SessionName { get; set; } // readable name of the session (user assigned)
        public List<ConversationMessage> Messages { get; set; }

        public Conversation()
        {
            Messages = new List<ConversationMessage>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{this.ConversationId}.{this.SessionName}.Count:{this.Messages.Count}\r\n");            
            if (Messages.Count > 0)
            {
                foreach(var message in Messages)
                {
                    sb.Append($"{message.ToString()}\r\n");
                }                
            }
            return sb.ToString();
        }
    }
}
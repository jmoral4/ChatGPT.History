using Microsoft.EntityFrameworkCore;

namespace ChatGPT.History
{
    class SqlitePersistance
    {
        HistoryDbConfig config;
        
        public SqlitePersistance(HistoryDbConfig config)
        {
            this.config = config;
        }
        public bool Exists()
        {
            var db = new HistoryContext(config);
            return db.Database.CanConnect();
        }

        public void EnsureCreateTable()
        {
            var db = new HistoryContext(config);
            db.Database.EnsureCreated();
        }

        public bool HasData()
        {
            var db = new HistoryContext(config);
            return db.Conversations.Any();
        }

        public Conversation GetConversationByConversationId(int id)
        {
            using var db = new HistoryContext(config);            
            var convo = db.Conversations.Where(x => x.ConversationId == id).FirstOrDefault();
            return convo;
        }

        public async Task<int> UpsertConversation(Conversation conversation)
        {
            using var db = new HistoryContext(config);
            db.Database.EnsureCreated();            
            db.Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;
            db.Conversations.Update(conversation);
            int recordsUpdated = await db.SaveChangesAsync();
            return recordsUpdated;
        }        

    }
}
using Microsoft.Data.Sqlite;
using System.IO;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace ChatGPT.History
{
    public class HistoryManager
    { 
        // sqlite db context
        private readonly HistoryDbConfig _config;
        private readonly SqlitePersistance _db;

        public HistoryManager(HistoryDbConfig config) {
            this._config = config;
            _db = new SqlitePersistance(config);
        }

        public Conversation GetHistorybySessionId(int conversationId)
        {
            return _db.GetConversationByConversationId(conversationId);
        }

        public async Task<int> AddOrUpdateConversation(Conversation convo)
        {
           return await _db.UpsertConversation(convo);
        }


    }
}
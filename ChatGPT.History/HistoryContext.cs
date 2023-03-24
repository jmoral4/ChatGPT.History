using Microsoft.EntityFrameworkCore;

namespace ChatGPT.History
{
    class HistoryContext : DbContext
    {
        HistoryDbConfig config;
        public DbSet<Conversation> Conversations { get; set; }        
        public HistoryContext(HistoryDbConfig config) { 
            this.config = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConversationMessage>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("getdate()");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!Directory.Exists(config.DbFolderPrefix))
                Directory.CreateDirectory(config.DbFolderPrefix);

            var path = Path.Combine(config.DbFolderPrefix, config.DatabaseFilename);
            options.UseSqlite($"Data Source=\"{path}\";Pooling=False;");
            Console.WriteLine($"SQLite configured for:{path}");            
        }
    }
}
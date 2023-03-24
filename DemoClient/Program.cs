using System.IO;
using System.Linq;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Azure.Core.Diagnostics;
using ChatGPT.History;
using Microsoft.Extensions.Azure;

namespace ChatGPT.ChatConsole
{
    internal class Program
    {
       
        static async Task Main(string[] args)
        {
            //OPTIONAL turn on AZ logging: using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();
            Stopwatch st = new Stopwatch();
            Conversation conversation = new Conversation();
            Random r = new Random();
            conversation.SessionName = r.Next().ToString();
            Console.WriteLine("Loading ChatGPT Commandline Client. Type: quit! to exit.");

            // Load configuration and retrieve API key
            // OPTIONAL REMOVE BELOW HERE ----------------------------------------------------
            ConfigurationManager configManager = new ConfigurationManager();
            string vaultUri = configManager.GetVaultUri();
            string secretName = configManager.GetSecretName();
            var aad = configManager.GetADCredentials();
            KeyVaultManager keyVaultManager = new KeyVaultManager(vaultUri,aad);
            string apiKey = await keyVaultManager.GetSecretAsync(secretName);
            // OPTIONAL UNCOMMENT BELOW ------------------------------------------------------
            // string apiKey = "<your OpenAI Api Key>";

            Console.WriteLine("Retrieved API key: " + apiKey);
            ChatGPTClient chatGPTClient = new ChatGPTClient(apiKey, ChatModels.GPT3Turbo, 500);

            var exitRequested = false;

            while (!exitRequested)
            {
                var sysprompt = await ReadUserInputAsync("Enter a System Prompt [optional]:");
                if (!string.IsNullOrEmpty(sysprompt))
                {
                    if (string.Equals(sysprompt, "quit!", StringComparison.InvariantCultureIgnoreCase) )
                    {
                        exitRequested = true;
                        break;
                    }
                    chatGPTClient.AppendSystemMessage(sysprompt);
                }

                var userprompt = await ReadUserInputAsync("Enter your Prompt:");
                if (!string.IsNullOrEmpty(userprompt))
                {
                    if (string.Equals(userprompt, "quit!", StringComparison.InvariantCultureIgnoreCase) || userprompt.StartsWith("quit"))
                    {
                        exitRequested = true;
                        break;
                    }
                    chatGPTClient.AppendUserInput(userprompt);
                    
                    st.Start();
                    string response = await chatGPTClient.GetResponseFromChatbotAsync();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(response);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Responded in {st.ElapsedMilliseconds}ms");
                    Console.ResetColor();
                    st.Reset();

                    foreach (var msg in chatGPTClient.GetMessageHistory())
                    {
                        conversation.Messages.Add(msg);                        
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Nothing to do...");
                    Console.ResetColor();
                }

                if (string.Equals(sysprompt, "quit!", StringComparison.InvariantCultureIgnoreCase) || string.Equals(userprompt, "quit!", StringComparison.InvariantCultureIgnoreCase) || userprompt.StartsWith("quit"))
                {
                    exitRequested = true;
                }
            }


            Console.WriteLine("------------HISTORY ------------------");
            // the entire chat history is available in chat.Messages
            foreach (var  msg in conversation.Messages)
            {
                Console.WriteLine($"{msg.Role}:{msg.Message}");
            }

            HistoryDbConfig dbConfig = new HistoryDbConfig() { DatabaseFilename = "ChatGPT.db", DbFolderPrefix = "History" };
            HistoryManager hm = new HistoryManager(dbConfig);

            var items = await hm.AddOrUpdateConversation(conversation);
            Console.WriteLine($"Saved {items} to Sqlite db.");
            
            // also save to text for redundancy/sanity while debugging
            string historyFilename = $"ChatGPT_Convo_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.md";
            File.WriteAllText(historyFilename, conversation.ToString());

            Console.WriteLine($"Rest easy chummer, All History was logged to: {dbConfig.DatabaseFilename}");

        }

        private static async Task<string> ReadUserInputAsync(string prompt, ConsoleColor color = ConsoleColor.Yellow)
        {
            Console.ForegroundColor = color;
            Console.Write(prompt);
            Console.ResetColor();
            return await Task.Run(() => Console.ReadLine());
        }


    }
}
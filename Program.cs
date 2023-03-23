using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.IO;
using System.Linq;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ChatGPTHistory
{
    internal class Program
    {
       
        static async Task Main(string[] args)
        {

            Stopwatch st = new Stopwatch();
            List<string> messageHistory = new List<string>();
            Console.WriteLine("Loading ChatGPT Commandline Client. Type: quit! to exit.");

            // Load configuration and retrieve API key
            ConfigurationManager configManager = new ConfigurationManager();
            string vaultUri = configManager.GetVaultUri();
            string secretName = configManager.GetSecretName();

            KeyVaultManager keyVaultManager = new KeyVaultManager(vaultUri);
            string apiKey = await keyVaultManager.GetSecretAsync(secretName);

            Console.WriteLine("Retrieved API key: " + apiKey);
            ChatGPTClient chatGPTClient = new ChatGPTClient(apiKey);

            var exitRequested = false;

            while (!exitRequested)
            {
                var sysprompt = await ReadUserInputAsync("Enter a System Prompt [optional]:");
                if (!string.IsNullOrEmpty(sysprompt))
                {
                    chatGPTClient.AppendSystemMessage(sysprompt);
                }

                var userprompt = await ReadUserInputAsync("Enter your Prompt:");
                if (!string.IsNullOrEmpty(userprompt))
                {
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

                    foreach (ChatMessage msg in chatGPTClient.GetMessageHistory())
                    {
                        messageHistory.Add($"{msg.Role}: {msg.Content}");
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
            foreach (var  msg in messageHistory)
            {
                Console.WriteLine($"{msg}");
            }

            var flname = $"ChatGptHistory{DateTime.Now.ToString("yyyyMMdd.HHmmss")}.md";
            // Write messageHistory to file
            File.WriteAllLines(flname, messageHistory);

            Console.WriteLine($"Rest easy chummer, All History was logged to: {flname}");

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
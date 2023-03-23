using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTHistory
{
    public class KeyVaultManager
    {
        private readonly SecretClient _client;

        public KeyVaultManager(string vaultUri)
        {
            _client = new SecretClient(new Uri(vaultUri), new DefaultAzureCredential());
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            Response<KeyVaultSecret> secretResponse = await _client.GetSecretAsync(secretName);
            return secretResponse.Value.Value;
        }
    }

}

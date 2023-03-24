using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace ChatGPT.ChatConsole
{
    public class KeyVaultManager
    {
        private readonly SecretClient _client;

        public KeyVaultManager(string vaultUri, AADCredentials aad)
        {
            _client = new SecretClient(new Uri(vaultUri), new ClientSecretCredential(aad.TenantId, aad.ClientId, aad.ClientSecret));
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            Response<KeyVaultSecret> secretResponse = await _client.GetSecretAsync(secretName);
            return secretResponse.Value.Value;
        }
    }

}

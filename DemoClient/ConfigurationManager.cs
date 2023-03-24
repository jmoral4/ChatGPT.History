using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPT.ChatConsole
{
    public class ConfigurationManager
    {
        private readonly IConfiguration _configuration;

        public ConfigurationManager()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public string GetVaultUri()
        {
            return _configuration["KeyVaultSettings:VaultUri"];
        }

        public string GetSecretName()
        {
            return _configuration["KeyVaultSettings:SecretName"];
        }

        public AADCredentials GetADCredentials()
        {
            return new AADCredentials()
            {
                TenantId = _configuration["AAD:TenantId"],
                ClientId = _configuration["AAD:ClientId"],
                ClientSecret = _configuration["AAD:ClientSecret"]
            };
        }
        
    }

    public class AADCredentials
    { 
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

}

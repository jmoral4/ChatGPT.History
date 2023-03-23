using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTHistory
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
    }

}

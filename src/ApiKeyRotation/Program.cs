using System;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1;
using Microsoft.Extensions.Logging;
using ApiKeyRotation.Services;
using DotNetEnv;

namespace ApiKeyRotation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting API Key Rotation...");

            // Load environment variables from .env file
            Env.Load();

            var vaultToken = Environment.GetEnvironmentVariable("VAULT_TOKEN");
            var vaultAddress = Environment.GetEnvironmentVariable("VAULT_ADDR");

            if (string.IsNullOrEmpty(vaultToken) || string.IsNullOrEmpty(vaultAddress))
            {
                Console.WriteLine("Vault token or address not found in environment variables.");
                return;
            }

            // Create dependencies
            var authMethod = new TokenAuthMethodInfo(vaultToken);
            var vaultSettings = new VaultClientSettings(vaultAddress, authMethod);
            IVaultClient vaultClient = new VaultClient(vaultSettings);

            ILogger<VaultService> logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger<VaultService>();

            // Pass dependencies to VaultService
            var vaultService = new VaultService(vaultClient, logger);

            var apiKey = vaultService.RotateKey("example-key-id");

            Console.WriteLine($"New API Key: {apiKey.Key}");
        }
    }
}

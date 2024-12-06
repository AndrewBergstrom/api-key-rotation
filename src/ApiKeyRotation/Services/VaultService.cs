using System;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1;
using ApiKeyRotation.Models;
using Microsoft.Extensions.Logging;

namespace ApiKeyRotation.Services
{
    public class VaultService
    {
        private readonly IVaultClient _vaultClient;
        private readonly ILogger<VaultService> _logger;

        // Constructor for dependency injection (e.g., for testing)
        public VaultService(IVaultClient vaultClient, ILogger<VaultService> logger)
        {
            _vaultClient = vaultClient ?? throw new ArgumentNullException(nameof(vaultClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

 public ApiKey RotateKey(string keyId)
{
    if (string.IsNullOrEmpty(keyId))
    {
        _logger?.LogError("Key ID cannot be null or empty.");
        throw new ArgumentException("Key ID cannot be null or empty.", nameof(keyId));
    }

    var newKey = Guid.NewGuid().ToString();

    _logger?.LogInformation($"Generated new key for Key ID: {keyId}");

    // Specify the correct mountPoint (secret/)
    _vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(
        path: $"api-key/{keyId}",
        data: new { Key = newKey },
        mountPoint: "secret" // Ensure the mountPoint matches your Vault configuration
    ).Wait();

    return new ApiKey { Id = keyId, Key = newKey };
}
    }
}

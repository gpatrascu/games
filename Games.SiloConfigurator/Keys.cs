using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace games.api.Azure;

public class Keys
{
    public static Response<KeyVaultSecret> GetStorageAccountConnectionString()
    {
        var keyVaultName = "biddinggamekeyvault";
        var kvUri = "https://" + keyVaultName + ".vault.azure.net";
        var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

        return client.GetSecret("storage-account-connection-string");
    }
}
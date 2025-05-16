using System.Text.Json;
using Azure.Storage;
using Azure.Storage.Blobs;

namespace DevOpsQuickScan.Infrastructure;

public class BaseRepository
{
    protected BlobContainerClient ContainerClient { get; }

    protected JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        IncludeFields = true
    };

    protected BaseRepository(string storageAccountName, string storageAccountKey, string containerUrl)
    {
        var credentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
        ContainerClient = new BlobContainerClient(new Uri(containerUrl), credentials);
    }
}
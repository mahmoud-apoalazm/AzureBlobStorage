using Azure.Storage;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploader;

public class AzureBlobService
{
    private readonly string _storageAccount= "mahmoudblob20023";
    private readonly string _accessKey = "oTd3W4xOVQNaZP1jhyEOD87JOlCILY9jvSmTefY5Be78uKzbujuF3zKS2KM+dV0ypOe1WlpIpQkm+AStlStKqA==";
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobService()
    {
        var credential =new StorageSharedKeyCredential(_storageAccount, _accessKey);
        var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
        _blobServiceClient =new BlobServiceClient(new Uri(blobUri), credential);
    }

    public async Task ListBlobContainerAsync()
    {
        var containers=_blobServiceClient.GetBlobContainersAsync();

       await foreach(var container in containers)
        {
            Console.WriteLine(container.Name);
        }
    }
    public async Task<List<Uri>> UploadFileAsync()
    {
        var blobUris=new List<Uri>();
        string filePath = "hello.txt";
        var blobcontainer = _blobServiceClient.GetBlobContainerClient("files");

        var blob =blobcontainer.GetBlobClient($"today/{filePath}");
        var tommorowBlob = blobcontainer.GetBlobClient($"tommorow/{filePath}");

        await blob.UploadAsync(filePath, true);
        blobUris.Add(blob.Uri);
        await tommorowBlob.UploadAsync(filePath, true);
        blobUris.Add(tommorowBlob.Uri);

        return blobUris;
    }
    public async Task<Uri> UploadAsStreamAsync()
    {
        
        string filePath = "hello.txt";
        var blobcontainer = _blobServiceClient.GetBlobContainerClient("files");

        var blob = blobcontainer.GetBlobClient($"today/{filePath}");
        FileStream fileStream= File.OpenRead(filePath);
        await blob.UploadAsync(fileStream, true);
        fileStream.Close();

        return blob.Uri;
    }
    public async Task GetFlatBlobsListAsync()
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient("files");
        var blobs=blobContainer.GetBlobsAsync();

        await foreach(var blob in blobs)
        {
            Console.WriteLine(format: "Blob name: {0}", blob.Name);
        }
    }

    public async Task GetHierachicalBlobsListAsync()
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient("files");
        var blobs = blobContainer.GetBlobsByHierarchyAsync();

        await foreach (var blob in blobs)
        {
            if (blob.IsPrefix)
            {
                Console.WriteLine("Virtual directory prefix: {0}", blob.Prefix);

                await GetHierachicalBlobsListAsync();
            }
            else
            {
                Console.WriteLine(format: "Blob name: {0}", blob.Blob.Name);

            }
        }
    }

    public async Task DeleteBlobAsync()
    {
        string filePath = "hello.txt";
        var blobcontainer = _blobServiceClient.GetBlobContainerClient("files");

        var blob = blobcontainer.GetBlobClient($"today/{filePath}");

        await blob.DeleteIfExistsAsync();
    }
}

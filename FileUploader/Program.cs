

using FileUploader;

var service = new AzureBlobService();
await service.ListBlobContainerAsync();
//await service.UploadFileAsync();

await service.GetFlatBlobsListAsync();
Console.WriteLine("=================================");
await service.GetHierachicalBlobsListAsync();
await service.DeleteBlobAsync();





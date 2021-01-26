using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Core.Interfaces.Azure;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services.Azure
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly AzureBlobSettings settings;
        public AzureBlobService(AzureBlobSettings settings)
        {
            this.settings = settings;
        }

        public async Task<bool> Delete(string blobName)
        {
            BlobContainerClient container = new BlobContainerClient(settings.ConnectionString, settings.ContainerName);
            container.CreateIfNotExists(PublicAccessType.BlobContainer);
            BlobClient blobClient = container.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
            return true;
        }

        public async Task<string> GetBlobAsBase64(string blobName)
        {
            string base64String = string.Empty;
            BlobContainerClient container = new BlobContainerClient(settings.ConnectionString, settings.ContainerName);
            container.CreateIfNotExists(PublicAccessType.BlobContainer);
            BlobClient blobClient = container.GetBlobClient(blobName);
            var ms = new MemoryStream();
            var response = await blobClient.DownloadToAsync(ms);
            base64String = Convert.ToBase64String(ms.ToArray());
            return base64String;
        }

        public async Task<string> Insert(string tableName, string fileName, string base64Content)
        {
            BlobContainerClient container = new BlobContainerClient(settings.ConnectionString, settings.ContainerName);
            container.CreateIfNotExists(PublicAccessType.BlobContainer);
            var blobName = tableName + "/" + Guid.NewGuid().ToString() + fileName;
            BlobClient blobClient = container.GetBlobClient(blobName);
            byte[] bytes = Convert.FromBase64String(base64Content);
            MemoryStream ms = new MemoryStream(bytes);
            await blobClient.UploadAsync(ms, true);
            ms.Close();
            return blobName;
        }


        public async Task<bool> Update(string blobName, string base64Content)
        {
            BlobContainerClient container = new BlobContainerClient(settings.ConnectionString, settings.ContainerName);
            container.CreateIfNotExists(PublicAccessType.BlobContainer);
            BlobClient blobClient = container.GetBlobClient(blobName);
            byte[] bytes = Convert.FromBase64String(base64Content);
            MemoryStream ms = new MemoryStream(bytes);
            await blobClient.UploadAsync(ms, true);
            ms.Close();
            return true;
        }

        public async Task<string> InsertFile(string tableName, string fileName, string base64Content, bool isPublic)
        {
            BlobContainerClient container = this.GetContainer(isPublic);
            var blobName = tableName + "/" + Guid.NewGuid().ToString() + fileName;
            BlobClient blobClient = container.GetBlobClient(blobName);
            byte[] bytes = Convert.FromBase64String(base64Content);
            MemoryStream ms = new MemoryStream(bytes);
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }

            await blobClient.UploadAsync(ms, new BlobHttpHeaders { ContentType = contentType });
            return blobName;
        }

        private BlobContainerClient GetContainer(bool isPublic = false)
        {

            BlobContainerClient container = new BlobContainerClient(settings.ConnectionString, isPublic == true ? settings.PublicContainerName : settings.PrivateContainerName);
            container.CreateIfNotExists(isPublic == true ? PublicAccessType.BlobContainer : PublicAccessType.None);
            return container;

        }

        public async Task<bool> DeleteFile(string blobName, bool isPublic = false)
        {
            BlobContainerClient container = this.GetContainer(isPublic);
            BlobClient blobClient = container.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
            return true;
        }

        public string GetBlobSasUri(string blobName, bool isPublic = false)
        {
            BlobContainerClient container = this.GetContainer(isPublic);
            // Create a SAS token that's valid for a day.
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = isPublic == true ? settings.PublicContainerName : settings.PrivateContainerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddDays(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(1),

            };
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
            string sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(settings.StorageSharedKeyCredential.AccountName, settings.StorageSharedKeyCredential.AccountKey)).ToString();
            return container.GetBlockBlobClient(blobName).Uri + "?" + sasToken;
        }

        public string GetUri(string blobName, bool isPublic = false)
        {
            BlobContainerClient container = this.GetContainer(isPublic);
            return container.GetBlockBlobClient(blobName).Uri.ToString();
        }
    }
}

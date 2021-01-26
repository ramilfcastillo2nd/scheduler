using System.Threading.Tasks;

namespace Core.Interfaces.Azure
{
    public interface IAzureBlobService
    {
        Task<string> Insert(string tableName, string fileName, string base64Content);
        Task<bool> Update(string blobName, string base64Content);
        Task<bool> Delete(string blobName);
        Task<string> GetBlobAsBase64(string blobName);

        //File Engine
        Task<string> InsertFile(string tableName, string fileName, string base64Content, bool isPublic);
        Task<bool> DeleteFile(string blobName, bool isPublic = false);

        string GetBlobSasUri(string blobName, bool ispublic = false);
        string GetUri(string blobName, bool isPublic = false);
    }
}

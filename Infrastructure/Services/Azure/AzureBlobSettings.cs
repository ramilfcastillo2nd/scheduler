namespace Infrastructure.Services.Azure
{
    public class AzureBlobSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string PublicContainerName { get; set; } = string.Empty;
        public string PrivateContainerName { get; set; } = string.Empty;
        public string ContainerName { get; set; }
        public Storagesharedkeycredential StorageSharedKeyCredential { get; set; } = new Storagesharedkeycredential();
    }

    public class Storagesharedkeycredential
    {
        public string AccountName { get; set; } = string.Empty;
        public string AccountKey { get; set; } = string.Empty;
    }


    public static class AzureBlobSettingsConstants
    {
        public const string IMAGES = "images";
    }
}

using Microsoft.AspNetCore.Http;

namespace Core.Dtos.Schedulers.Input
{
    public class UploadSchedulerInputDto
    {
        public int UserProfileId { get; set; }
        public int CampaignId { get; set; }
        public IFormFile CsvData { get; set; }
    }
}

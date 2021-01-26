using Core.Dtos.AzureBlobs;
using Core.Interfaces.Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using scheduler_azure.api.Errors;
using System.Threading.Tasks;

namespace scheduler_azure.api.Controllers
{
    public class AzureBlobController : BaseApiController
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly IConfiguration _config;
        public string BaseUrl = string.Empty;
        public AzureBlobController(IConfiguration config, IAzureBlobService azureBlobService)
        {
            _azureBlobService = azureBlobService;
            _config = config;
            BaseUrl = _config["AzureBlobSettings:BaseUrl"];
        }
        [AllowAnonymous]
        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromBody] UploadImageInputDto request)
        {
            try
            {
                var url = await _azureBlobService.Insert("images", request.ImageFileName, request.Base64String);
                url = $"{BaseUrl}/{url}"; 
                return Ok(new { Url = url });
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong. Please try again"));
            }
        }

        [AllowAnonymous]
        [HttpDelete("image")]
        public async Task<IActionResult> DeleteImage([FromBody] DeleteImageInputDto request)
        {
            try
            {
                var blob = request.Url.Replace(BaseUrl, "").Trim();
                var url = await _azureBlobService.Delete(blob);
                return Ok(new ApiResponse(200,"Successfully deleted image blob!"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong. Please try again"));
            }
        }
    }
}

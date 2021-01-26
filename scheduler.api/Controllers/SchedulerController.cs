using AutoMapper;
using Core.Dtos.Campaigns.Output;
using Core.Dtos.Schedulers.Input;
using Core.Dtos.Schedulers.Output;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using scheduler.api.Errors;
using scheduler.api.Extensions;
using scheduler.api.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace scheduler.api.Controllers
{
    public class SchedulerController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ISchedulerService _schedulerService;
        private readonly UserManager<AppUser> _userManager;
        public SchedulerController(ISchedulerService schedulerService, IMapper mapper, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _schedulerService = schedulerService;
            _userManager = userManager;
        }

        [Authorize(Roles = "sdr")]
        [HttpGet("currentuser")]
        public async Task<IActionResult> GetSchedulerBySdr([FromQuery]CommonSpecParams specParams)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

                var schedulerResponse = await _schedulerService.GetSchedulers(user.Id, specParams);

                var schedulersMapped = _mapper.Map<IReadOnlyList<SchedulerOutputDto>>(schedulerResponse.Item2);
                var campaignMapped = _mapper.Map<GetCampaignForSchedulerOutputDto>(schedulerResponse.Item3);
                var contentMapped = _mapper.Map<ContentOutputDto>(schedulerResponse.Item4);
                campaignMapped.Content = contentMapped;
                var count = schedulerResponse.Item1;

                return Ok(new { 
                    Schedulers = new Pagination<SchedulerOutputDto>(specParams.PageIndex, specParams.PageSize, count, schedulersMapped),
                    Campaign = campaignMapped
                });
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateScheduler([FromBody] UpdateSchedulerInputDto request)
        {
            try
            {
                var scheduler = await _schedulerService.GetSchedulerById(request.id);
                if (scheduler == null)
                    return BadRequest(new ApiResponse(400, "Scheduler is not existing."));

                var validationResponse = await _schedulerService.ValidateUpdateInput(request);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                scheduler.FirstName = request.FirstName;
                scheduler.LastName = request.LastName;
                scheduler.JobTitle = request.JobTitle;
                scheduler.Company = request.Company;
                scheduler.Industry = request.Industry;
                scheduler.City = request.City;
                scheduler.State = request.State;
                scheduler.LinkedInUrl = request.LinkedInUrl;
                scheduler.ConnectionRequest = request.ConnectionRequest;
                scheduler.Date = request.Date;
                scheduler.RequestAccepted = request.RequestAccepted;
                scheduler.IsMessage1 = request.IsMessage1;
                scheduler.DateMessage1 = request.DateMessage1;
                scheduler.IsMessage2 = request.IsMessage2;
                scheduler.DateMessage2 = request.DateMessage2;
                scheduler.IsMessage3 = request.IsMessage3;
                scheduler.DateMessage3 = request.DateMessage3;
                scheduler.IsMessage4 = request.IsMessage4;
                scheduler.DateMessage4 = request.DateMessage4;
                scheduler.IsMessage5 = request.IsMessage5;
                scheduler.DateMessage5 = request.DateMessage5;
                scheduler.Neutral = request.Neutral;
                scheduler.Negative = request.Negative;
                await _schedulerService.UpdateScheduler(scheduler);

                return Ok(new ApiResponse(200, "Success"));
            }
            catch(Exception x)
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedulerById(int id)
        {
            try
            {
                var scheduler = await _schedulerService.GetSchedulerById(id);
                if(scheduler == null)
                    return BadRequest(new ApiResponse(400, "Scheduler is not existing."));

                var schedulerMapped = _mapper.Map<SchedulerOutputDto>(scheduler);
                return Ok(schedulerMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize]
        [HttpGet("{campaignId}/{userProfileId}")]
        public async Task<IActionResult> GetScheduler(int campaignId, int userProfileId, [FromQuery] CommonSpecParams specParams)
        {
            try
            {

                var validationResponse = await _schedulerService.ValidateGetInput(campaignId, userProfileId);
                if(!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var schedulerResponse = await _schedulerService.GetSchedulers(campaignId, userProfileId, specParams);

                var schedulersMapped = _mapper.Map<IReadOnlyList<SchedulerOutputDto>>(schedulerResponse.Item2);
                var count = schedulerResponse.Item1;

                return Ok(new Pagination<SchedulerOutputDto>(specParams.PageIndex, specParams.PageSize, count, schedulersMapped));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadScheduler([FromForm] UploadSchedulerInputDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

                var validationResponse = await _schedulerService.ValidateUploadInput(request);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var schedulers = await ReadSchedulerCsv(request.CsvData);
                if (schedulers == null)
                    return BadRequest(new ApiResponse(400, "File has no content."));

                var content = schedulers.Select(s => s.Result).ToList();
                var states = content.Select(s => s.Location).ToArray();

                var contentMapped = _mapper.Map<List<Scheduler>>(content);

                await _schedulerService.InsertScheduler(contentMapped, request, user.Id.ToString());

                return Ok(new ApiResponse(200, "Success"));
            }
            catch(Exception x)
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        private async Task<List<CsvMappingResult<SchedulerFileInputDto>>> ReadSchedulerCsv(IFormFile formFile)
        {
            try
            {
                var filePath = Path.GetTempFileName();

                if (formFile.Length > 0)
                {

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }

                CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                CsvSchedulersMapping csvMapper = new CsvSchedulersMapping();
                CsvParser<SchedulerFileInputDto> csvParser = new CsvParser<SchedulerFileInputDto>(csvParserOptions, csvMapper);
                var result = csvParser
                             .ReadFromFile($"{filePath}", Encoding.ASCII)
                             .ToList();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}

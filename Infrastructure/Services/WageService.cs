using Core.Dtos.Validation.Output;
using Core.Dtos.Wages.Input;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class WageService : IWageService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public WageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateWage(Wage request)
        {
            _unitOfWork.Repository<Wage>().Add(request);
            await _unitOfWork.Complete();
        }

        public async Task DeleteWage(Wage wage)
        {
            _unitOfWork.Repository<Wage>().Delete(wage);
            await _unitOfWork.Complete();
        }

        public async Task<Wage> GetWageInfoByIdAsync(int id)
        {
            var department = await _unitOfWork.Repository<Wage>().GetByIdAsync(id);
            return department;
        }

        public async Task<IReadOnlyList<Wage>> GetWagesAsync(CommonSpecParams specParams)
        {
            var specs = new GetWagesSpecification(specParams);
            return await _unitOfWork.Repository<Wage>().ListAsync(specs);
        }

        public async Task UpdateWage(Wage request)
        {
            _unitOfWork.Repository<Wage>().Update(request);
            await _unitOfWork.Complete();
        }

        public async Task<ValidationOutputDto> ValidateCreateInputAsync(AddWageInputDto request)
        {
            if (string.IsNullOrEmpty(request.SdrType))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Sdr Type is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.CampaignType))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign type is a required field.",
                    StatusCode = 400
                };

            if (request.BasePay == 0)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Base pay must be greater than 0.",
                    StatusCode = 400
                };

            //var wageInfo = await _unitOfWork.Repository<Wage>().GetEntityWithSpec

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public async Task<ValidationOutputDto> ValidateUpdateInput(UpdateWageInputDto request)
        {
            if (string.IsNullOrEmpty(request.SdrType))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Sdr Type is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.CampaignType))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign type is a required field.",
                    StatusCode = 400
                };

            if (request.BasePay == 0)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Base pay must be greater than 0.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }
    }
}

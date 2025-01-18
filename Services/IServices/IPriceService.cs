using HomeExam.AlertManagementAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HomeExam.AlertManagementAPI.Services.IServices
{
    public interface IPriceService
    {
        Task UpdatePrice(IEnumerable<PriceAlertDto> priceAlertDtoList);
    }
}

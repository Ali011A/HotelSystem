using Hotel.Application.DTOs.OfferDTOs;
using Hotel.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet]
        public async Task<List<OfferResponseDto>> GetAll()
        {
            return await _offerService.GetAllOffersAsync();
        }

        [HttpGet("{id}")]
        public async Task<OfferResponseDto?> GetById(Guid id)
        {
            return await _offerService.GetOfferByIdAsync(id);
        }

        [HttpPost]
      //  [Authorize(Roles = "Staff")]
        public async Task<OfferResponseDto> Add(OfferCreateDto dto)
        {
            // هنا لازم تجيب staffId من الـ Claims بعد الـ Login
           var staffId = Guid.Parse(User.FindFirst("StaffId")!.Value);

           return await _offerService.AddOfferAsync(dto, staffId);
        }

        [HttpPut("{id}")]
      //  [Authorize(Roles = "Staff")]
        public async Task<OfferResponseDto?> Update(Guid id, OfferUpdateDto dto)
        {
            return await _offerService.UpdateOfferAsync(id, dto);
        }

        [HttpDelete("{id}")]
      //  [Authorize(Roles = "Staff")]
        public async Task<bool> Delete(Guid id)
        {
            return await _offerService.DeleteOfferAsync(id);
        }
    }

}

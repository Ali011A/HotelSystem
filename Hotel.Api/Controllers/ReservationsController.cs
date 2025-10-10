using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;
         private readonly IMapper _mapper;
 
         public ReservationsController(IReservationService service, IMapper mapper)
         {
             _service = service;
             _mapper = mapper;
         }
 
         [HttpPost]
    
         public async Task<ActionResult<ReservationViewModel>> Create([FromBody] CreateReservationDto dto, CancellationToken ct)
         {
             var vm = await _service.CreateAsync(dto, ct);
             return CreatedAtAction(nameof(GetById), new { id = vm.Id}, vm);
         }
 
         [HttpGet("{id:guid}")]
        
         public async Task<ActionResult<ReservationViewModel>> GetById(Guid id, CancellationToken ct)
         {
               var vm = await _service.GetByIdAsync(id, ct);
                 if (vm == null) return NotFound();
                 return Ok(vm);
             }
 
         [HttpGet]
       
         public async Task<ActionResult<PagedResponse<ReservationViewModel>>> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
         {
     var paged = await _service.ListAsync(page, pageSize, ct);
                 return Ok(paged);
             }
 
         [HttpPut("{id:guid}")]
   
         public async Task<ActionResult<ReservationViewModel>> Update(Guid id, [FromBody] UpdateReservationDto dto, CancellationToken ct)
         {
     var vm = await _service.UpdateAsync(id, dto, ct);
                 return Ok(vm);
             }
 
        [HttpDelete("{id:guid}")]
       
         public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
         {
     await _service.CancelAsync(id, ct);
                 return NoContent();
             }
    }
}

using AutoMapper;
using Hotel.Api.ResponseViewModel;
using Hotel.Api.ViewModel;
using Hotel.Application.DTOs;
using Hotel.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        IMapper Mapper { get; set; }

        public FeedbackController(IFeedbackService feedbackService,IMapper mapper)
        {
            _feedbackService = feedbackService;
            Mapper = mapper;
        }

        [HttpPost("create-feedback")]
        public async Task<ResponseViewModel<CreateFeedbackViewModel>> CreateFeedback([FromBody] CreateFeedbackViewModel model)
        {
            var feedbackDto = Mapper.Map<CreateFeedbackDto>(model);

            var result = await _feedbackService.CreateAsync(feedbackDto);

            return new ResponseViewModel<CreateFeedbackViewModel>
            {
                Data = model,
                IsSuccess = result.Success,
                Massage = result.ErroredMessage,

            };
        }


        [HttpGet("getall-feedback")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var result = await _feedbackService.GetAllFeedbacksAsync();

            if(!ModelState.IsValid)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("get-feedback/{id:guid}")] 
        public async Task<IActionResult> GetFeedback(Guid id)
        {
            var result = await _feedbackService.GetFeedbackAsync(id);
            if(!ModelState.IsValid)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("edite-feedback/{id:guid}")]
        public async Task<ResponseViewModel<UpdateFeedbackViewModel>> UpdateFeedback(Guid id, [FromBody] UpdateFeedbackViewModel model)
        {
            var dto = Mapper.Map<UpdateFeedbackDto>(model);

            var result = await _feedbackService.UpdateAsync(id, dto);
            return new ResponseViewModel<UpdateFeedbackViewModel>
            {
                Data = model,
                IsSuccess = result.Success,
                Massage = result.ErroredMessage,
            };
        }

        [HttpDelete("delete-feedback/{id:guid}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var result = await _feedbackService.DeleteAsync(id);
            if(!ModelState.IsValid)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}

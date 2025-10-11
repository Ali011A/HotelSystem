using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Application.Services;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Interfaces.UnitOfWork;
using Hotel.Domain.Models;
using Hotel.Application.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Service
{
    public class FeedbackService : IFeedbackService
    {
        IFeedbackRepository FeedbackRepository { get; set; }

        IMapper Mapper { get; set; }
        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper )
        {
            FeedbackRepository = feedbackRepository;
            Mapper = mapper;
           
        }
        public async Task<ResultModelVoid> CreateAsync(CreateFeedbackDto dto)
        {
            var canAdd = await FeedbackRepository.CustomerHasCompletedReservationAsync(dto.ReservationId);

            if (!canAdd)
            {
                return new ResultModelVoid
                {
                    Success = false,
                    ErroredMessage = "Reservation not completed or not found for this customer."
                };
            }

            
            var entity = Mapper.Map<Feedback>(dto);
           
            var savedRows = await FeedbackRepository.AddAsync(entity);

            if (savedRows > 0)
            {
                return new ResultModelVoid
                {
                    Success = true,
                    SuccessFullMessage = "Feedback added successfully."
                };
            }

            return new ResultModelVoid
            {
                Success = false,
                ErroredMessage = "Failed to save feedback."
            };
        }

  
    
        public async Task<ResultModel<Guid>> UpdateAsync(Guid id, UpdateFeedbackDto dto, CancellationToken cancellation = default)
        {
            var existing = await FeedbackRepository.GetByIdAsync(id, cancellation);

            if (existing == null)
            {
                return new ResultModel<Guid>
                {
                    Success = false,
                    ErroredMessage = "Feedback not found.",
                    Data = Guid.Empty
                };
            }

            
            Mapper.Map(dto, existing);

            
            var savedRows = await FeedbackRepository.UpdateAsync(existing);

            if (savedRows > 0)
            {
                return new ResultModel<Guid>
                {
                    Success = true,
                    SuccessFullMessage = "Feedback updated successfully.",
                    Data = existing.Id
                };
            }

            return new ResultModel<Guid>
            {
                Success = false,
                ErroredMessage = "Failed to update feedback. No changes were applied."
          
            };
        }

     

        public async Task<ResultModelVoid> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var savedRows = await FeedbackRepository.SoftDeleteAsync(id, cancellation);

            if (savedRows > 0)
            {
                return new ResultModelVoid
                {
                    Success = true,
                    SuccessFullMessage = "Feedback is deleted successfully."
                };
            }
           
            return new ResultModelVoid
            {
                Success = false,
                ErroredMessage = "Failed to delete feedback. It may not exist."
            };
        }

      
       
        public async Task<ResultModel<CreateFeedbackDto>> GetFeedbackAsync(Guid id, CancellationToken cancellation = default)
        {
            var feedback = await FeedbackRepository.GetByIdAsync(id, cancellation);

            if (feedback == null)
            {
                return new ResultModel<CreateFeedbackDto>
                {
                    Success = false,
                    ErroredMessage = $"Feedback with ID '{id}' not found.",
                    Data = null
                };
            }

            var dto = Mapper.Map<CreateFeedbackDto>(feedback);

            return new ResultModel<CreateFeedbackDto>
            {
                Success = true,
                SuccessFullMessage = "Feedback retrieved successfully.",
                Data = dto
            };
        }

       
        public async Task<ResultModel<List<CreateFeedbackDto>>> GetAllFeedbacksAsync()
        {
            var feedbacks = await FeedbackRepository.GetAllFeedBaksAsync();

            if (feedbacks != null)
            {
                var dtos = Mapper.Map<List<CreateFeedbackDto>>(feedbacks);

                return new ResultModel<List<CreateFeedbackDto>>
                {
                    Success = true,
                    SuccessFullMessage = "All feedbacks retrieved successfully.",
                    Data = dtos
                };
            }

            return new ResultModel<List<CreateFeedbackDto>>
            {
                Success = false,
                ErroredMessage = "Failed to retrieve all feedbacks.",
                Data = null
            };
        }
    }
}
    


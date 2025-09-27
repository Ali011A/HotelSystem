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

        IUnitOfWork UnitOfWork { get; set; }

        IMapper Mapper { get; set; }
        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            FeedbackRepository = feedbackRepository;
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }
        public async Task<ResultModelVoid> CreateAsync(CreateFeedbackDto dto)
        {
            
            var canAdd = await FeedbackRepository.CustomerHasCompletedReservationAsync(dto.ReservationId);
            if (!canAdd) throw new InvalidOperationException("Reservation not completed or not found for this customer.");

            var entity = Mapper.Map<Feedback>(dto);
            await FeedbackRepository.AddAsync(entity);

            var Saved = await UnitOfWork.SaveChangesAsync();

            if (Saved)
                return 
                    new ResultModelVoid 
                    { 
                        Success = true ,
                        SuccessFullMessage ="Added Successfully FeedBack"
                    };

            return new ResultModelVoid 
            { 
                Success = false,
                ErroredMessage = "Failed to save feedback." 
            };
        }

        public async Task<ResultModel<Guid>> UpdateAsync(Guid id, UpdateFeedbackDto dto, CancellationToken cancellation = default)
        {
            var existing = await FeedbackRepository.GetByIdAsync(id, cancellation);

            if (existing == null) throw new KeyNotFoundException("Feedback not found");

            Mapper.Map(dto, existing);

            await FeedbackRepository.UpdateAsync(existing);

            var seved= await UnitOfWork.SaveChangesAsync();

            if (seved)

                return new ResultModel<Guid>
                {
                    Success = true,
                    SuccessFullMessage = " updated feed back Successfully",
                    Data = existing.Id
                };

            return new ResultModel<Guid>
            {
                Success = false,
                ErroredMessage = "faild updated feedback"
            };
           
        }

        public async Task<ResultModelVoid> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            await FeedbackRepository.SoftDeleteAsync(id, cancellation);

            var save = await UnitOfWork.SaveChangesAsync();

            if (save)
                return new ResultModelVoid
                {
                    Success = true,
                    SuccessFullMessage = " feed back is deleted"
                };
            return new ResultModelVoid
            {
                Success = false,
                ErroredMessage = " faild deleted feedback"
            };
        }

        public async Task<ResultModel<CreateFeedbackDto>> GetFeedBack(Guid id, CancellationToken cancellation = default)
        {
          var feedback =  await FeedbackRepository.GetByIdAsync(id);

           var dto = Mapper.Map<CreateFeedbackDto>(feedback);

            if(dto!=null)

            return new ResultModel<CreateFeedbackDto>
            {
                Success = true,
                SuccessFullMessage = "retrived feedback ",
                Data = dto

            };

            return new ResultModel<CreateFeedbackDto>
            {
                Success = false,
                ErroredMessage = " faild to retrived feedback",
                Data = null
            };
        }

        public async Task<ResultModel<List<CreateFeedbackDto>>> GettAllFeedBacks()
        {
            var feedbacks = await FeedbackRepository.GetAllFeedBaksAsync();

            if (feedbacks != null)
            {
                var dtos = Mapper.Map<List<CreateFeedbackDto>>(feedbacks);

                return new ResultModel<List<CreateFeedbackDto>>
                {
                    Success = true,
                    SuccessFullMessage = "retrived all feedbacks",
                    Data = dtos
                };

            }

            return new ResultModel<List<CreateFeedbackDto>>
            {
                Success = false,
                ErroredMessage = " faild to retruved all feedbacks",

            };

        }


    }
}

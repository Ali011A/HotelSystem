using Hotel.Application.DTOs;
using Hotel.Application.ResultModel;
using Hotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Services
{
    public interface IFeedbackService
    {
         Task<ResultModelVoid> CreateAsync(CreateFeedbackDto dto);
        Task<ResultModel<Guid>> UpdateAsync(Guid id, UpdateFeedbackDto dto, CancellationToken cancellation = default);
        Task<ResultModelVoid> DeleteAsync(Guid id, CancellationToken cancellation = default);
        Task<ResultModel<CreateFeedbackDto>> GetFeedbackAsync(Guid id, CancellationToken cancellation = default);
        Task<ResultModel<List<CreateFeedbackDto>>> GetAllFeedbacksAsync();


    }
}

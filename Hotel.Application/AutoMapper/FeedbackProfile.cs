using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.AutoMapper
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            // Create -> Entity
            CreateMap<CreateFeedbackDto, Feedback>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

            // Update -> Entity
            CreateMap<UpdateFeedbackDto, Feedback>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ReservationId, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // Entity -> DTO (للعرض)
            CreateMap<Feedback, CreateFeedbackDto>();
            CreateMap<Feedback, CreateFeedbackDto>();


            
        }
    }
}

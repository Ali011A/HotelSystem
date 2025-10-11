using AutoMapper;
using Hotel.Api.ViewModel;
using Hotel.Application.DTOs;

namespace Hotel.Api.AuoroMapper
{
    public class AuotoMaperProfile : Profile
    {
        public AuotoMaperProfile()
        {
            CreateMap<CreateFeedbackViewModel, CreateFeedbackDto>().ReverseMap();
            CreateMap<UpdateFeedbackViewModel, UpdateFeedbackDto>().ReverseMap();




        }
    }
}

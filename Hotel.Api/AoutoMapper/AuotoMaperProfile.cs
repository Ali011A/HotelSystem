using AutoMapper;
using Hotel.Api.ViewModel;
using Hotel.Application.DTOs;
using Hotel.Domain.Models;

namespace Hotel.Api.AuoroMapper
{
    public class AuotoMaperProfile : Profile
    {
        public AuotoMaperProfile()
        {
            CreateMap<CreateFeedbackViewModel, CreateFeedbackDto>().ReverseMap();
            CreateMap<UpdateFeedbackViewModel, UpdateFeedbackDto>().ReverseMap();
            CreateMap<CreateFeedbackDto, Feedback>().ReverseMap();
            CreateMap<UpdateFeedbackDto, Feedback>().ReverseMap();


            CreateMap<ReservationViewModel, CreateReservationDto> ().ReverseMap();
            CreateMap<CreateReservationDto, Reservation>().ReverseMap();
            CreateMap<ReservationViewModel, Reservation>().ReverseMap();
            CreateMap<ReservationDetailsDto, ReservationViewModel> ().ReverseMap();
            CreateMap<UpdateReservationDto, ReservationViewModel>().ReverseMap();   







        }
    }
}

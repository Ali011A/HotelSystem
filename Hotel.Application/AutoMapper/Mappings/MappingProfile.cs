using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.AutoMapper.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {

            //CreateMap<Reservation, ReservationViewModel>()
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            //    .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.room.RoomNumber ))
            //    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName));
            //CreateMap<CreateReservationDto, Reservation>();
            CreateMap<ReservationDetailsDto, ReservationViewModel>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNumber))
          .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
          .ForMember(dest => dest.CheckinDate, opt => opt.MapFrom(src => src.CheckinDate))
          .ForMember(dest => dest.CheckoutDate, opt => opt.MapFrom(src => src.CheckoutDate))
          .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));


            // 2) Fallback: Map from Reservation entity -> ReservationViewModel
            //    Use safe navigation for navigation properties (room/customer) in case they are null.
            CreateMap<Reservation, ReservationViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoomNumber,
                           opt => opt.MapFrom(src => src.room != null ? src.room.RoomNumber : 0))
                .ForMember(dest => dest.CustomerName,
                           opt => opt.MapFrom(src => (src.Customer != null ? (src.Customer.FirstName ?? "") + " " + (src.Customer.LastName ?? "") : string.Empty)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CheckinDate, opt => opt.MapFrom(src => src.CheckinDate))
                .ForMember(dest => dest.CheckoutDate, opt => opt.MapFrom(src => src.CheckoutDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));


            // 3) Map from CreateReservationDto -> Reservation (for create commands)
            //    Note: we intentionally do NOT map BookingDate/Id here; the Service should set BookingDate and Id.
            CreateMap<CreateReservationDto, Reservation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BookingDate, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Nights, opt => opt.Ignore());
                

        }
    }
}

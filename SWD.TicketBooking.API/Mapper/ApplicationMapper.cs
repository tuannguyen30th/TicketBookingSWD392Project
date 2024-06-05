using AutoMapper;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.User;


namespace SWD.TicketBooking.API.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            ////////User/////////
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<SignUpRequest, SignUpModel>().ReverseMap();
            CreateMap<UserResponse, UserModel>().ReverseMap();

            CreateMap<User, CreateUserReq>().ReverseMap();

            CreateMap<UpdateUserModel, UpdateUserReq>().ReverseMap();
            CreateMap<User, UpdateUserModel>().ReverseMap();

            ///////Route////////
            CreateMap<RouteResponse, RouteModel>().ReverseMap();
            CreateMap<SWD.TicketBooking.Repo.Entities.Route, RouteModel>().ReverseMap();

            //////Trip/////////
            CreateMap<PopularTripModel, PopularTripResponse>().ReverseMap();
        }
    }
}

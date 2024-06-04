using AutoMapper;
using SWD.TicketBooking.Repo.Common.RequestModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos.User;


namespace SWD.TicketBooking.Service.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            ////////User/////////
            CreateMap<User, UserModel>().ReverseMap();

            CreateMap<User, CreateUserReq>().ReverseMap();

        }
    }
}

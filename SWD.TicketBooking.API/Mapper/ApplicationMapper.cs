using AutoMapper;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.User;
using static SWD.TicketBooking.API.Common.ResponseModels.ServiceFromStationResponse;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStation;


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
            CreateMap<PictureModel,GetPictureResponse>().ReverseMap();
         //   CreateMap<Trip, PictureModel>().ReverseMap();

            /////Utility/////
            CreateMap<UtilityInTripResponse,UtilityModel > ().ReverseMap();
            CreateMap<FromCityToCityModel.CityModel, FromCityToCityRepsonse.CityResponse>().ReverseMap();
            CreateMap<FromCityToCityModel.CityInfo, FromCityToCityRepsonse.CityInfo>().ReverseMap();
            CreateMap<FeedbackRequestModel, FeedbackRequest>().ReverseMap();
            CreateMap<FeedbackRequestModel, Feedback>().ReverseMap();
            CreateMap<ServiceModel, ServiceFromStationResponse.ServiceResponse>();
            CreateMap<ServiceTypeModel, ServiceFromStationResponse.ServiceTypeResponse>()
                .ForMember(dest => dest.ServiceResponses, opt => opt.MapFrom(src => src.ServiceModels));
            CreateMap<StationFromRouteModel, StationFromRouteResponse>();



            ////Feedback////
            CreateMap<TripFeedbackModel, FeedbackInTripResponse>().ReverseMap();
        }
    }
}

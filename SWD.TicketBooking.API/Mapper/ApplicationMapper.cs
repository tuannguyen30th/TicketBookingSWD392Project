using AutoMapper;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.User;
using static SWD.TicketBooking.API.Common.RequestModels.CreateTripRequest;
using static SWD.TicketBooking.API.Common.ResponseModels.GetSeatBookedFromTripResponse;
using static SWD.TicketBooking.API.Common.ResponseModels.ServiceFromStationResponse;
using static SWD.TicketBooking.Service.Dtos.CreateTripModel;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;


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

            CreateMap<UpdateUserModel, UpdateUserRequest>().ReverseMap();
            CreateMap<User, UpdateUserModel>().ReverseMap();

            ///////Route////////
            CreateMap<RouteResponse, RouteModel>().ReverseMap();
            CreateMap<SWD.TicketBooking.Repo.Entities.Route, RouteModel>().ReverseMap();
            CreateMap<CreateRouteModel, CreateRouteRequest>().ReverseMap();
            CreateMap<UpdateRouteModel, UpdateRouteRequest>().ReverseMap();

            //////Trip/////////
            CreateMap<PopularTripModel, PopularTripResponse>().ReverseMap();
            CreateMap<PictureModel,GetPictureResponse>().ReverseMap();
            CreateMap<SearchTripModel,SearchTripResponse>().ReverseMap();
            CreateMap<CreateTripModel, CreateTripRequest>().ReverseMap();
            CreateMap<TicketType_TripRequest, TicketType_TripModel>().ReverseMap();
            CreateMap<Trip_UtilityRequest, Trip_UtilityModel>().ReverseMap();
            CreateMap<GetSeatBookedFromTripModel, GetSeatBookedFromTripResponse>().ReverseMap();
            CreateMap<TicketType_TripModel, TicketType_TripResponse>().ReverseMap();
            //   CreateMap<Trip, PictureModel>().ReverseMap();

            /////Utility/////
            CreateMap<UtilityInTripResponse,UtilityModel > ().ReverseMap();
            CreateMap<FromCityToCityModel.CityModel, FromCityToCityRepsonse.CityResponse>().ReverseMap();
            CreateMap<FromCityToCityModel.CityInfo, FromCityToCityRepsonse.CityInfo>().ReverseMap();
            CreateMap<FeedbackRequestModel, FeedbackRequest>().ReverseMap();
            CreateMap<FeedbackRequestModel, Feedback>().ReverseMap();
            CreateMap<ServiceModel, ServiceFromStationResponse.ServiceResponse>().ReverseMap();
            CreateMap<ServiceTypeModel, ServiceFromStationResponse.ServiceTypeResponse>()
                .ForMember(dest => dest.ServiceResponses, opt => opt.MapFrom(src => src.ServiceModels)).ReverseMap();
            CreateMap<StationFromRouteModel, StationFromRouteResponse>().ReverseMap();
            CreateMap<CreateServiceModel, CreateServiceRequest>().ReverseMap();
            CreateMap<UpdateServiceModel, UpdateServiceRequest>().ReverseMap();

               




            ////Feedback////
            CreateMap<TripFeedbackModel, FeedbackInTripResponse>().ReverseMap();




            ////Station////
            CreateMap<GetStationModel,Station>().ReverseMap();

            CreateMap<GetStationModel,GetStationResponse>().ReverseMap();   


            CreateMap<CreateStationModel,CreateStationRequest>().ReverseMap();

            /////Company/////
            CreateMap<GetCompanyModel, Company>().ReverseMap();
            CreateMap<GetCompanyModel, GetCompanyResponse>().ReverseMap();
            CreateMap<CreateCompanyModel, CreateCompanyRequest>().ReverseMap();


            /////City/////
            CreateMap<CreateCityModel, CreateCityRequest>().ReverseMap();

            /////TicketDetail/////
            CreateMap<GetTicketDetailByUserModel, GetTicketDetailByUserResponse>().ReverseMap();
            CreateMap<GetDetailTicketDetailByTicketDetailModel, GetDetailTicketDetailByTicketDetailResponse>().ReverseMap();
            CreateMap<SearchTicketModel, SearchTicketResponse>().ReverseMap();
        }
    }
}

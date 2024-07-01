using AutoMapper;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.Booking;
using SWD.TicketBooking.Service.Dtos.User;
using static SWD.TicketBooking.API.RequestModels.CreateTripRequest;
using static SWD.TicketBooking.API.ResponseModels.GetSeatBookedFromTripResponse;
using static SWD.TicketBooking.API.ResponseModels.ServiceFromStationResponse;
using static SWD.TicketBooking.Service.Dtos.CreateTripModel;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;
using SWD.TicketBooking.API.RequestModels.Booking;
using SWD.TicketBooking.API.ResponseModels;


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
            CreateMap<User, UpdateUserResponseModel>()
                .ReverseMap();
            CreateMap<User, UpdateUserModel>().ReverseMap();

            ///////Route////////
            CreateMap<RouteResponse, RouteModel>().ReverseMap();
            CreateMap<SWD.TicketBooking.Repo.Entities.Route, RouteModel>().ReverseMap();
            CreateMap<CreateRouteModel, CreateRouteRequest>().ReverseMap();
            CreateMap<UpdateRouteModel, UpdateRouteRequest>().ReverseMap();

            CreateMap<CreateRouteRequest, CreateRouteModel>()
            .ForMember(dest => dest.StationInRoutes, opt => opt.MapFrom(src => src.StationInRoutes));
            CreateMap<API.RequestModels.StationInRouteModel, Service.Dtos.StationInRouteModel>();

            //////Trip/////////
            CreateMap<PopularTripModel, PopularTripResponse>().ReverseMap();
            CreateMap<SearchTripModel,SearchTripResponse>().ReverseMap();
            CreateMap<CreateTripModel, CreateTripRequest>().ReverseMap();
            CreateMap<TicketType_TripRequest, TicketType_TripModel>().ReverseMap();
            CreateMap<Trip_UtilityRequest, Trip_UtilityModel>().ReverseMap();
            CreateMap<GetSeatBookedFromTripModel, GetSeatBookedFromTripResponse>().ReverseMap();
            CreateMap<TicketType_TripModel, TicketType_TripResponse>().ReverseMap();
            //   CreateMap<Trip, PictureModel>().ReverseMap();

            /////Utility/////
            CreateMap<UtilityInTripResponse,UtilityModel > ().ReverseMap();
            CreateMap<CreateNewUtilityRequest, CreateUtilityModel>().ReverseMap();
            CreateMap<UtilityReponse,Utility>().ReverseMap();

            CreateMap<FromCityToCityModel.CityModel, FromCityToCityRepsonse.CityResponse>().ReverseMap();
            CreateMap<FromCityToCityModel.CityInfo, FromCityToCityRepsonse.CityInfo>().ReverseMap();
            CreateMap<FeedbackRequestModel, FeedbackRequest>().ReverseMap();
            CreateMap<FeedbackRequestModel, Feedback>().ReverseMap();

            /////Service/////
            CreateMap<ServiceModel, ServiceResponse>().ReverseMap();
            CreateMap<GetServiceModel, SWD.TicketBooking.Repo.Entities.Service>().ReverseMap();
            CreateMap<GetServiceModel, GetServiceResponse>().ReverseMap();
            CreateMap<ServiceTypeModel, ServiceTypeResponse>().ReverseMap();

            /*        CreateMap<ServiceTypeModel, ServiceFromStationResponse.ServiceTypeResponse>()
                        .ForMember(dest => dest.ServiceResponse, opt => opt.MapFrom(src => src.ServiceModels)).ReverseMap();*/
            CreateMap<StationFromRouteModel, StationFromRouteResponse>().ReverseMap();
            CreateMap<CreateServiceModel, CreateServiceRequest>().ReverseMap();
            CreateMap<UpdateServiceModel, UpdateServiceRequest>().ReverseMap();           
            ////Feedback////
            CreateMap<TripFeedbackModel, FeedbackInTripResponse>().ReverseMap();
            ////Station////
            CreateMap<GetStationModel,Station>().ReverseMap();
            CreateMap<GetStationModel,GetStationResponse>().ReverseMap();   
            CreateMap<CreateStationModel,CreateStationRequest>().ReverseMap();
            CreateMap<CreateStationWithServiceModel,CreateStationWithServiceRequest>().ReverseMap();
            /////Company/////
            CreateMap<GetCompanyModel, Company>().ReverseMap();
            CreateMap<GetCompanyModel, GetCompanyResponse>().ReverseMap();
            CreateMap<CreateCompanyModel, CreateCompanyRequest>().ReverseMap();
            /////City/////
            CreateMap<CreateCityModel, CreateCityRequest>().ReverseMap();
            CreateMap<CitiesModel, CitiesResponse>().ReverseMap();
            CreateMap<CitiesModel, City>().ReverseMap();
            /////TicketDetail/////
            CreateMap<GetTicketDetailByUserModel, GetTicketDetailByUserResponse>().ReverseMap();
            CreateMap<ServiceDetail, ServiceDetailModel>().ReverseMap();
            CreateMap<GetDetailOfTicketByIDModel, GetDetailOfTicketByIDResponse>()
                .ForMember(_ => _.ServiceDetailList, opt => opt.MapFrom(_ => _.ServiceDetailList)).ReverseMap();
            CreateMap<SearchTicketModel, SearchTicketResponse>().ReverseMap();
            ///////Station_Service///////
            CreateMap<CreateServiceInStationModel, CreateServiceInStationRequest>().ReverseMap();
            CreateMap<UpdateServiceInStationModel, UpdateServiceInStationRequest>().ReverseMap();
            //////Booking//////
            CreateMap<AddOrUpdateBookingModel, SWD.TicketBooking.Repo.Entities.Booking>().ReverseMap();
            CreateMap<AddOrUpdateTicketModel, TicketDetail>().ReverseMap();
            CreateMap<AddOrUpdateServiceModel, TicketDetail_Service>().ReverseMap();
            CreateMap<AddOrUpdateServiceModel, AddOrUpdateServiceRequest>().ReverseMap();
            CreateMap<AddOrUpdateTicketModel, AddOrUpdateTicketRequest>().ReverseMap();
            CreateMap<AddOrUpdateBookingModel, AddOrUpdateBookingRequest>().ReverseMap();
            CreateMap<BookingModel, BookingRequest>().ReverseMap();
            CreateMap(typeof(PagedResult<>), typeof(PagedResultResponse<>));


            /////TicketType/////
            CreateMap<TicketTypeResponse,TicketType>().ReverseMap();
        }
    }
}

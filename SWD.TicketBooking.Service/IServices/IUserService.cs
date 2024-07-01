using Microsoft.AspNetCore.Http;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.User;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel> GetUserByEmailForOTP(string email);
        Task<UserModel> GetUserByEmail(string email);
        Task<User> GetUserByAccessToken(string accessToken);
        Task<User> GetUserByEmail2(string email);
        Task<(CreateUserReq returnModel, string message)> SendOTPCode(CreateUserReq req);
        Task<ActionOutcome> SubmitOTP(SubmitOTPReq req);
        Task<UserDetailModel> GetUserById(Guid id);
        Task<UpdateUserResponseModel /*returnModel, string message*/> UpdateUser(Guid id, UpdateUserModel updateUser);
        Task<bool> UploadAvatar(IFormFile file);
        Task<List<GetStaffFromCompanyModel>> GetStaffFromCompany(Guid companyID);
        Task<Guid> GetCompanyIDByUser (Guid userId);
    }
}

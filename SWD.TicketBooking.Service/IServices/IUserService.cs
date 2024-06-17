using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.BackendService;
using SWD.TicketBooking.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel> GetUserByEmailForOTP(string email);
        Task<UserModel> GetUserByEmail(string email);
        Task<(CreateUserReq returnModel, string message)> SendOTPCode(CreateUserReq req);
        Task<(UserModel returnModel, string message)> SubmitOTP(SubmitOTPReq req);
        Task<UserModel> GetUserById(Guid id);
        Task<(UpdateUserModel returnModel, string message)> UpdateUser(Guid id, UpdateUserModel updateUser);
    }
}

using AutoMapper;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.IRepositories;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.SeedData;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.User;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Transactions;
using static QRCoder.PayloadGenerator;
using static System.Net.WebRequestMethods;

namespace SWD.TicketBooking.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public readonly IFirebaseService _firebaseService;
        private readonly IEmailService _emailService;

        public static int Page_Size { get; set; } = 10;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseService firebaseService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseService = firebaseService;
            _emailService = emailService;
        }
        public async Task<Repo.Entities.User> GetUserByAccessToken(string accessToken)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(u => u.AccessToken == accessToken).FirstOrDefaultAsync();
            if (user != null && user.TokenExpiration > DateTime.UtcNow)
            {
                return user;
            }

            return null;
        }
        public async Task<List<UserDetailModel>> GetAllUsers()
        {
            try
            {
                var user = await _unitOfWork.UserRepository
                                            .GetAll()
                                            .Include(u => u.UserRole)
                                            .ToListAsync();
                var rs = _mapper.Map<List<UserDetailModel>>(user);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<UserModel> GetUserByEmailForOTP(string email)
        {
            try
            {
                if (!FunctionCommon.IsValidEmail(email))
                {
                    throw new BadRequestException("EMAIL KHÔNG HỢP LỆ!");
                }
                var userEntity = await _unitOfWork.UserRepository
                                                  .FindByCondition(x => x.Email == email)
                                                  .FirstOrDefaultAsync();
                var result = _mapper.Map<UserModel>(userEntity);

                if (result == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("NGƯỜI DÙNG"));
                }

                if (result.OTPCode == "0" && result.IsVerified == true)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("NGƯỜI DÙNG", "KHI TẠO LẤY MÃ OTP"));
                }

                if (result.IsVerified == false)
                {
                    return result;
                }

                throw new InternalServerErrorException("ĐÃ CÓ LỖI XẢY RA!");
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<UserModel> GetUserByEmail(string email)
        {
            try
            {
                if (!FunctionCommon.IsValidEmail(email))
                {
                    throw new BadRequestException("EMAIL KHÔNG HỢP LỆ!");
                }

                var userEntity = await _unitOfWork.UserRepository
                                                  .FindByCondition(x => x.Email == email)
                                                  .Include(_ => _.UserRole)
                                                  .FirstOrDefaultAsync();

                if (userEntity == null)
                {
                    throw new NotFoundException("KHÔNG TÌM THẤY NGƯỜI DÙNG VỚI EMAIL NÀY!");
                }

                var getBalance = await _unitOfWork.TransactionRepository
                                                  .FindByCondition(_ => _.UserID == userEntity.UserID)
                                                  .OrderByDescending(_ => _.TransactionDate)
                                                  .Select(_ => (double?)_.BalanceAfterTransaction)
                                                  .FirstOrDefaultAsync() ?? 0.0;

                var userModel = new UserModel
                {
                    UserID = userEntity.UserID,
                    UserName = userEntity.UserName ,
                    Password = userEntity.Password ,
                    FullName = userEntity.FullName ,
                    Email = userEntity.Email ,
                    Avatar = userEntity.Avatar ,
                    Address = userEntity.Address ,
                    OTPCode = userEntity.OTPCode ,
                    PhoneNumber = userEntity.PhoneNumber ,
                    Balance = getBalance,
                    CreateDate = userEntity.CreateDate,
                    IsVerified = userEntity.IsVerified,
                    Status = userEntity.Status,
                    CompanyID = userEntity.CompanyID ,
                    RoleID = userEntity.UserRole?.RoleID 
                };

                return userModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }





        public async Task<(CreateUserReq returnModel, string message)> SendOTPCode(CreateUserReq req)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!FunctionCommon.IsValidEmail(req.Email))
                    {
                        throw new BadRequestException("EMAIL KHÔNG HỢP LỆ!");
                    }
                    var user = _unitOfWork.UserRepository.FindByCondition(x => x.Email == req.Email).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.IsVerified == false)
                        {
                            user.CreateDate = DateTimeOffset.Now;
                            user.OTPCode = req.OTPCode;
                            _unitOfWork.UserRepository.Update(user);
                            int rs = await _unitOfWork.UserRepository.Commit();

                            if (rs > 0)
                            {
                                scope.Complete();
                                return (_mapper.Map<CreateUserReq>(user), "GỬI MÃ OTP THÀNH CÔNG!");
                            }
                            else
                            {
                                throw new BadRequestException("GỬI MÃ OTP THẤT BẠI!");
                            }
                        }
                        else
                        {
                            throw new BadRequestException(SD.Notification.Existed("NGƯỜI DÙNG", "EMAIL"));
                        }
                    }
                    var userEntity = _mapper.Map<Repo.Entities.User>(req);
                    _unitOfWork.UserRepository.AddAsync(userEntity);
                    int commitResult = await _unitOfWork.UserRepository.Commit();

                    if (commitResult > 0)
                    {
                        scope.Complete();
                        return (_mapper.Map<CreateUserReq>(userEntity), "GỬI MÃ OTP THÀNH CÔNG!");
                    }
                    else
                    {
                        throw new BadRequestException("GỬI MÃ OTP THẤT BẠI!");
                    }
                }
                catch (BadRequestException ex)
                {
                    throw ex; 
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
        public async Task<ActionOutcome> SubmitOTP(SubmitOTPReq req)
        {
            try
            {
                if (!FunctionCommon.IsValidEmail(req.Email))
                {
                    throw new BadRequestException("EMAIL KHÔNG HỢP LỆ!");
                }
                var rs = new ActionOutcome();
                var user = await _unitOfWork.UserRepository.FindByCondition(u => u.Email.Equals(req.Email)).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new NotFoundException("KHÔNG TÌM THẤY EMAIL!");
                }
                if (!user.OTPCode.Equals(req.OTPCode))
                {
                    throw new BadRequestException("MÃ OTP KHÔNG CHÍNH XÁC!");
                }
                user.OTPCode = "0";
                user.IsVerified = true;
                user.Status = SD.GeneralStatus.ACTIVE;
                _unitOfWork.UserRepository.Update(user);
                int result = _unitOfWork.Complete();
                rs.Result = _mapper.Map<UserModel>(user);
                rs.Message = "XÁC MINH OTP THÀNH CÔNG!";
                if (result > 0)
                {
                    return rs;
                }
                else
                {
                    throw new InternalServerErrorException("LỖI XẢY RA VỚI CƠ SỞ DỮ LIỆU!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<UserDetailModel> GetUserById(Guid id)
        {
            try
            {
                var userEntity = await _unitOfWork.UserRepository
                                            .GetAll()
                                            .Where(u => u.UserID.Equals(id))
                                            .Include(u => u.UserRole)
                                            .FirstOrDefaultAsync();
                if (userEntity == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("NGƯỜI DÙNG"));
                }
                var getBalance = await _unitOfWork.TransactionRepository
                                                   .FindByCondition(_ => _.UserID == id)
                                                   .OrderByDescending(_ => _.TransactionDate)
                                                   .Select(_ => (double?)_.BalanceAfterTransaction)
                                                   .FirstOrDefaultAsync() ?? 0.0;

                var userModel = new UserDetailModel
                {
                    UserID = userEntity.UserID,
                    UserName = userEntity.UserName,
                    Password = userEntity.Password,
                    FullName = userEntity.FullName,
                    Email = userEntity.Email,
                    Avatar = userEntity.Avatar,
                    Address = userEntity.Address,
                    OTPCode = userEntity.OTPCode,
                    PhoneNumber = userEntity.PhoneNumber,
                    Balance = getBalance,
                    CreateDate = userEntity.CreateDate,
                    IsVerified = userEntity.IsVerified,
                    Status = userEntity.Status,
                    CompanyID = userEntity.CompanyID,
                    RoleID = userEntity.RoleID,
                    RoleName = userEntity.UserRole?.RoleName

                };
                return userModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<GetStaffFromCompanyModel>> GetStaffFromCompany(Guid companyID)
        {
            try
            {
                var rs = await _unitOfWork.UserRepository.GetAll().Where(_ => _.CompanyID == companyID).Select(_ => new GetStaffFromCompanyModel
                {
                    StaffID = _.UserID,
                    Name = _.FullName
                }).ToListAsync();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UpdateUserResponseModel> UpdateUser(Guid id, UpdateUserModel updateUser)
        {
            try
            {
                var existedUser = await _unitOfWork.UserRepository.FindByCondition(x => x.UserID == id).FirstOrDefaultAsync();
                if (existedUser != null)
                {
                    if (!updateUser.Password.IsNullOrEmpty() && !SecurityUtil.Hash(updateUser.Password).Equals(existedUser.Password))
                    {
                        throw new BadRequestException("MẬT KHẨU CŨ KHÔNG ĐÚNG!");
                    }
                    if (!updateUser.Password.IsNullOrEmpty() && updateUser.NewPassword != null && updateUser.ConfirmPassword.Equals(updateUser.NewPassword))
                    {
                        existedUser.Password = SecurityUtil.Hash(updateUser.NewPassword);                       
                    }
                    else if (!updateUser.Password.IsNullOrEmpty() && updateUser.NewPassword != null && !updateUser.ConfirmPassword.Equals(updateUser.NewPassword))
                    {
                        throw new BadRequestException("MẬT KHẨU XÁC NHẬN KHÔNG ĐÚNG!");
                    }

                    if (!updateUser.UserName.IsNullOrEmpty())
                    {
                        existedUser.UserName = updateUser.UserName;
                    }
                    if (!updateUser.FullName.IsNullOrEmpty())
                    {
                        existedUser.FullName = updateUser.FullName;
                    }
                    if (!updateUser.Address.IsNullOrEmpty())
                    {
                        existedUser.Address = updateUser.Address;
                    }
                    if (!updateUser.PhoneNumber.IsNullOrEmpty() && FunctionCommon.IsValidPhoneNumber(updateUser.PhoneNumber))
                    {
                        existedUser.PhoneNumber = updateUser.PhoneNumber;
                    }
                    if (!updateUser.UserName.IsNullOrEmpty())
                    {
                        existedUser.UserName = updateUser.UserName;
                    }
                    if (updateUser.Avatar != null && updateUser.Avatar.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(existedUser.Avatar))
                        {
                            string url = $"{FirebasePathName.AVATAR}{existedUser.UserID}";
                            var deleteResult = await _firebaseService.DeleteFileFromFirebase(url);
                            if (!deleteResult.IsSuccess)
                            {
                                throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHI XÓA ẢNH"));
                            }
                        }
                        var imagePath = $"{FirebasePathName.AVATAR}{existedUser.UserID}";
                        var imageUploadResult = await _firebaseService.UploadFileToFirebase(updateUser.Avatar, imagePath);

                        if (imageUploadResult.IsSuccess)
                        {
                            existedUser.Avatar = (string)imageUploadResult.Result;
                        }
                        else
                        {
                            throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHI TẢI ẢNH LÊN"));
                        }
                    }

                    var updatedUser = _unitOfWork.UserRepository.Update(existedUser);
                    var update = _mapper.Map<UpdateUserResponseModel>(updatedUser);
                    _unitOfWork.Complete();
                    return (update/*, "OK"*/);
                }
                else
                {
                    throw new BadRequestException(SD.Notification.NotFound("NGƯỜI DÙNG"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> UploadAvatar(IFormFile file)
        {
            try
            {
                var imagePath = FirebasePathName.AVATAR_DEFAULT + $"{Guid.NewGuid().ToString()}";
                var imageUploadResult = await _firebaseService.UploadFileToFirebase(file, imagePath);
                if (!imageUploadResult.IsSuccess)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHI TẢI ẢNH LÊN"));
                }
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Repo.Entities.User> GetUserByEmailToLoginGG(string email)
        {
            try
            {
                var userEntity = await _unitOfWork.UserRepository.FindByCondition(x => x.Email == email).FirstOrDefaultAsync();
                return userEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }        
        
        public async Task<Guid> GetCompanyIDByUser(Guid userId)
        {
            try
            {
                var userEntity = await _unitOfWork.UserRepository
                                                  .FindByCondition(x => x.UserID == userId)
                                                  .Select(_ => _.CompanyID)
                                                  .FirstOrDefaultAsync();
                return (Guid)userEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<UserModel>> GetAllUsersByRole(string roleName)
        {
            try
            {
                var user = await _unitOfWork.UserRepository
                                                  .GetAll()
                                                  //.Include(_ => _.UserRole)
                                                  .Where(_ => _.UserRole.RoleName.ToUpper().Equals(roleName.ToUpper()))
                                                  .ToListAsync();

                var rs = _mapper.Map<List<UserModel>>(user);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

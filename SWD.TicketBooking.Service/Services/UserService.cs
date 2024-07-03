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

        public static int Page_Size { get; set; } = 10;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseService firebaseService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseService = firebaseService;
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
        public async Task<List<UserModel>> GetAllUsers()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetAll().ToListAsync();
                var rs = _mapper.Map<List<UserModel>>(users);
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
                var userEntity = _unitOfWork.UserRepository.FindByCondition(x => x.Email == email).Include(_ => _.UserRole).FirstOrDefault();
                var userModel = _mapper.Map<UserModel>(userEntity);
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
                var user = await _unitOfWork.UserRepository
                                            .GetAll()
                                            .Where(u => u.UserID.Equals(id))
                                            .Include(u => u.UserRole)
                                            .FirstOrDefaultAsync();
                var us = _mapper.Map<UserDetailModel>(user);
                return us;
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
        public async Task<UpdateUserResponseModel /*returnModel/*, string message*/> UpdateUser(Guid id, UpdateUserModel updateUser)
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

                    existedUser.UserName = updateUser.UserName;
                    //existedUser.Password = SecurityUtil.Hash(updateUser.NewPassword);
                    existedUser.FullName = updateUser.FullName;
                    existedUser.Address = updateUser.Address;
                    existedUser.PhoneNumber = updateUser.PhoneNumber;

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

        public async Task<Repo.Entities.User> GetUserByEmail2(string email)
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
    }
}

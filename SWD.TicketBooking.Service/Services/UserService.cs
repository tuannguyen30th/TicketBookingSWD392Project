using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.User;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using System.Transactions;

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
        public async Task<List<UserModel>> GetAllUsers()
        {
            try
            {
                var users = _unitOfWork.UserRepository.GetAll();
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
                    throw new InternalServerErrorException(SD.Notification.Internal("NGƯỜI DÙNG", "ĐÃ CÓ LỖI XẢY RA TẠO LẤY MÃ OTP"));
                }

                if (result.IsVerified == false)
                {
                    return result;
                }

                throw new InternalServerErrorException(SD.Notification.Internal("NGƯỜI DÙNG", "ĐÃ CÓ LỖI XẢY RA"));
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
                var userEntity = _unitOfWork.UserRepository.FindByCondition(x => x.Email == email).FirstOrDefault();
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
                    var user = _unitOfWork.UserRepository.FindByCondition(x => x.Email == req.Email).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.IsVerified == false)
                        {
                            // User exists but not verified, update OTP code
                            user.CreateDate = DateTimeOffset.Now;
                            user.OTPCode = req.OTPCode;
                            _unitOfWork.UserRepository.Update(user);
                            int rs = await _unitOfWork.UserRepository.Commit();

                            if (rs > 0)
                            {
                                scope.Complete();
                                return (_mapper.Map<CreateUserReq>(user), "Send OTP successfully");
                            }
                            else
                            {
                                return (null, "GỬI MÃ OTP THẤT BẠI!");
                            }
                        }
                        else
                        {
                            // User is already verified
                            throw new BadRequestException(SD.Notification.Existed("NGƯỜI DÙNG", "EMAIL"));
                        }
                    }

                    // If user does not exist, create a new user and send OTP
                    var userEntity = _mapper.Map<User>(req);
                    _unitOfWork.UserRepository.AddAsync(userEntity);
                    int commitResult = await _unitOfWork.UserRepository.Commit();

                    if (commitResult > 0)
                    {
                        scope.Complete();
                        return (_mapper.Map<CreateUserReq>(userEntity), "GỬI OTP THÀNH CÔNG!");
                    }
                    else
                    {
                        return (null, "GỬI MÃ OTP THẤT BẠI!");
                    }
                }
                catch (BadRequestException ex)
                {
                    throw ex; 
                }
                catch (Exception ex)
                {
                    throw new Exception("LỖI XẢI RA KHI GỬI MÃ OTP", ex);
                }
            }
        }
        public async Task<(UserModel returnModel, string message)> SubmitOTP(SubmitOTPReq req)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.FindByCondition(u => u.Email.Equals(req.Email)).FirstOrDefaultAsync();

                if (user == null)
                {
                    return (null, "KHÔNG THỂ TÌM THẤY EMAIL!");
                }
                if (!user.OTPCode.Equals(req.OTPCode))
                {
                    return (null, "MÃ OTP KHÔNG CHÍNH XÁC!");
                }
                user.OTPCode = "0";
                user.IsVerified = true;
                user.Status = "Active";
                _unitOfWork.UserRepository.Update(user);
                int result = _unitOfWork.Complete();

                if (result > 0)
                {
                    return (_mapper.Map<UserModel>(user), "XÁC MINH OTP THÀNH CÔNG!");
                }
                else
                {
                    return (null, "LỖI XẢY RA VỚI CƠ SỞ DỮ LIỆU!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<UserModel> GetUserById(Guid id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                var us = _mapper.Map<UserModel>(user);
                return us;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<(UpdateUserModel returnModel, string message)> UpdateUser(Guid id, UpdateUserModel updateUser)
        {
            try
            {
                var existedUser = await _unitOfWork.UserRepository.FindByCondition(x => x.UserID == id).FirstOrDefaultAsync();
                if (existedUser != null)
                {

                    existedUser.UserName = updateUser.UserName;
                    existedUser.Password = updateUser.Password;
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
                                throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHÔNG THỂ XÓA HÌNH ẢNH"));
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
                            throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHÔNG THỂ TẢI LÊN HÌNH ẢNH"));
                        }

                        _unitOfWork.UserRepository.Update(existedUser);
                        //await _unitOfWork.UserRepository.Commit();
                        _unitOfWork.Complete();
                        var update = _mapper.Map<UpdateUserModel>(existedUser);
                        return (update, "OK");
                    }
                    else
                    {
                        throw new BadRequestException("SAI MẬT KHẨU!");
                    }
                }
                else
                {
                    throw new BadRequestException("KHÔNG THỂ TÌM THẤY NGƯỜI DÙNG");
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
                    throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "LỖI KHI TẢI ẢNH LÊN FIREBASE"));
                }
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

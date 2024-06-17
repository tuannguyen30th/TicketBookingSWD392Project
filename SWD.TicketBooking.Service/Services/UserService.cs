﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.User;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using System.Transactions;

namespace SWD.TicketBooking.Service.Services
{
    public class UserService : IUserService
    {

        private readonly IRepository<User, Guid> _userRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<UserRole, Guid> _userRoleRepository;
        public readonly IFirebaseService _firebaseService;

        public static int Page_Size { get; set; } = 10;

        public UserService(IRepository<User, Guid> userRepository, IMapper mapper, IRepository<UserRole, Guid> userRoleRepository, IFirebaseService firebaseService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _firebaseService = firebaseService;
        }
        public async Task<List<UserModel>> GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetAll();
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

                var userEntity = await _userRepository.FindByCondition(x => x.Email == email).FirstOrDefaultAsync();
                var result = _mapper.Map<UserModel>(userEntity);

                if (result == null)
                {
                    throw new NotFoundException("Cannot find user");
                }

                if (result.OTPCode == "0" && result.IsVerified == true)
                {
                    throw new InternalServerErrorException("Some error occur");
                }

                if (result.IsVerified == false)
                {
                    return result;
                }

                throw new InternalServerErrorException("Some error occur");
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
                var userEntity = _userRepository.FindByCondition(x => x.Email == email).FirstOrDefault();
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
                    var user = _userRepository.FindByCondition(x => x.Email == req.Email).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.IsVerified == false)
                        {
                            // User exists but not verified, update OTP code
                            user.CreateDate = DateTimeOffset.Now;
                            user.OTPCode = req.OTPCode;
                            _userRepository.Update(user);
                            int rs = await _userRepository.Commit();

                            if (rs > 0)
                            {
                                scope.Complete();
                                return (_mapper.Map<CreateUserReq>(user), "Send OTP successfully");
                            }
                            else
                            {
                                return (null, "Failed to send OTP");
                            }
                        }
                        else
                        {
                            // User is already verified
                            throw new BadRequestException("Email is already existed!");
                        }
                    }

                    // If user does not exist, create a new user and send OTP
                    var userEntity = _mapper.Map<User>(req);
                    _userRepository.AddAsync(userEntity);
                    int commitResult = await _userRepository.Commit();

                    if (commitResult > 0)
                    {
                        scope.Complete();
                        return (_mapper.Map<CreateUserReq>(userEntity), "Send OTP successfully");
                    }
                    else
                    {
                        return (null, "Failed to send OTP");
                    }
                }
                catch (BadRequestException ex)
                {
                    throw ex; // Re-throw custom exceptions without wrapping
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while sending OTP.", ex);
                }
            }
        }



        public async Task<(UserModel returnModel, string message)> SubmitOTP(SubmitOTPReq req)
        {
            try
            {
                var user = await _userRepository.FindByCondition(u => u.Email.Equals(req.Email)).FirstOrDefaultAsync();

                if (user == null)
                {
                    return (null, "Email not found!");
                }
                if (!user.OTPCode.Equals(req.OTPCode))
                {
                    return (null, "OTP is not correct");
                }
                user.OTPCode = "0";
                user.IsVerified = true;
                user.Status = "Active";
                _userRepository.Update(user);
                int result = await _userRepository.Commit();

                if (result > 0)
                {
                    return (_mapper.Map<UserModel>(user), "OTP verified successfully");
                }
                else
                {
                    return (null, "Failed to commit changes");
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
                var user = await _userRepository.GetByIdAsync(id);
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
                var existedUser = await _userRepository.FindByCondition(x => x.UserID == id).FirstOrDefaultAsync();
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
                                throw new InternalServerErrorException($"Failed to delete old image");
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
                            throw new InternalServerErrorException($"Failed to upload new image:");
                        }

                        _userRepository.Update(existedUser);
                        _userRepository.Commit();
                        var update = _mapper.Map<UpdateUserModel>(existedUser);
                        return (update, "OK");
                    }
                    else
                    {
                        throw new BadRequestException("Password is not true!");
                    }
                }
                else
                {
                    throw new BadRequestException("User not found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
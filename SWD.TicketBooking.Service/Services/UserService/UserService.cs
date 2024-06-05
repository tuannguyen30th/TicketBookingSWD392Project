﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Exceptions;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Service.Dtos.User;
using System.Net;


namespace SWD.TicketBooking.Service.Services.UserService
{
    public class UserService
    {
       
        private readonly IRepository<User, int> _userRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<UserRole, int> _userRoleRepository;
        public static int Page_Size { get; set; } = 10;

        public UserService(IRepository<User, int> userRepository, IMapper mapper, IRepository<UserRole, int> userRoleRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
         
        }
        public async Task<UserModel> GetUserByEmailForOTP(string email)
        {
            try
            {
              
                    var userEntity = await _userRepository.FindByCondition(x => x.Email == email).FirstOrDefaultAsync();
                    var result = _mapper.Map<UserModel>(userEntity);

                    if (result == null)
                    {
                        throw new Exception();
                    }

                    if (result.OTPCode == "0" && result.IsVerified == true)
                    {
                        throw new Exception();
                    }

                    if (result.IsVerified == false)
                    {
                        return result;
                    }

                    throw new Exception();
            }
            
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<(CreateUserReq returnModel, string message)> SendOTPCode(CreateUserReq req)
        {
            try
            {
                var userEntity = _mapper.Map<User>(req);
                var user = _userRepository.FindByCondition(x => x.Email == req.Email).FirstOrDefault();

                if (user != null && user.IsVerified == false)
                {
                    user.CreateDate = DateTimeOffset.Now;
                    user.OTPCode = req.OTPCode;
                    user = _userRepository.Update(user);
                    int rs = await _userRepository.Commit();
                    if (rs > 0)
                    {
                        return (_mapper.Map<CreateUserReq>(user), "Send OTP successfully");
                    }
                    else
                    {
                        return (null, "Fail");
                    }
                }
                var existedUser = _userRepository.FindByCondition(x => x.Email == req.Email && x.IsVerified == true).FirstOrDefault();
                if (existedUser != null)
                {
                    throw new BadRequestException("Email is already existed!");
                }
                return (null, "Fail");
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw;
            }
        }

    }
}

﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility = SWD.TicketBooking.Repo.Entities.Utility;

namespace SWD.TicketBooking.Service.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UtilityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
       /* public async Task<List<UtilityModel>> GetAllUtilityByTripID(Guid id)
        {
            var utilities = await _unitOfWork.Trip_UtilityRepository
                .FindByCondition(tu => tu.TripID == id && tu.Status.Trim().Equals(SD.ACTIVE))
                .Select(tu => tu.Utility)
                .ToListAsync();
            var result = new List<UtilityModel>();
            foreach (var trip in utilities)
            {
                var newModel = new UtilityModel
                {
                    Name = trip.Name,
                    Description = trip.Description,
                    Status = trip.Status,
                };
                result.Add(newModel);
            }
            return result;
        }*/
    }
}

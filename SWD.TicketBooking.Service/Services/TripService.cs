﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Exceptions;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class TripService
    {
        private readonly IRepository<Trip, int> _tripRepo;
        private readonly IRepository<Booking, int> _bookingRepo;
        private readonly IRepository<TicketType_Trip, int> _ticketTypeTripRepo;
        private readonly IMapper _mapper;

        public TripService(IRepository<Trip, int> tripRepo, IRepository<Booking, int> bookingRepo, IRepository<TicketType_Trip, int> ticketTypeTripRepo, IMapper mapper)
        {
            _tripRepo = tripRepo;
            _bookingRepo = bookingRepo;
            _ticketTypeTripRepo = ticketTypeTripRepo;
            _mapper = mapper;
        }

        public async Task<PictureModel> GetPictureOfTrip(int id)
        {
            try
            {
                var trip = await _tripRepo.GetByIdAsync(id);
                if (trip == null)
                {
                    throw new BadRequestException("Trip not found!");
                }
                else
                {
                    var result =_mapper.Map<PictureModel>(trip);
                    return result;
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<PopularTripModel>> GetPopularTrips()
        {
            try
            {
                var topTrips = _bookingRepo.GetAll()
                                            .GroupBy(b => b.TripID)
                                            .Select(g => new
                                            {
                                                TripID = g.Key,
                                                TotalQuantity = g.Sum(b => b.Quantity),
                                            })
                                            .OrderByDescending(t => t.TotalQuantity)
                                            .Take(5)
                                            .ToList();



                var trips = await _tripRepo.GetAll()
                                                .Include(t => t.Route.FromCity)
                                                .Include(t => t.Route.ToCity)
                                                .Where(t => t.Status.ToLower().Trim() == "active" && topTrips.Select(_ => _.TripID).Contains(t.TripID))
                                                .ToListAsync();

                var rs = new List<PopularTripModel>();

                foreach (var t in trips)
                {
                    var minPriceByTrip = await _ticketTypeTripRepo.GetAll()
                     .Where(ttt => ttt.TripID == t.TripID)
                     .GroupBy(ttt => ttt.TripID)
                     .Select(g => new
                     {
                         TripId = g.Key,
                         MinPrice = g.Min(ttt => ttt.Price)
                     })
                     .ToDictionaryAsync(x => x.TripId, x => x.MinPrice);

                    var popuTrip = new PopularTripModel
                    {
                        TripId = t.TripID,
                        FromCity = t.Route.FromCity.Name,
                        ToCity = t.Route.ToCity.Name,
                        ImageUrl = t.ImageUrl,
                        PriceFrom = minPriceByTrip.GetValueOrDefault(t.TripID, 0),
                    };

                    rs.Add(popuTrip);
                }

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}

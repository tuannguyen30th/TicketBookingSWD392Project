using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DashboardInfoByCompanyModel> GetAllDashboardInfoByCompany(Guid companyID)
        {
            try
            {
                var allRoutes = await _unitOfWork.Route_CompanyRepository
                                 .GetAll()
                                 .Where(_ => _.Status.Equals(SD.GeneralStatus.ACTIVE) && _.CompanyID.Equals(companyID))
                                 .Select(_ => _.Route_CompanyID)
                                 .ToListAsync();

                var allTrips = await _unitOfWork.TripRepository
                                .GetAll()
                                .Where(_ => allRoutes.Contains((Guid)_.Route_CompanyID))
                                .Select(_ => _.TripID)
                                .ToListAsync();

                var currentDate = DateTime.Now;
                var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var allTickets = await _unitOfWork.BookingRepository
                                                     .GetAll()
                                                     .Where(_ => _.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING) &&
                                                                 _.BookingTime >= startOfMonth &&
                                                                 _.BookingTime <= endOfMonth &&
                                                                 allTrips.Contains((Guid)_.TripID))
                                                     .ToListAsync();

                var monthlyRevenue = allTickets.Sum(_ => _.TotalBill);

                var startOfYear = new DateTime(currentDate.Year, 1, 1);
                var endOfYear = new DateTime(currentDate.Year, 12, 31);

                var yearlyRevenue = _unitOfWork.BookingRepository
                                                    .GetAll()
                                                    .Where(_ => _.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING) &&
                                                               _.BookingTime >= startOfYear &&
                                                               _.BookingTime <= endOfYear &&
                                                               allTrips.Contains((Guid)_.TripID))
                                                    .Sum(s => s.TotalBill);

                var test = await _unitOfWork.BookingRepository
                                            .GetAll()
                                            .GroupBy(b => b.TripID)
                                            .Select(g => new
                                            {
                                                TripID = g.Key,
                                                TotalBooking = g.Count()
                                            })
                                            .ToListAsync();

                var routeIDs = await _unitOfWork.TripRepository
                                                .GetAll()
                                                .Where(t => allRoutes.Contains((Guid)t.Route_CompanyID) && test.Select(tt => tt.TripID).Contains(t.TripID))
                                                .Include(t => t.Route_Company.Route)
                                                .Select(t => t.Route_Company.RouteID)
                                                .Distinct()
                                                .ToListAsync();

                var trips = _unitOfWork.TripRepository
                                          .GetAll()
                                          .Include(tr => tr.Route_Company.Route)
                                          .Where(_ => allRoutes.Contains((Guid)_.Route_CompanyID))
                                          .ToList();

                var topRoute = trips
                                .GroupBy(tr => tr.Route_Company.RouteID)
                                .Select(g => new
                                {
                                    RouteID = g.Key,
                                    TotalBookings = g.Sum(tr => test.Where(t => t.TripID == tr.TripID).Sum(t => t.TotalBooking))
                                })
                                .OrderByDescending(r => r.TotalBookings)
                                .FirstOrDefault();

                var getRoute = await _unitOfWork.RouteRepository
                                .GetAll()
                                .Include(_ => _.FromCity)
                                .Include(_ => _.ToCity)
                                .Where(_ => _.RouteID.Equals((Guid)topRoute.RouteID))
                                .FirstOrDefaultAsync();

                var rs = new DashboardInfoByCompanyModel
                {
                    MostPopularRouteID = (Guid)getRoute.RouteID,
                    MostPopularRoute_FromCity = getRoute.FromCity.Name,
                    MostPopularRoute_ToCity = getRoute.ToCity.Name,
                    TotalBookingsInPopularRoute = topRoute.TotalBookings,
                    TotalRoutes = allRoutes.Count(),
                    TotalTrips = allTrips.Count(),
                    TotalBookingsInMonth = (int)allTickets.Sum(_ => _.Quantity),
                    MonthlyRevenue = (double)monthlyRevenue,
                    YearlyRevenue = (double)yearlyRevenue
                };

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

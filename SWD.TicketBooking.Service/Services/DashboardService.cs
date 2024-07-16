using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
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
                                .Include(_ => _.Route_Company)
                                .Where(_ => allRoutes.Contains((Guid)_.Route_CompanyID) && _.IsTemplate == false)
                                .ToListAsync();

                var currentDate = DateTime.Now;
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                var allTicketDetails = await _unitOfWork.TicketDetailRepository
                                                        .GetAll()
                                                        .Where(_ => _.Status.Equals(SD.Booking_TicketStatus.USED_TICKET))
                                                        .Select(_ => _.BookingID)
                                                        .Distinct()
                                                        .ToListAsync();

                var allTickets = await _unitOfWork.BookingRepository
                                                     .GetAll()
                                                     .Where(_ => _.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING) &&
                                                                 _.BookingTime.Value.Month == currentMonth &&
                                                                 _.BookingTime.Value.Year == currentYear &&
                                                                 allTicketDetails.Contains(_.BookingID) &&
                                                                 allTrips.Select(_ => _.TripID).Contains((Guid)_.TripID))
                                                     .ToListAsync();

                var monthlyRevenue = new List<MonthlyRevenueModel>();

                for (int month = 0; month < 12; month++)
                {
                    var startOfMonth = new DateTime(currentDate.Year, month + 1, 1);
                    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    var allTicket = await _unitOfWork.BookingRepository
                                                     .GetAll()
                                                     .Where(_ => _.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING) &&
                                                                 _.BookingTime >= startOfMonth &&
                                                                 _.BookingTime <= endOfMonth &&
                                                                 allTrips.Select(_ => _.TripID).Contains((Guid)_.TripID))
                                                     .ToListAsync();

                    var revenue = allTicket.Sum(_ => _.TotalBill);
                    var result = new MonthlyRevenueModel
                    {
                        Month = startOfMonth.Month,
                        RevenueInMonth = (double)revenue
                    };
                    monthlyRevenue.Add(result);
                }

                var startOfYear = new DateTime(currentDate.Year, 1, 1);
                var endOfYear = new DateTime(currentDate.Year, 12, 31);

                var yearlyRevenue = _unitOfWork.BookingRepository
                                                    .GetAll()
                                                    .Where(_ => _.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING) &&
                                                               _.BookingTime >= startOfYear &&
                                                               _.BookingTime <= endOfYear &&
                                                               allTrips.Select(_ => _.TripID).Contains((Guid)_.TripID))
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
                    .Where(t => test.Select(tt => tt.TripID).Contains(t.TripID))
                    .Include(t => t.Route_Company.Route)
                    .Select(t => t.Route_Company.RouteID)
                    .Distinct()
                    .ToListAsync();

                var topRoutes = allTrips
                    .GroupBy(tr => tr.Route_Company.RouteID)
                    .Select(g => new
                    {
                        RouteID = g.Key,
                        TotalBookings = g.Sum(tr => test.Where(t => t.TripID == tr.TripID).Sum(t => t.TotalBooking))
                    })
                    .OrderByDescending(r => r.TotalBookings)
                    .Take(5)
                    .ToList();


                var popularRoutes = new List<PopularRouteModel>();

                foreach (var route in topRoutes)
                {
                    var getRoute = await _unitOfWork.RouteRepository
                                                    .GetAll()
                                                    .Include(_ => _.FromCity)
                                                    .Include(_ => _.ToCity)
                                                    .Where(_ => _.RouteID.Equals((Guid)route.RouteID))
                                                    .FirstOrDefaultAsync();

                    var routeRs = new PopularRouteModel
                    {
                        RouteID = getRoute.RouteID,
                        FromCityID = getRoute.FromCity.CityID,
                        FromCity = getRoute.FromCity.Name,
                        ToCityID = getRoute.ToCity.CityID,
                        ToCity = getRoute.ToCity.Name,
                        StartLocation = getRoute.StartLocation,
                        EndLocation = getRoute.EndLocation,
                        TotalBooking = route.TotalBookings
                    };

                    popularRoutes.Add(routeRs);
                }

                var rs = new DashboardInfoByCompanyModel
                {
                    PopularRoutes = popularRoutes,
                    TotalRoutes = allRoutes.Count(),
                    TotalTrips = allTrips.Count(),
                    TotalBookingsInMonth = allTickets.Count(),
                    MonthlyRevenue = monthlyRevenue,
                    YearlyRevenue = (double)yearlyRevenue
                };

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<DashboardAdminModel> DashboardAdmin()
        {
            try
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                var totalRevenueInMoth = await _unitOfWork.BookingRepository
                                                          .GetAll()
                                                          .Where(_ => _.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING))
                                                          .SumAsync(_ => _.TotalBill);
                var totalTicketBookedInMonth = _unitOfWork.TicketDetailRepository
                                                          .GetAll()
                                                          .Include(_ => _.Booking)
                                                          .Where(_ => _.Booking.BookingTime.HasValue &&
                                                                      _.Booking.BookingTime.Value.Month == currentMonth &&
                                                                      _.Booking.BookingTime.Value.Year == currentYear &&
                                                                      _.Status.Equals(SD.Booking_TicketStatus.USED_TICKET))
                                                          .Count();
                var totalUsers = _unitOfWork.UserRepository
                                            .GetAll()
                                            .Where(_ => _.UserRole.RoleName.ToUpper().Equals("CUSTOMER") 
                                                     && _.Status.Equals(SD.GeneralStatus.ACTIVE))
                                            .Count();
               
                var monthsInYear = Enumerable.Range(1, 12);

                var revenueAllMonthInYear = await _unitOfWork.BookingRepository
                                                             .GetAll()
                                                             .Where(_ => _.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING) && _.BookingTime.HasValue && _.BookingTime.Value.Year == currentYear)
                                                             .GroupBy(_ => new
                                                             {
                                                                 _.BookingTime.Value.Year,
                                                                 _.BookingTime.Value.Month
                                                             })
                                                             .Select(_ => new
                                                             {
                                                                 Year = _.Key.Year,
                                                                 Month = _.Key.Month,
                                                                 TotalRevenueMonthInYear = _.Sum(_ => _.TotalBill)
                                                             })
                                                             .ToListAsync();

                var fullRevenueMonths = monthsInYear.Select(month =>
                                                   {
                                                       var revenuesForMonth = revenueAllMonthInYear
                                                           .Where(_ => _.Month == month)
                                                           .ToList();

                                                       var totalRevenueForMonth = revenuesForMonth.Sum(_ => _.TotalRevenueMonthInYear ?? 0);

                                                       return new RevenueAllMonthInYear
                                                       {
                                                           Year = currentYear,
                                                           Month = month,
                                                           TotalRevenueMonthInYear = totalRevenueForMonth
                                                       };
                                                   })
                                                    .OrderBy(revenueData => revenueData.Month)
                                                    .ToList();

                var totalCompanies = await _unitOfWork.CompanyRepository
                                                      .GetAll()
                                                      .Where(_ => _.Status.Equals(SD.GeneralStatus.ACTIVE))
                                                      .CountAsync();

                var revenueOfCompanyInMonths = await _unitOfWork.BookingRepository
                                                                .GetAll()
                                                                .Include(_ => _.Trip.Route_Company.Company)
                                                                .Where(_ => _.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING) &&
                                                                            _.BookingTime.Value.Month == currentMonth &&
                                                                            _.BookingTime.Value.Year == currentYear)
                                                                .GroupBy(_ => new {
                                                                    _.Trip.Route_Company.Company.CompanyID,
                                                                    _.Trip.Route_Company.Company.Name
                                                                })
                                                                .Select(_ => new RevenueOfCompanyInMonth
                                                                {
                                                                    CompanyID = _.Key.CompanyID,
                                                                    CompanyName = _.Key.Name,
                                                                    Year = currentYear,
                                                                    Month = currentMonth,
                                                                    TotalRevenueOfCompanyInMonth = _.Sum(_ => _.TotalBill ?? 0)
                                                                })
                                                                .OrderBy(_ => _.CompanyID) 
                                                                .ToListAsync();
                var result = new DashboardAdminModel
                {
                    TotalRevenueInMonth = totalRevenueInMoth,
                    TotalTicketBookedInMonth = totalTicketBookedInMonth,
                    ToTalUsers = totalUsers,
                    TotalCompanies = totalCompanies,
                    RevenueAllMonthInYears = fullRevenueMonths,
                    RevenueOfCompanyInMonths = revenueOfCompanyInMonths
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

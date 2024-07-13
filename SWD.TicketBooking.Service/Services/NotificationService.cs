using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> SendNotification(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>()
                {
                    { "key1", "value1" }
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }
        public async Task<string> SendStaffNotification(Guid staffID, Guid tripID)
        {
            var token = await _unitOfWork.UserRepository
                                         .GetAll()
                                         .Where(t=>t.UserID.Equals(staffID))
                                         .Select(t=>t.AccessToken)
                                         .FirstOrDefaultAsync();
            var trip = await _unitOfWork.TripRepository.FindByCondition(t => t.TripID.Equals(tripID)).FirstOrDefaultAsync();
            var route_companyID = await _unitOfWork.TripRepository
                                         .GetAll()
                                         .Where(r=>r.TripID.Equals(tripID))
                                         .Select(r=>r.Route_CompanyID)
                                         .FirstOrDefaultAsync();
            var routeCompany = await _unitOfWork.Route_CompanyRepository
                                         .GetAll()
                                         .Where(r=>r.Route_CompanyID.Equals(route_companyID))
                                         .FirstOrDefaultAsync();
            var route  = await _unitOfWork.RouteRepository
                                          .GetAll()
                                          .Where(r=>r.RouteID.Equals(routeCompany.RouteID))
                                          .FirstOrDefaultAsync();
            var company = await _unitOfWork.CompanyRepository
                                           .GetAll()
                                           .Where(c => c.CompanyID.Equals(routeCompany.CompanyID))
                                           .FirstOrDefaultAsync();
            var fromCity = await _unitOfWork.CityRepository.FindByCondition(c=>c.CityID.Equals(route.FromCityID)).FirstOrDefaultAsync();
            var toCity = await _unitOfWork.CityRepository.FindByCondition(c => c.CityID.Equals(route.ToCityID)).FirstOrDefaultAsync();

            var message = new Message()
            {
                Token = token,
                Notification = new Notification()
                {
                    Title = "The Bus Journey",
                    Body = $"Bạn vừa được {company.Name} phân công vào chuyến xe từ {fromCity.Name} ở {route.StartLocation} đến {toCity.Name} ở {route.EndLocation} vào lúc {string.Format("{0:HH:mm} ngày {0:dd-MM-yyyy}", trip.StartTime)} đến {string.Format("{0:HH:mm} ngày {0:dd-MM-yyyy}", trip.EndTime)}."
                },
                Data = new Dictionary<string, string>()
                {
                    { "Key1","Value1"}
            }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }

        public async Task<string> GetCityName(Guid cityID)
        {
            var city = await _unitOfWork.CityRepository.GetByIdAsync(cityID);
            return city.Name;
        }

    }
}
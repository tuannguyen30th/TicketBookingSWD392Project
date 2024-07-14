using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class TripStatusUpdaterService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
      
        public TripStatusUpdaterService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateTripStatusesAsync();
                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken); 
            }
        }

        private async Task UpdateTripStatusesAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var currentTime = DateTime.UtcNow;

                var tripsToUpdate = await unitOfWork.TripRepository
                                                    .GetAll()
                                                    .Where(_ => _.EndTime <= currentTime && _.Status != SD.GeneralStatus.INACTIVE)
                                                    .ToListAsync();
                

                foreach (var trip in tripsToUpdate)
                {
                    trip.EndTime = trip.EndTime.Value.AddMinutes(15);

                    if (trip.EndTime <= currentTime)
                    {
                        var bookings = await unitOfWork.BookingRepository.GetAll()
                                                       .Where(_ => _.TripID == trip.TripID)
                                                       .ToListAsync();
                        foreach (var booking in bookings)
                        {
                            var tickets = await unitOfWork.TicketDetailRepository.GetAll()
                                                          .Where(_ => _.BookingID == booking.BookingID)
                                                          .ToListAsync();
                            foreach (var ticket in tickets)
                            {
                                if (ticket.Status.Equals(SD.Booking_TicketStatus.UNUSED_TICKET))
                                {
                                    ticket.Status = SD.Booking_TicketStatus.USED_TICKET;
                                }
                            }
                        }
                        trip.Status = SD.GeneralStatus.INACTIVE;
                    }
                }

               unitOfWork.Complete();
            }
        }
    }
}

using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IStation_ServiceService
    {
        Task<bool> CreateServiceStation(CreateServiceInStationModel createServiceInStationModel);
        Task<bool> UpdateServiceStation(UpdateServiceInStationModel updateServiceInStationModel, Guid stationServiceID);
        Task<bool> ChangeStatusServiceInStation(Guid Station_ServiceID);
    }
}

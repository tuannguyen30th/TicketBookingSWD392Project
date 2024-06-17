using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IServiceService
    {
        Task<int> CreateService(CreateServiceModel createServiceModel);
        Task<int> UpdateService(UpdateServiceModel updateServiceModel);
        Task<bool> UpdateStatus(Guid serviceID);
    }
}

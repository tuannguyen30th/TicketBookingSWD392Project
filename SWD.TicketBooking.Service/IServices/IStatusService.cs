using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IStatusService
    {
        Task<int> ChangeStatus(string entity, Guid Id);
        Task<int> UpdateStatusServiceInTicket(Guid stationId, Guid ticketDetailId, Guid serviceId);

    }
}

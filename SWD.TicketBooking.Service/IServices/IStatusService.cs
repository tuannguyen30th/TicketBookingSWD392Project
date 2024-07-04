using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IStatusService
    {
        Task<int> ChangeStatus(int entity, Guid Id);

    }
}

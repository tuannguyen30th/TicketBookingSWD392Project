using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IServiceTypeService
    {
        Task<ServiceTypeModel> ServicesOfTypeFromStations(Guid stationID, Guid serviceTypeID);
    }
}

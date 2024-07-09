using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IRouteService
    {
        Task<List<RouteModel>> GetAllRoutes();
        Task<int> CreateRoute(CreateRouteModel model);
        Task<int> UpdateRoute(Guid routeId, CreateRouteModel model);
        Task<List<GetRouteFromCompanyModel>> GetAllRouteFromCompany(Guid companyID);
        Task<List<PopularRouteModel>> GetPopularRoutes();
    }
}

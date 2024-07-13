using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IDashboardService
    {
        Task<DashboardInfoByCompanyModel> GetAllDashboardInfoByCompany(Guid companyID);
        Task<DashboardAdminModel> DashboardAdmin();
    }
}

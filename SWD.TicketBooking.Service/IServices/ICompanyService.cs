using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public interface ICompanyService
    {
        Task<List<GetCompanyModel>> GetAllActiveCompanies();
        Task<GetCompanyModel> GetCompanyById(Guid id);
        Task<int> CreateCompany(CreateCompanyModel model);
        Task<int> UpdateCompany(Guid companyId, CreateCompanyModel model);
        Task<int> ChangeStatus(Guid companyId, string status);
    }
}

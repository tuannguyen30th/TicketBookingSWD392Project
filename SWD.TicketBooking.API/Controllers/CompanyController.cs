using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("company-management")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("managed-companies")]
        public async Task<IActionResult> GetAllActiveCompanies()
        {
            var companies = await _companyService.GetAllActiveCompanies();
            var rs = _mapper.Map<List<GetCompanyResponse>>(companies);
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpGet("managed-companies/{companyID}")]
        public async Task<IActionResult> GetCompanyById([FromRoute] Guid companyID)
        {
            var company = await _companyService.GetCompanyById(companyID);
            var rs = _mapper.Map<GetCompanyResponse>(company);
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpPost("managed-companies")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest req)
        {
            var map = _mapper.Map<CreateCompanyModel>(req);
            var rs = await _companyService.CreateCompany(map);
            if (rs < 1)
            {
                return BadRequest("Create failed");
            }
            return Ok("Create successfully");
        }

        [AllowAnonymous]
        [HttpPut("managed-companies/{companyID}")]
        public async Task<IActionResult> UpdateCompany([FromRoute] Guid companyID, [FromBody] CreateCompanyRequest req)
        {
            /*if (companyID <= 0)
            {
                return BadRequest("Invalid ID");
            }*/
            var map = _mapper.Map<CreateCompanyModel>(req);
            var rs = await _companyService.UpdateCompany(companyID, map);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }

        [AllowAnonymous]
        [HttpPut("managed-companies/{companyID}/status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid companyID, [FromBody] ChangeStatusRequest req)
        {
            /*if (companyID <= 0)
            {
                return BadRequest("Invalid ID");
            }*/
            var rs = await _companyService.ChangeStatus(companyID, req.Status);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }
    }
}

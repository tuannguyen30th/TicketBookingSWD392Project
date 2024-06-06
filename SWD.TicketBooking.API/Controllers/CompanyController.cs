﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(CompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("all-active-companies")]
        public async Task<IActionResult> GetAllActiveCompanies()
        {
            var companies = await _companyService.GetAllActiveCompanies();
            var rs = _mapper.Map<List<GetCompanyResponse>>(companies);
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpGet("company-by-id/{id}")]
        public async Task<IActionResult> GetCompanyById([FromRoute] int id)
        {
            var company = await _companyService.GetCompanyById(id);
            var rs = _mapper.Map<GetCompanyResponse>(company);
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpPost("company")]
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
        [HttpPut("company/{id}")]
        public async Task<IActionResult> UpdateCompany([FromRoute] int id ,[FromBody] CreateCompanyRequest req)
        {
            var map = _mapper.Map<CreateCompanyModel>(req);
            var rs = await _companyService.UpdateCompany(id, map);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class CompanyService
    {
        private readonly IRepository<Company, int> _companyRepo;
        private readonly IMapper _mapper;

        public CompanyService(IRepository<Company, int> companyRepo, IMapper mapper)
        {
            _companyRepo = companyRepo;
            _mapper = mapper;
        }

        public async Task<List<GetCompanyModel>> GetAllActiveCompanies()
        {
            try
            {
                var companies = await _companyRepo.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE)).ToListAsync();
                var rs = _mapper.Map<List<GetCompanyModel>>(companies);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<GetCompanyModel> GetCompanyById(int id)
        {
            try
            {
                var company = await _companyRepo.GetByIdAsync(id);
                var rs = _mapper.Map<GetCompanyModel>(company);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> CreateCompany(CreateCompanyModel model)
        {
            try
            {
                var checkExisted = await _companyRepo.GetAll().Where(_ => _.Name ==  model.Name).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Company name existed");
                }
                var company = await _companyRepo.AddAsync(new Company
                {
                    Name = model.Name,
                    Status = SD.ACTIVE
                });
                if (company == null)
                {
                    throw new InternalServerErrorException("Cannot create");
                }
                var rs = await _companyRepo.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateCompany(int companyId, CreateCompanyModel model)
        {
            try
            {
                var checkExisted = await _companyRepo.GetAll().Where(_ => _.Name == model.Name).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Company name already existed");
                }

                var entity = await _companyRepo.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE) && _.CompanyID == companyId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find company");
                }

                entity.Name = model.Name;

                var companyUpdate = _companyRepo.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                var rs = await _companyRepo.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> ChangeStatus(int companyId, string status)
        {
            try
            {
                var entity = await _companyRepo.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE) && _.CompanyID == companyId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find company");
                }

                entity.Status = status;

                var companyUpdate = _companyRepo.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                var rs = await _companyRepo.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
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
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IRepository<Company, Guid> _unitOfWork.CompanyRepository;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork unitOfWork, IRepository<Company, Guid> companyRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_unitOfWork.CompanyRepository = companyRepo;
            _mapper = mapper;
        }

        public async Task<List<GetCompanyModel>> GetAllActiveCompanies()
        {
            try
            {
                var companies = await _unitOfWork.CompanyRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)).ToListAsync();
                var rs = _mapper.Map<List<GetCompanyModel>>(companies);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<GetCompanyModel> GetCompanyById(Guid id)
        {
            try
            {
                var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
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
                var checkExisted = await _unitOfWork.CompanyRepository.GetAll().Where(_ => _.Name ==  model.Name).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Company name existed");
                }
                var company = await _unitOfWork.CompanyRepository.AddAsync(new Company
                {
                    CompanyID = Guid.NewGuid(),
                    Name = model.Name,
                    Status = SD.GeneralStatus.ACTIVE
                });
                if (company == null)
                {
                    throw new InternalServerErrorException("Cannot create");
                }
                var rs = await _unitOfWork.CompanyRepository.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateCompany(Guid companyId, CreateCompanyModel model)
        {
            try
            {
                var checkExisted = await _unitOfWork.CompanyRepository.GetAll().Where(_ => _.Name == model.Name).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Company name already existed");
                }

                var entity = await _unitOfWork.CompanyRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.CompanyID == companyId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find company");
                }

                entity.Name = model.Name;

                var companyUpdate = _unitOfWork.CompanyRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                //var rs = await _unitOfWork.CompanyRepository.Commit();
                var rs = _unitOfWork.Complete();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> ChangeStatus(Guid companyId, string status)
        {
            try
            {
                var entity = await _unitOfWork.CompanyRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.CompanyID == companyId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find company");
                }

                entity.Status = status;

                var companyUpdate = _unitOfWork.CompanyRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                //var rs = await _unitOfWork.CompanyRepository.Commit();
                var rs = _unitOfWork.Complete();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

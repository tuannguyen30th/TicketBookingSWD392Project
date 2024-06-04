/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketBooking.Common.Payloads.Requests;
using TicketBooking.Dtos;
using TicketBooking.Entities;
using TicketBooking.Repositories;

namespace TicketBooking.Services
{
    public class TransportationPictureService
    {
        private readonly IRepository<TransportationPicture, int> _transportationPictureRepo;
        private readonly IMapper _mapper;

        public TransportationPictureService(IRepository<TransportationPicture, int> transportationPictureRepo, IMapper mapper)
        {
            _transportationPictureRepo = transportationPictureRepo;
            _mapper = mapper;
        }

        public async Task<(List<TransportationPictureDTO> returnModel, string message)> GetAllPictureOfTransportation(int transId)
        {
            try
            {
                var pictures = await _transportationPictureRepo.GetAll().Where(p => p.TransportationID == transId && p.Status.ToLower() == "active").ToListAsync();
                if (pictures.IsNullOrEmpty())
                {
                    return (null, "Empty List");
                }
                var result = _mapper.Map<List<TransportationPictureDTO>>(pictures);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationPictureDTO returnModel, string message)> GetPictureById(int id)
        {
            try
            {
                var picture = await _transportationPictureRepo.FindByCondition(v => v.TransportationPictureID == id && v.Status.ToUpper() == "Active".ToUpper()).FirstOrDefaultAsync();
                if (picture == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationPictureDTO>(picture);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationPictureDTO returnModel, string message)> CreateTransportationPicture(CreateTransportationPictureReq req)
        {
            try
            {
                var picture = await _transportationPictureRepo.AddAsync(new TransportationPicture
                {
                    ImageUrl = req.ImageUrl,
                    TransportationID = req.TransportationID,
                    Status = "Active"
                });
                await _transportationPictureRepo.Commit();
                if (picture == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationPictureDTO>(picture);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationPictureDTO returnModel, string message)> UpdatePicture(int id, UpdateTransportationPictureReq req)
        {
            try
            {
                var findPicture = await _transportationPictureRepo.FindByCondition(v => v.TransportationPictureID == id && v.Status.ToUpper() == "Active".ToUpper()).FirstOrDefaultAsync();
                if (findPicture == null)
                {
                    return (null, "Not found!");
                }

                findPicture.ImageUrl = req.ImageUrl;

                var checkExistedPicture = await _transportationPictureRepo.FindByCondition(x => x.TransportationID == findPicture.TransportationID && x.ImageUrl == findPicture.ImageUrl).FirstOrDefaultAsync();
                if (checkExistedPicture != null)
                {
                    return (null, "Picture already exists on this transportation");
                }

                _transportationPictureRepo.Update(findPicture);
                await _transportationPictureRepo.Commit();

                var result = _mapper.Map<TransportationPictureDTO>(findPicture);
                return (result, "Ok");
            }

            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationPictureDTO returnModel, string message)> ChangePictureStatus(int id, ChangeStatusReq req)
        {
            try
            {
                var picture = await _transportationPictureRepo.FindByCondition(v => v.TransportationPictureID == id).FirstOrDefaultAsync();
                if (picture == null)
                {
                    return (null, "Cannot found this vehicle type");
                }
                else
                {
                    picture.Status = req.Status;
                }
                var updateResult = _transportationPictureRepo.Update(picture);
                await _transportationPictureRepo.Commit();
                if (updateResult == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationPictureDTO>(updateResult);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
}
*/
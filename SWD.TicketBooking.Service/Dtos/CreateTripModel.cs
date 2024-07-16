using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class CreateTripModel
    {
        public Guid? Route_CompanyID { get; set; }
        public bool? IsTemplate { get; set; }
        public Guid? StaffID { get; set; }
        public Guid? TemplateID { get; set; }
        public List<IFormFile> ImageUrls { get; set; } = new List<IFormFile>();

        public string TimeTripsString
        {
            get => JsonConvert.SerializeObject(TimeTrips);
            set => TimeTrips = string.IsNullOrEmpty(value) ? new List<TimeTrip>() : JsonConvert.DeserializeObject<List<TimeTrip>>(value);
        }
        public string TicketType_TripModelsString
        {
            get => JsonConvert.SerializeObject(TicketType_TripModels);
            set => TicketType_TripModels = string.IsNullOrEmpty(value) ? new List<TicketType_TripModel>() : JsonConvert.DeserializeObject<List<TicketType_TripModel>>(value);
        }
        public string Trip_UtilityModelsString
        {
            get => JsonConvert.SerializeObject(Trip_UtilityModels);
            set => Trip_UtilityModels = string.IsNullOrEmpty(value) ? new List<Trip_UtilityModel>() : JsonConvert.DeserializeObject<List<Trip_UtilityModel>>(value);
        }

        public List<TimeTrip> TimeTrips { get; set; } = new List<TimeTrip>();
        public List<TicketType_TripModel> TicketType_TripModels { get; set; } = new List<TicketType_TripModel>();
        public List<Trip_UtilityModel> Trip_UtilityModels { get; set; } = new List<Trip_UtilityModel>();
    }

    public static class CreateTripModelExtention
    {
        public static Trip MapToTrip(this CreateTripModel createModel)
        {
            var newTripId = Guid.NewGuid();
            return new Trip
            {
                TripID = newTripId,
                Route_CompanyID = createModel.Route_CompanyID,
                StaffID = null,
                IsTemplate = true,
                TemplateID = newTripId,
                Status = SD.GeneralStatus.ACTIVE,
            };
        }


        private static readonly int Min_TicketType_TripModels_Quantity = 2;
        private static readonly int Min_TicketType_TripModels_TotalQuantity = 20;
        private static readonly int Min_TicketType_TripModels_Quantity_For_CheckName = 2;
        public static async Task<(bool isSuccess, string message)> ValidateForCreatingWithTemplate(this CreateTripModel createModel, IUnitOfWork unitOfWork)
        {
            if (createModel.ImageUrls.Count == 0)
            {
                return (false, "TẤT CẢ CÁC TRƯỜNG PHẢI CÓ DỮ LIỆU!");
            }

            if (createModel.TicketType_TripModels.Count < Min_TicketType_TripModels_Quantity)
            {
                return (false, "PHẢI CÓ ÍT NHẤT 2 LOẠI GHẾ!");
            }

            int? totalQuantity = createModel.TicketType_TripModels.Sum(_ => _.Quantity);
            if (totalQuantity.HasValue && totalQuantity.Value <= Min_TicketType_TripModels_TotalQuantity)
            {
                throw new BadRequestException($"TỔNG SỐ LƯỢNG GHẾ PHẢI ÍT NHẤT LÀ {Min_TicketType_TripModels_TotalQuantity}!");
            }

            if (createModel.TicketType_TripModels.Count == 2)
            {
                var ticketTypes = new List<string> { "HÀNG ĐẦU", "HÀNG SAU" };

                foreach (var ticketType in createModel.TicketType_TripModels)
                {
                    var checkName = await unitOfWork.TicketTypeRepository
                                                 .FindByCondition(_ => _.TicketTypeID == ticketType.TicketTypeID)
                                                 .Select(_ => _.Name.ToUpper())
                                                 .FirstOrDefaultAsync();

                    if (!ticketTypes.Contains(checkName))
                    {
                        return (false, "NẾU LÀ HAI LOẠI GHẾ THÌ BẮT BUỘC PHẢI LÀ HÀNG ĐẦU VÀ HÀNG SAU!");
                    }
                }
            }

            if (createModel.TicketType_TripModels.Any(_ => _.Price <= 0))
            {
                throw new BadRequestException("GIÁ VÉ PHẢI LỚN HƠN 0!");
            }

            if (createModel.TicketType_TripModels.Any(_ => _.Quantity <= 0))
            {
                throw new BadRequestException("SỐ LƯỢNG PHẢI LỚN HƠN 0!");
            }
            if (createModel.TicketType_TripModels.Any(_ => _.Quantity % 4 != 0))
            {
                throw new BadRequestException("SỐ LƯỢNG GHẾ KHÔNG HỢP LỆ!");
            }


            return (true, string.Empty);
        }
    }

    public class TimeTrip
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
    public class TicketType_TripModel
    {
        public Guid? TicketTypeID { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
    }
    public class Trip_UtilityModel
    {
        public Guid? UtilityID { get; set; }
    }
}
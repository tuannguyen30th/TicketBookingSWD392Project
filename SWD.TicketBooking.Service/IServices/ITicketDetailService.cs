﻿using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public interface ITicketDetailService
    {
        Task<GetDetailOfTicketByIDModel> GetDetailOfTicketByID(Guid ticketDetailID);
        Task<List<GetTicketDetailByUserModel>> GetTicketDetailByUser(Guid customerID);
        Task<SearchTicketModel> SearchTicket(string QRCode, string email);
        Task<ActionOutcome> CancelTicket(Guid ticketDetailID);
        Task<GetTicketDetailInMobileModel> GetTicketDetailInMobile(string qrCode);
        Task<bool> VerifyTicketDetail(string qrCode);

    }
}

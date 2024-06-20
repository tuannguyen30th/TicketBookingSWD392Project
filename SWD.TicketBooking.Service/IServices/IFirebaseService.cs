using Microsoft.AspNetCore.Http;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.BackendService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IFirebaseService
    {
        Task<ActionOutcome> UploadFileToFirebase(IFormFile file, string pathFileName);
        Task<ActionOutcome> UploadFilesToFirebase(List<IFormFile> files, string basePath);
        public Task<string> GetUrlImageFromFirebase(string pathFileName);
        public Task<ActionOutcome> DeleteFileFromFirebase(string pathFileName);
    }
}

using Microsoft.AspNetCore.Http;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.BackendService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services.FirebaseService
{
    public interface IFirebaseService
    {
        Task<AppActionResult> UploadFileToFirebase(IFormFile file, string pathFileName);
        Task<AppActionResult> UploadFilesToFirebase(List<IFormFile> files, string basePath);
        public Task<string> GetUrlImageFromFirebase(string pathFileName);
        public Task<AppActionResult> DeleteFileFromFirebase(string pathFileName);
    }
}

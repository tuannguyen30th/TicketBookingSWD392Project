using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net;
using Firebase.Auth;
using Firebase.Storage;
using RestSharp;
using Microsoft.Extensions.Configuration;
using SWD.TicketBooking.Repo.Repositories;
using static SWD.TicketBooking.Service.Configuration.ConfigurationModel;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Dtos;

namespace SWD.TicketBooking.Service.Services
{
    public class FirebaseService : GenericBackendService, IFirebaseService
    {
        private ActionOutcome _result;
        private FirebaseConfiguration _firebaseConfiguration;
        private readonly IConfiguration _configuration;
        public FirebaseService(IServiceProvider serviceProvider, IConfiguration configuration, FirebaseConfiguration firebaseConfiguration) : base(serviceProvider)
        {
            _result = new();
            _firebaseConfiguration = firebaseConfiguration;
            _configuration = configuration;
        }

        public async Task<ActionOutcome> DeleteFileFromFirebase(string pathFileName)
        {
            var _result = new ActionOutcome();
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseConfiguration.ApiKey));
                var account = await auth.SignInWithEmailAndPasswordAsync(_firebaseConfiguration.AuthEmail, _firebaseConfiguration.AuthPassword);
                var storage = new FirebaseStorage(
             _firebaseConfiguration.Bucket,
             new FirebaseStorageOptions
             {
                 AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                 ThrowOnCancel = true
             });
                await storage
                    .Child(pathFileName)
                    .DeleteAsync();
                _result.Message = "Xóa file thành công!".ToUpper();
                _result.IsSuccess = true;
            }
            catch (FirebaseStorageException ex)
            {
                _result.Message = $"Lỗi khi xóa file: {ex.Message}".ToUpper();
            }
            return _result;
        }

        public async Task<string> GetUrlImageFromFirebase(string pathFileName)
        {
            var a = pathFileName.Split("/o/")[1];
            var api = $"https://firebasestorage.googleapis.com/v0/b/cloudfunction-yt-2b3df.appspot.com/o?name={a}";
            if (string.IsNullOrEmpty(pathFileName))
            {
                return string.Empty;
            }

            var client = new RestClient();
            var request = new RestRequest(api);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jmessage = JObject.Parse(response.Content);
                var downloadToken = jmessage.GetValue("downloadTokens").ToString();
                return
                    $"https://firebasestorage.googleapis.com/v0/b/{_configuration["cloudfunction-yt-2b3df.appspot.com"]}/o/{pathFileName}?alt=media&token={downloadToken}";
            }

            return string.Empty;
        }

        public async Task<ActionOutcome> UploadFileToFirebase(IFormFile file, string pathFileName)
        {
            var _result = new ActionOutcome();
            bool isValid = true;
            if (file == null || file.Length == 0)
            {
                isValid = false;
                _result.Message = "File đang bị trống!".ToUpper();
            }
            if (isValid)
            {
                var stream = file!.OpenReadStream();
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseConfiguration.ApiKey));
                var account = await auth.SignInWithEmailAndPasswordAsync(_firebaseConfiguration.AuthEmail, _firebaseConfiguration.AuthPassword);
                string destinationPath = $"{pathFileName}";

                var task = new FirebaseStorage(
                _firebaseConfiguration.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child(destinationPath)
                .PutAsync(stream);
                var downloadUrl = await task;

                if (task != null)
                {
                    _result.Result = downloadUrl;
                }
                else
                {
                    _result.IsSuccess = false;
                    _result.Message =  "Tải file lên lỗi!".ToUpper();
                }
            }
            return _result;
        }
        public async Task<ActionOutcome> UploadFilesToFirebase(List<IFormFile> files, string basePath)
        {
            var _result = new ActionOutcome();
            var uploadResults = new List<string>();

            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseConfiguration.ApiKey));
            var account = await auth.SignInWithEmailAndPasswordAsync(_firebaseConfiguration.AuthEmail, _firebaseConfiguration.AuthPassword);
            var storage = new FirebaseStorage(
                _firebaseConfiguration.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                    ThrowOnCancel = true
                });

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    _result.Message = "Một hoặc nhiều file bị trống!".ToUpper();
                    continue;
                }

                var stream = file.OpenReadStream();
                string destinationPath = $"{basePath}/{file.FileName}";

                var task = storage.Child(destinationPath).PutAsync(stream);
                var downloadUrl = await task;

                if (task != null)
                {
                    uploadResults.Add(downloadUrl);
                }
                else
                {
                    _result.IsSuccess = false;
                    _result.Message = $"Tải file lên bị lỗi : {file.FileName}".ToUpper();
                }
            }

            _result.Result = uploadResults;
            if (uploadResults.Count == files.Count)
            {
                _result.IsSuccess = true;
                _result.Message = "Tải toàn bộ file thành công!".ToUpper();
            }
            else
            {
                _result.IsSuccess = false;
                _result.Message = "Một số file bị lỗi khi tải lên!".ToUpper();
            }

            return _result;
        }
    }
}

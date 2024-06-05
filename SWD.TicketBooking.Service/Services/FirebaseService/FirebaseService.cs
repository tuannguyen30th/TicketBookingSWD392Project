using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net;
using Firebase.Auth;
using Firebase.Storage;
using RestSharp;
using Microsoft.Extensions.Configuration;
using SWD.TicketBooking.Repo.Repositories;
using static SWD.TicketBooking.Service.Configuration.ConfigurationModel;
using SWD.TicketBooking.Service.Dtos.BackendService;

namespace SWD.TicketBooking.Service.Services.FirebaseService
{
    public class FirebaseService : GenericBackendService, IFirebaseService
    {
        ///private readonly IConverter _pdfConverter;
        private AppActionResult _result;
        private FirebaseConfiguration _firebaseConfiguration;
        private readonly IConfiguration _configuration;
        public FirebaseService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
        {
            //_pdfConverter = pdfConverter;
            _result = new();
            _firebaseConfiguration = Resolve<FirebaseConfiguration>();
            _configuration = configuration;
        }

        public async Task<AppActionResult> DeleteFileFromFirebase(string pathFileName)
        {
            var _result = new AppActionResult();
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
                _result.Messages.Add("Delete image successful");
            }
            catch (FirebaseStorageException ex)
            {
                _result.Messages.Add($"Error deleting image: {ex.Message}");
            }
            return _result;
        }

        public async Task<string> GetUrlImageFromFirebase(string pathFileName)
        {
            var a = pathFileName.Split("/");
            pathFileName = $"{a[0]}%2F{a[1]}";
            var api = $"https://firebasestorage.googleapis.com/v0/b/cloudfunction-yt-2b3df.appspot.com/o?name={pathFileName}";
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
                    $"https://firebasestorage.googleapis.com/v0/b/{_configuration["Firebase:Bucket"]}/o/{pathFileName}?alt=media&token={downloadToken}";
            }

            return string.Empty;
        }

        public async Task<AppActionResult> UploadFileToFirebase(IFormFile file, string pathFileName)
        {
            var _result = new AppActionResult();
            bool isValid = true;
            if (file == null || file.Length == 0)
            {
                isValid = false;
                _result.Messages.Add("The file is empty");
            }
            if (isValid)
            {
                /* "ApiKey": "AIzaSyCMlVnrHg3XcmcgNXfIvHjw-6OGyoaC33w",
     "AuthEmail": "nguyenngoctuanthct@gmail.com",
     "AuthPassword": "0917899898",
     "Bucket": "cloudfunction-yt-2b3df.appspot.com"*/
                var stream = file!.OpenReadStream();
                var auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCMlVnrHg3XcmcgNXfIvHjw-6OGyoaC33w"));
                var account = await auth.SignInWithEmailAndPasswordAsync("nguyenngoctuanthct@gmail.com", "0917899898");
                string destinationPath = $"{pathFileName}";

                var task = new FirebaseStorage(
                "cloudfunction-yt-2b3df.appspot.com",
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
                    _result.Messages.Add("Upload failed");
                }
            }
            return _result;
        }
    }
}

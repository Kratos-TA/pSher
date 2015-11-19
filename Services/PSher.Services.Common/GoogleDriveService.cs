namespace PSher.Services.Common
{
    using System.Threading.Tasks;
    using Contracts;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v2;
    using Google.Apis.Drive.v2.Data;
    using Google.Apis.Services;
    using Google.Apis.Util.Store;
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Google.Apis.Auth.OAuth2.Flows;
    using Google.Apis.Auth.OAuth2.Responses;
    using PSher.Common.Constants;
    using File = Google.Apis.Drive.v2.Data.File;

    public class GoogleDriveService : IWebStorageService
    {
        private readonly DriveService service;

        public GoogleDriveService()
        {
            this.service = this.CreateServie();
        }

        public async Task<string> UploadImageToCloud(byte[] byteArrayContent, string fileName, string fileExstension, string parentPath = WebStorageConstants.Collection)
        {
            var parentId = await this.CreateDirectory(parentPath);


            File body = new File
            {
                Title = fileName,
                Parents = new List<ParentReference>() { new ParentReference() { Id = parentId } }
            };

            MemoryStream stream = new MemoryStream(byteArrayContent);
            try
            {
                FilesResource.InsertMediaUpload request = this.service.Files.Insert(body, stream, "image/" + fileExstension);
                var result = await request.UploadAsync();

                return WebStorageConstants.GoogleDriveSourceLink + request.ResponseBody.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task<string> CreateDirectory(string title, string parent = null)
        {
            File directory = null;
            File body = new File
            {
                Title = title,
                MimeType = "application/vnd.google-apps.folder"
            };

            try
            {
                FilesResource.InsertRequest request = this.service.Files.Insert(body);
                directory = await request.ExecuteAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return directory.Id;
        }

        private DriveService CreateServie()
        { 
                var tokenResponse = new TokenResponse
                {
                    AccessToken = WebStorageConstants.GoogleDriveAccessToken,
                    RefreshToken = WebStorageConstants.GoogleRefreshToken,
                };

                var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = WebStorageConstants.GoogleDriveId,
                        ClientSecret = WebStorageConstants.GoogleDriveSecret
                    },
                    Scopes = new[] { DriveService.Scope.Drive },
                    DataStore = new FileDataStore(WebStorageConstants.GoogleDriveApplicationName)
                });

                var credential = new UserCredential(apiCodeFlow, WebStorageConstants.GoogleDriveEmail, tokenResponse);

                var newService = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = WebStorageConstants.GoogleDriveApplicationName
                });

                return newService;
        }
    }
}

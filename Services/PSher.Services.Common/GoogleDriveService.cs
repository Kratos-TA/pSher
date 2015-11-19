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
    using System.Threading;
    using PSher.Common.Constants;
    using File = Google.Apis.Drive.v2.Data.File;

    public class GoogleDriveService : IWebStorageService
    {
        private readonly DriveService service;

        public GoogleDriveService()
        {
            this.service = this.AuthenticateOauth();
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

        private DriveService AuthenticateOauth()
        {
            string[] scopes = new string[]
            {
                DriveService.Scope.Drive,  // view and manage your files and documents
                DriveService.Scope.DriveAppdata,  // view and manage its own configuration data
                DriveService.Scope.DriveAppsReadonly,   // view your drive apps
                DriveService.Scope.DriveFile,   // view and manage files created by this app
                DriveService.Scope.DriveMetadataReadonly,   // view metadata for files
                DriveService.Scope.DriveReadonly,   // view files and documents on your drive
                DriveService.Scope.DriveScripts // modify your app scripts
            };

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential = GoogleWebAuthorizationBroker
                    .AuthorizeAsync(
                        new ClientSecrets { ClientId = WebStorageConstants.GoogleDriveId, ClientSecret = WebStorageConstants.GoogleDriveSecret },
                            scopes,
                            WebStorageConstants.GoogleDriveApplicationUserName,
                            CancellationToken.None,
                            new FileDataStore("pSher.Drive.Auth.Store")).Result;

                DriveService service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = WebStorageConstants.GoogleDriveApplicationName,
                });

                return service;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

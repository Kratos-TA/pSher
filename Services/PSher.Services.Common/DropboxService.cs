namespace PSher.Services.Common
{
    using System.Threading.Tasks;

    using PSher.Common.Constants;
    using PSher.Services.Common.Contracts;

    using Spring.IO;
    using Spring.Social.Dropbox.Api;
    using Spring.Social.Dropbox.Connect;
    using Spring.Social.OAuth1;

    public class DropboxService : IWebStorageService
    {
        private string dropboxAppKey;
        private string dropboxAppSecret;
        private IDropbox dropboxApi;
        private OAuthToken oauthAccessToken;

        public DropboxService()
        {
            this.dropboxAppKey = WebStorageConstants.DropboxAppKey;
            this.dropboxAppSecret = WebStorageConstants.DropboxAppSecret;
            this.dropboxApi = this.GetDropboxApi();
        }

        public async Task<Entry> UploadImageToCloud(IResource resource, string fileName)
        {
            string path = "/" + WebStorageConstants.Collection + "/" + fileName;
            Entry uploadFileEntry = await this.dropboxApi.UploadFileAsync(resource, path);

            return uploadFileEntry;
        }

        public async Task<string> GetImageUrl(string path)
        {
            var mediaLink = await this.dropboxApi.GetMediaLinkAsync(path);

            return mediaLink.Url;
        }

        private IDropbox GetDropboxApi()
        {
            DropboxServiceProvider serviceProvider = this.Initialize(this.dropboxAppKey, this.dropboxAppSecret);
            IDropbox currentDropboxApi = serviceProvider.GetApi(this.oauthAccessToken.Value, this.oauthAccessToken.Secret);

            return currentDropboxApi;
        }

        private DropboxServiceProvider Initialize(string key, string secret)
        {
            DropboxServiceProvider dropboxServiceProvider = new DropboxServiceProvider(key, secret, AccessLevel.AppFolder);

            this.oauthAccessToken = this.LoadOAuthToken();

            return dropboxServiceProvider;
        }

        private OAuthToken LoadOAuthToken()
        {
            this.oauthAccessToken = new OAuthToken(WebStorageConstants.DropBoxOAuth1, WebStorageConstants.DropBoxOAuth2);

            return this.oauthAccessToken;
        }
    }
}
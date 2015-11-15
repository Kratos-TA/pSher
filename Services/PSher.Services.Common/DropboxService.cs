namespace PSher.Services.Common
{
    using System.Diagnostics;
    using System.IO;

    using Spring.IO;
    using Spring.Social.Dropbox.Api;
    using Spring.Social.Dropbox.Connect;
    using Spring.Social.OAuth1;

    using PSher.Common.Constants;
    using PSher.Services.Common.Contracts;
    using System.Threading.Tasks;

    public class DropboxService : IDropboxService
    {
        private string dropboxAppKey;
        private string dropboxAppSecret;
        private IDropbox dropboxApi;
        private OAuthToken oauthAccessToken;        

        public DropboxService()
        {
            this.dropboxAppKey = DropboxConstants.DropboxAppKey;
            this.dropboxAppSecret = DropboxConstants.DropboxAppSecret;
            this.dropboxApi = this.GetDropboxApi();
        }

        public async Task<Entry> UploadImageToCloud(IResource resource, string fileName)
        {
            string path = "/" + DropboxConstants.Collection + "/" + fileName;
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

            // This constant may contain the wrong file path - if no authentication check here.
            if (!File.Exists(DropboxConstants.OAuthTokenFileName))
            {
                this.AuthorizeAppOAuth(dropboxServiceProvider);
            }

            this.oauthAccessToken = this.LoadOAuthToken();

            return dropboxServiceProvider;
        }

        private void AuthorizeAppOAuth(DropboxServiceProvider dropboxServiceProvider)
        {
            OAuthToken oauthToken = dropboxServiceProvider.OAuthOperations.FetchRequestTokenAsync(null, null).Result;

            OAuth1Parameters parameters = new OAuth1Parameters();
            string authenticateUrl = dropboxServiceProvider.OAuthOperations.BuildAuthorizeUrl(oauthToken.Value, parameters);
            Process.Start(authenticateUrl);

            AuthorizedRequestToken requestToken = new AuthorizedRequestToken(oauthToken, null);
            OAuthToken oauthAccessToken =
                dropboxServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;

            string[] oauthData = new string[] { oauthAccessToken.Value, oauthAccessToken.Secret };
            File.WriteAllLines(DropboxConstants.OAuthTokenFileName, oauthData);
        }

        private OAuthToken LoadOAuthToken()
        {
            string[] lines = File.ReadAllLines(DropboxConstants.OAuthTokenFileName);
            this.oauthAccessToken = new OAuthToken(lines[0], lines[1]);

            return this.oauthAccessToken;
        }
    }
}

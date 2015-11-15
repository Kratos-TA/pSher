namespace PSher.Services.Common.Contracts
{
    using Spring.IO;
    using Spring.Social.Dropbox.Api;

    public interface IDropboxService
    {
        Entry UploadImageToCloud(IResource resource);

        string GetImageUrl(string path);
    }
}

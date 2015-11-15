namespace PSher.Services.Common.Contracts
{
    using System.Threading.Tasks;

    using Spring.IO;
    using Spring.Social.Dropbox.Api;

    public interface IDropboxService
    {
        Task<Entry> UploadImageToCloud(IResource resource, string fileName);

        Task<string> GetImageUrl(string path);
    }
}

namespace PSher.Services.Common.Contracts
{
    using System.Threading.Tasks;

    public interface IWebStorageService
    {
        /// <summary>
        /// The method should return the uploaded image direct source URL.
        /// </summary>
        /// <param name="byteArrayContent">The content of the image in byte array format.</param>
        /// <param name="fileName">The name of the image that be stored int the cloud provider.</param>
        /// <param name="fileExstension">The extension of the file.</param>
        /// <param name="parentPath">The parent category path of the image. (Author Id for example).</param>
        /// <returns>Returns source URL of the image.</returns>
        Task<string> UploadImageToCloud(
            byte[] byteArrayContent, 
            string fileName, 
            string fileExstension,
            string parentPath = null);
    }
}

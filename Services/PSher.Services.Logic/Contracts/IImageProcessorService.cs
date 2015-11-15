namespace PSher.Services.Logic.Contracts
{
    using System.Threading.Tasks;

    public interface IImageProcessorService
    {
        Task<byte[]> Resize(byte[] originalImage, int width);
    }
}

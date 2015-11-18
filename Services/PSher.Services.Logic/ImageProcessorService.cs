namespace PSher.Services.Logic
{
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;

    using ImageProcessor;
    using ImageProcessor.Imaging;
    using ImageProcessor.Imaging.Formats;

    using PSher.Common.Constants;
    using PSher.Services.Logic.Contracts;

    public class ImageProcessorService : IImageProcessorService
    {
        public async Task<byte[]> Resize(byte[] originalImage, int width)
        {
            return await Task.Run(() =>
            {
                using (var originalImageStream = new MemoryStream(originalImage))
                {
                    using (var resultImage = new MemoryStream())
                    {
                        using (var imageFactory = new ImageFactory())
                        {
                            var createdImage = imageFactory
                                .Load(originalImageStream);
                         
                                createdImage = createdImage
                                    .Resize(new ResizeLayer(new Size(width, 0), ResizeMode.Max));

                            createdImage
                                .Format(new JpegFormat { Quality = GlobalConstants.ImageQuality })
                                .Save(resultImage);
                        }

                        return resultImage.GetBuffer();
                    }
                }
            });
        }
    }
}

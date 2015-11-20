namespace PSher.Api.Tests.Setups
{
    using System.Threading.Tasks;
    using Moq;
    using Services.Logic.Contracts;

    public static class LogicServices
    {
        public static IImageProcessorService GetImageProcessorService()
        {
            var service = new Mock<IImageProcessorService>();

            service
                .Setup(s => s.Resize(
                    It.IsAny<byte[]>(),
                    It.IsAny<int>()))
                .Returns(Task<byte[]>.FromResult(new byte[1000]));

            return service.Object;
        }
    }
}

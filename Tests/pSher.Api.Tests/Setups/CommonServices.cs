namespace PSher.Api.Tests.Setups
{
    using System.Threading.Tasks;
    using Moq;
    using PSher.Services.Common.Contracts;

    public static class CommonServices
    {
        public static IWebStorageService GetGoogleDriveService()
        {
            var service = new Mock<IWebStorageService>();

            service
                .Setup(s => s.UploadImageToCloud(
                    It.IsAny<byte[]>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task<string>.FromResult("http://downlowdmefromhere.net"));

            return service.Object;
        }

        public static INotificationService GetNotifyService()
        {
            var service = new Mock<INotificationService>();

            service
                .Setup(s => s.Notify(It.IsAny<string>()));

            return service.Object;
        }
    }
}

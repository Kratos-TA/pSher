namespace PSher.Api.Tests.Setups
{
    using System.ComponentModel.DataAnnotations;
    using PSher.Services.Data;
    using PSher.Services.Data.Contracts;

    public static class DataServices
    {
        private static Repositories repositorie = Repositories.Instance;

        public static IAlbumsService GetAlbumService()
        {
            return new AlbumsService(
                repositorie.GetAlbumsRepository(),
                repositorie.GetUsersRepository());
        }

        public static IImagesService GetImagesService()
        {
            return new ImagesService(
                repositorie.GetImagesRepository(),
                repositorie.GetUsersRepository(),
                repositorie.GetAlbumsRepository(),
                LogicServices.GetImageProcessorService(),
                CommonServices.GetGoogleDriveService(),
                CommonServices.GetNotifyService());
        }

        public static ITagsService GetTagsService()
        {
            return new TagsService(repositorie.GetTagsRepository());
        }
    }
}

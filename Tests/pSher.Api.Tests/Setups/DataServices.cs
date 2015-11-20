namespace PSher.Api.Tests.Setups
{
    using PSher.Services.Data;
    using PSher.Services.Data.Contracts;

    public static class DataServices
    {
        public static IAlbumsService GetAlbumService()
        {
            return new AlbumsService(
                Repositories.GetAlbumsRepository(),
                Repositories.GetUsersRepository());
        }

        public static IImagesService GetImagesService()
        {
            return new ImagesService(
                Repositories.GetImagesRepository(),
                Repositories.GetUsersRepository(),
                Repositories.GetAlbumsRepository(),
                LogicServices.GetImageProcessorService(),
                CommonServices.GetGoogleDriveService(),
                CommonServices.GetNotifyService());
        }


        public static ITagsService GetTagsService()
        {
            return new TagsService(Repositories.GetTagsRepository());
        }
    }
}

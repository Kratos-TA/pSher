namespace PSher.Api.Tests.Constants
{
    using Common.Constants;

    public class TestsConstants
    {
        // To be sure when testing for non private items that there enough items to fill default page size
        public const int EntitiesPerRepesitory = GlobalConstants.DefaultPageSize * 3; 
        public const int TagsPerImage = 3;
        public const int TagsPerAlbum = 5;
        public const int AlbumsPerImage = 2;
        public const int ImagesPerAlbum = 10;
        public const int ImagesPerUser = 10;
        public const int AlbumsPerUser = 3;
        public const int CommentsPerImage = 5;
        public const int MarksPerImage = 5;
        public const int ImagesPerTag = TagsPerImage * 2;
        public const int AlbumPerTag = TagsPerAlbum * 2;
        public const int CommonModulDivisor = 5;

        public const string ImageBaseTitle = "Some Image ";
        public const string UserBaseUserName = "Some UserName ";
        public const string UserBaseFirstName = "Some FirstName ";
        public const string UserBaseLastName = "Some LastName ";
        public const string UserBaseEmail = "some-mail-{0}@pesho.net";
        public const string AlbumBaseName = "Some Album ";
        public const string DescriptionBaseTextr = "Some Description ";
        public const string ImageBasyeUrl = "http://someurl.com/{0}.jpg";
        public const string ImageBaseThumbnailUrl = "http://someurl.com/{0}-thumbnail.jpg";
        public const string ImageBaseExstnsion = ".jpg";

        public const string ImageInfoBaseOriginalName = "some-image-name";
        public const string CommentBaseText = "Some comment text ";
        public const string TagBaseName = "Some Tag ";
    }
}

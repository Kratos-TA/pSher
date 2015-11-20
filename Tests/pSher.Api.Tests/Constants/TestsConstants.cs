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
    }
}

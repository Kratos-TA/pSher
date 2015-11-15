namespace PSher.Api.DataTransferModels.Images
{
    using PSher.Api.Infrastructure.Mapping;
    using PSher.Models;

    public class ImageSimpleResponseModel : IMapFrom<Image>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsPrivate { get; set; }

        public string DropboxUrl { get; set; }
    }
}
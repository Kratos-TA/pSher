namespace PSher.Api.DataTransferModels.Album
{
    using System.ComponentModel.DataAnnotations;

    using PSher.Api.Validation;
    using PSher.Common.Constants;

    public class UpdateAlbumRequestModel
    {
        [StringInCommaSepatatedCollectionLength(ValidationConstants.MinTagName, ValidationConstants.MaxTagName, "Tags")]
        public string Tags { get; set; }

        [IntegerInCommaSepatatedCollection("ImagesIds")]
        public string ImagesIds { get; set; }

        public bool? IsPrivate { get; set; }
        
        [MinLength(ValidationConstants.MinAlbumName)]
        [MaxLength(ValidationConstants.MaxAlbumName)]
        public string Name { get; set; }
    }
}
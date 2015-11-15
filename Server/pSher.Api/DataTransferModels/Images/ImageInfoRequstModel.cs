namespace PSher.Api.DataTransferModels.Images
{
    using System.ComponentModel.DataAnnotations;
    using PSher.Common.Constants;
    using PSher.Common.Models;

    public class ImageInfoRequstModel
    {
        public  RawFile ToRawFile()
        {
            return new RawFile()
            {
                OriginalFileName = this.OriginalName,
                FileExtension = this.OriginalExtension,
                Content = this.ByteArrayContent
            };
        }

        [Required]
        [MaxLength(ValidationConstants.MaxImageInfoOriginalName)]
        public string OriginalName { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MaxImageInfoFileExtension)]
        [MinLength(ValidationConstants.MaxImageInfoFileExtension)]
        public string OriginalExtension { get; set; }

        [Required]
        public byte[] ByteArrayContent { get; set; }
    }
}
namespace PSher.Api.DataTransferModels.Images
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using PSher.Common.Constants;
    using PSher.Common.Models;

    public class ImageInfoRequstModel
    {
    
        [Required]
        [MaxLength(ValidationConstants.MaxImageInfoOriginalName)]
        public string OriginalName { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MaxImageInfoFileExtension)]
        [MinLength(ValidationConstants.MinImageInfoFileExtension)]
        public string OriginalExtension { get; set; }

        [Required]
        public string Base64Content { get; set; }

        public RawFile ToRawFile()
        {
            return new RawFile()
            {
                OriginalFileName = this.OriginalName,
                FileExtension = this.OriginalExtension,
                Content = this.ByteArrayContent
            };
        }

        public byte[] ByteArrayContent => Convert.FromBase64String(this.Base64Content);
    }
}
﻿namespace PSher.Api.DataTransferModels.Images
{
    using System.ComponentModel.DataAnnotations;
    using PSher.Api.Validation;
    using PSher.Common.Constants;

    public class SaveImageRequestModel
    {
        [StringInCommaSepatatedCollectionLength(ValidationConstants.MinTagName, ValidationConstants.MaxTagName, "Tags")]
        public string Tags { get; set; }
        
        public string Albums { get; set; }

        [Required]
        [MinLength(ValidationConstants.MinImageTitle)]
        [MaxLength(ValidationConstants.MaxImageTitle)]
        public string Title { get; set; }

        [MaxLength(ValidationConstants.MaxImageDescription)]
        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public ImageInfoRequstModel ImageInfo { get; set; }
    }
}
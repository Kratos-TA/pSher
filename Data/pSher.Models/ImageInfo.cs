namespace PSher.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PSher.Common.Constants;

    public class ImageInfo
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MaxImageInfoOriginalName)]
        public string OriginalName { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MaxImageInfoFileExtension)]
        [MinLength(ValidationConstants.MinImageInfoFileExtension)]
        public string OriginalExtension { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}

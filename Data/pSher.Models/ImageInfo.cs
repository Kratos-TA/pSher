namespace PSher.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ImageInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(248)]
        public string OriginalName { get; set; }

        [Required]
        [MaxLength(6)]
        public string OriginalExtension { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}

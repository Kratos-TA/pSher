namespace PSher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
        private ICollection<Album> albums;

        public Image()
        {
            this.albums = new HashSet<Album>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public virtual User Author { get; set; }

        [Required]
        [MaxLength(2000)]
        public string DropboxUrl { get; set; }

        public DateTime UploadedOn { get; set; }

        public int RatingId { get; set; }

        public virtual Rating Rating { get; set; }

        public int DiscussionId { get; set; }

        public Discussion Discussion { get; set; }

        public int ImageInfoId { get; set; }

        public virtual ImageInfo ImageInfo { get; set; }

        public virtual ICollection<Album> Albums
        {
            get { return this.albums; }
            set { this.albums = value; }
        }
    }
}

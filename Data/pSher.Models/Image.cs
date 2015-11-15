 namespace PSher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PSher.Common.Constants;

    public class Image
    {
        private ICollection<Album> albums;
        private ICollection<Comment> comments;
        private ICollection<Tag> tags;

        public Image()
        {
            this.albums = new HashSet<Album>();
            this.comments = new HashSet<Comment>();
            this.tags = new HashSet<Tag>();
            this.Rating = new Rating();
        }

        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [MinLength(ValidationConstants.MinImageTitle)]
        [MaxLength(ValidationConstants.MaxImageTitle)]
        public string Title { get; set; }

        [MaxLength(ValidationConstants.MaxImageDescription)]
        public string Description { get; set; }

        public bool IsPrivate { get; set; }
        
        [Required]
        public virtual User Author { get; set; }

        // [Required]
        [MaxLength(ValidationConstants.MaxImageDropBoxUrlLength)]
        public string DropboxUrl { get; set; }

        public DateTime UploadedOn { get; set; }

        public virtual Rating Rating { get; set; }

        public virtual ImageInfo ImageInfo { get; set; }

        public virtual ICollection<Album> Albums
        {
            get { return this.albums; }
            set { this.albums = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Tag> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }
    }
}

namespace PSher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
        private ICollection<Album> albums;
        private ICollection<Tag> tags;

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
        public virtual User Author { get; set; }

        [Required]
        [MaxLength(2000)]
        public string DropboxUrl { get; set; }

        public DateTime UploadedOn { get; set; }

        public virtual Rating Rating { get; set; }
        
        public Discussion Discussion { get; set; }

        public virtual ImageInfo ImageInfo { get; set; }

        public virtual ICollection<Album> Albums
        {
            get { return this.albums; }
            set { this.albums = value; }
        }

        public virtual ICollection<Tag> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }
    }
}

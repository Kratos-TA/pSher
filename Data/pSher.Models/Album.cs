namespace PSher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Album
    {
        private ICollection<Album> albums;
        private ICollection<Image> images;

        public Album()
        {
            this.albums = new HashSet<Album>();
            this.images = new HashSet<Image>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Album> Albums
        {
            get { return this.albums; }
            set { this.albums = value; }
        }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }
    }
}

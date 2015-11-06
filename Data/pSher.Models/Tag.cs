namespace PSher.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PSher.Common.Constants;

    public class Tag
    {
        private ICollection<Image> images;
        private ICollection<Album> albums;

        public Tag()
        {
            this.images = new HashSet<Image>();
            this.albums = new HashSet<Album>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.MinTagName)]
        [MaxLength(ValidationConstants.MaxTagName)]
        public string Name { get; set; }      

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public virtual ICollection<Album> Albums
        {
            get { return this.albums; }
            set { this.albums = value; }
        }
    }
}

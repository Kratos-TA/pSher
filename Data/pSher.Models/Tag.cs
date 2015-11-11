namespace PSher.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PSher.Common.Constants;

    public class Tag
    {
        private string name;
        private ICollection<Image> images;
        private ICollection<Album> albums;

        public Tag()
        {
            this.images = new HashSet<Image>();
            this.albums = new HashSet<Album>();
        }

        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [MinLength(ValidationConstants.MinTagName)]
        [MaxLength(ValidationConstants.MaxTagName)]
        [Index(IsUnique = true)]
        public string Name
        {
            get { return this.name; }
            set { this.name = value.ToLower(); }
        }      

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

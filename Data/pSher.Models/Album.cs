namespace PSher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PSher.Common.Constants;

    public class Album
    {
        private ICollection<Image> images;
        private ICollection<Tag> tags;

        public Album()
        {
            this.images = new HashSet<Image>();
            this.tags = new HashSet<Tag>();
        }

        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPrivate { get; set; }

        [Required]
        [MinLength(ValidationConstants.MinAlbumName)]
        [MaxLength(ValidationConstants.MaxAlbumName)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public virtual ICollection<Tag> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }
    }
}

namespace PSher.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Gallery
    {
        private ICollection<Album> albums;

        public Gallery()
        {
            this.albums = new HashSet<Album>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Album> Albums
        {
            get { return this.albums; }
            set { this.albums = value; }
        }
    }
}

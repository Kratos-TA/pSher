namespace PSher.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PSher.Common.Constants;

    public class Photostream
    {
        private ICollection<Image> images;

        public Photostream()
        {
            this.images = new HashSet<Image>();
        }

        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }
    }
}

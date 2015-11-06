namespace PSher.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        private ICollection<Image> images;

        public Tag()
        {
            this.images = new HashSet<Image>();
        }

        [Key]
        public int Id { get; set; }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }
    }
}

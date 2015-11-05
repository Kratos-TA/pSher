namespace PSher.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Rating
    {
        private ICollection<Mark> marks;
        
        public Rating()
        {
            this.marks = new HashSet<Mark>();
        }

        [Key]
        public int Id { get; set; }

        public int ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<Mark> Marks
        {
            get { return this.marks; }
            set { this.marks = value; }
        }
    }
}

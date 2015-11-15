using System.Linq;

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

        public bool IsDeleted { get; set; }

        public virtual ICollection<Mark> Marks
        {
            get { return this.marks; }
            set { this.marks = value; }
        }
    }
}

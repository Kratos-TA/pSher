namespace PSher.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Discussion
    {
        private ICollection<Comment> comments;

        public Discussion()
        {
            this.comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }
    }
}

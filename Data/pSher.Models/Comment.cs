namespace PSher.Models
{
    using System.ComponentModel.DataAnnotations;

    using PSher.Common.Constants;

    public class Comment
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public virtual User Author { get; set; }

        [Required]
        [MinLength(ValidationConstants.MinCommentText)]
        [MaxLength(ValidationConstants.MaxCommentText)]
        public string Text { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }
    }
}
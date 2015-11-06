namespace PSher.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public virtual User Author { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(248)]
        public string Text { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }
    }
}
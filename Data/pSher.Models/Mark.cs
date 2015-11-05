namespace PSher.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Mark
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Mark value must be between 1 and 5")]
        public int Value { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}

namespace PSher.Models
{
    using System.ComponentModel.DataAnnotations;

    using PSher.Common.Constants;

    public class Mark
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [Range(ValidationConstants.MinMarkValue, ValidationConstants.MaxMarkValue, ErrorMessage = ErrorMessages.MarkLength)]
        public int Value { get; set; }

        public virtual User GivenBy { get; set; }
    }
}

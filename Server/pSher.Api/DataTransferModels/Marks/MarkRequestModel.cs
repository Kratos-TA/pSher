namespace PSher.Api.DataTransferModels.Marks
{
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    public class MarkRequestModel
    {
        [Required]
        public string AuthorUserName { get; set; }

        [Required]
        public int ImageId { get; set; }

        [Required]
        [Range(ValidationConstants.MinMarkValue, ValidationConstants.MinMarkValue, ErrorMessage = ErrorMessages.MarkLength)]
        public int Value { get; set; }
    }
}
namespace PSher.Api.DataTransferModels.Marks
{
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    public class MarkRequestModel
    {
        [Required]
        public int ImageId { get; set; }

        [Required]
        [Range(ValidationConstants.MinMarkValue, ValidationConstants.MaxMarkValue, ErrorMessage = ErrorMessages.MarkLength)]
        public int Value { get; set; }
    }
}
namespace PSher.Api.DataTransferModels.Users
{
    using System.ComponentModel.DataAnnotations;
    using PSher.Common.Constants;

    public class UpdateUserRequestModel
    {
        public string Email { get; set; }

        [MaxLength(ValidationConstants.MaxUserRealName)]
        [MinLength(ValidationConstants.MinUserRealName)]
        public string FirstName { get; set; }

        [MaxLength(ValidationConstants.MaxUserRealName)]
        [MinLength(ValidationConstants.MinUserRealName)]
        public string LastName { get; set; }
    }
}
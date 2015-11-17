using System;

namespace PSher.Common.Constants
{
    public class ErrorMessages
    {
        public const string MarkLength = "Mark value must be between 1 and 5!";

        // Services.Data
        public const string InvalidUser = "Current user cannot be found!";

        // Api
        public const string PropertyNameLength = "The {0} length must be between {1} and {2} simbols!";
        public const string IntegerId = "The {0} is invalid Id, could not be parserd to integer!";
        public const string InvalidRequestModel = "The request model of {0} is invalid!";
        public const string RequestCannotBeEmpty = "Request cannot by empty!";
    }
}

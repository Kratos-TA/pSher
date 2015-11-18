namespace PSher.Common.Constants
{
    public class ErrorMessages
    {
        public const string MarkLength = "Mark value must be between 1 and 5!";

        // Services.Data
        public const string InvalidUser = "Current user cannot be found!";

        // Api
        public const string PropertyNameLength = "The {0} length must be between {1} and {2} symbols!";
        public const string IntegerId = "The {0} is invalid Id, could not be parsed to integer!";
        public const string InvalidRequestModel = "Invalid request model, there is a big chance the problem is in you. Check you JSON string";
        public const string RequestCannotBeEmpty = "Request cannot by empty!";

        public const string UnoutorizedAccess =
            "The {0} you want to access is private and authentication by its owner is required to be accessed!";
    }
}

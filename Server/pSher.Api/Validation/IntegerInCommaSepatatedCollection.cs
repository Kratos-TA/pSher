namespace PSher.Api.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using PSher.Common.Constants;

    public class IntegerInCommaSepatatedCollection : ValidationAttribute
    {
        private readonly string propertyName;

        public IntegerInCommaSepatatedCollection(string propertyName)
        {
            this.propertyName = propertyName;
            this.ErrorMessage = ErrorMessages.PropertyNameLength;
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            if (!string.IsNullOrWhiteSpace(valueAsString))
            {
                var integers = valueAsString
                    .Split(new[] { GlobalConstants.CommaSeparatedCollectionSeparator }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                foreach (var i in integers)
                {
                    int ussles;
                    bool isInteger = int.TryParse(i, out ussles);
                    if (!isInteger)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessage, this.propertyName);
        }
    }
}
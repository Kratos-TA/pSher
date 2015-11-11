namespace PSher.Api.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using PSher.Common.Constants;

    public class StringInCommaSepatatedCollectionLength : ValidationAttribute
    {
        private readonly int minimumLength;
        private readonly int maximumLength;

        public StringInCommaSepatatedCollectionLength(int minimumLength, int maximumLength)
        {
            this.minimumLength = minimumLength;
            this.maximumLength = maximumLength;
            this.ErrorMessage = ErrorMessages.TagNamesLength;
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            if (!string.IsNullOrWhiteSpace(valueAsString))
            {
                var tags = valueAsString
                    .Split(new[] { GlobalConstants.CommaSeparatedCollectionSeparator }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                return tags.All(tag => this.minimumLength <= tag.Length && tag.Length <= this.maximumLength);
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessage, this.minimumLength, this.maximumLength);
        }
    }
}
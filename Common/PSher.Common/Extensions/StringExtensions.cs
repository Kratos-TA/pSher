namespace PSher.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PSher.Common.Constants;

    public static class StringExtensions
    {
        public static IEnumerable<string> GetEnumerableFromCommSeparatedString(this string commaSeparatedString)
        {
            if (string.IsNullOrWhiteSpace(commaSeparatedString))
            {
                return Enumerable.Empty<string>();
            }

            var values = new HashSet<string>();

            commaSeparatedString.Split(new[] { GlobalConstants.CommaSeparatedCollectionSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(v =>
                {
                    values.Add(v.ToLower().Trim());
                });

            return values;
        }
    }
}

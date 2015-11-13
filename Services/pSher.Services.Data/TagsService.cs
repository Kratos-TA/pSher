namespace PSher.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using PSher.Common.Constants;
    using PSher.Common.Extensions;
    using PSher.Data.Contracts;
    using PSher.Models;
    using PSher.Services.Data.Contracts;

    public class TagsService : ITagsService
    {
        private readonly IRepository<Tag> tags;

        public TagsService(IRepository<Tag> tags)
        {
            this.tags = tags;
        }

        public async Task<IEnumerable<Tag>> TagsFromCommaSeparatedValues(string tagsAsCommaSeparatedValues)
        {
            if (string.IsNullOrWhiteSpace(tagsAsCommaSeparatedValues))
            {
                return Enumerable.Empty<Tag>();
            }

            var tagNames = new HashSet<string>();

            tagsAsCommaSeparatedValues.Split(new[] { GlobalConstants.CommaSeparatedCollectionSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(tag =>
                {
                    tagNames.Add(tag.ToLower().Trim());
                });

            var resultTags = await this.tags
                .All()
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync();

            (await this.tags
                .All()
                .Where(t => tagNames.Contains(t.Name.ToLower()))
                .Select(t => t.Name.ToLower())
                .ToListAsync())
                .ForEach(t => tagNames.Remove(t));

            tagNames.ForEach(tagName => resultTags.Add(new Tag { Name = tagName }));

            return resultTags;
        }
    }
}

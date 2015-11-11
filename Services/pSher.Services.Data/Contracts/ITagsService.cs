namespace PSher.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PSher.Models;

    public interface ITagsService
    {
        Task<IEnumerable<Tag>> TagsFromCommaSeparatedValues(string tagsAsCommaSeparatedValues);
    }
}

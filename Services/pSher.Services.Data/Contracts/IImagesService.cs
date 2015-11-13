namespace PSher.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PSher.Common.Constants;
    using PSher.Models;

    public interface IImagesService : IService
    {
        Task<IEnumerable<Image>> ImagesFromCommaSeparatedIds(string tagsAsCommaSeparatedValues);

        IQueryable<Image> All(int page = 1, int pageSize = GlobalConstants.DefaultPageSize);

        IQueryable<Image> GetImageById(int id);

        bool Update(int id, string title, string description, IEnumerable<Tag> tags);

        bool DeleteImage(int id);

        // TODO: Add ImageInfo as method parameter
        // TODO: Add albums
        Task<int> Add(string title, string authorUserName, string description, bool isPrivate, IEnumerable<Tag> tags = null, IDictionary<string, DateTime> albums = null);
    }
}

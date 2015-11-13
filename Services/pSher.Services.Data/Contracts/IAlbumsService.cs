namespace PSher.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PSher.Common.Constants;
    using PSher.Models;

    public interface IAlbumsService
    {
        IQueryable<Album> All(int page = 1, int pageSize = GlobalConstants.DefaultPageSize);

        IQueryable<Album> GetAlbumById(int id);

        // IQueryable<Album> GetAllByName(string name);

        // IQueryable<Album> GetAllByTag(string tagName);

        // IQueryable<Album> GetAllByCreatedOn(DateTime createdOn);

        Task<int> Add(string name, string authorUserName, bool isPrivate, IEnumerable<Tag> albumTags = null, IEnumerable<Image> albumImages = null);
    }
}

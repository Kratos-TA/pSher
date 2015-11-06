namespace PSher.Services.Data.Contracts
{
    using System;
    using System.Linq;

    using PSher.Common.Constants;
    using PSher.Models;

    public interface IAlbumsService
    {
        IQueryable<Album> All(int page = 1, int pageSize = GlobalConstants.DefaultPageSize);

        IQueryable<Album> GetAllByUserName(string userName);

        IQueryable<Album> GetAllByName(string name);

        IQueryable<Album> GetAllByTag(string tagName);

        IQueryable<Album> GetAllByCreatedOn(DateTime createdOn);
    }
}

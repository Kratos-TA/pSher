namespace PSher.Services.Data.Contracts
{
    using System;
    using System.Linq;

    using PSher.Models;
    using PSher.Common.Constants;

    public interface IImagesService
    {
        IQueryable<Image> All(int page = 1, int pageSize = GlobalConstants.DefaultPageSize);

        IQueryable<Image> GetAllByUserName(string userName);

        IQueryable<Image> GetAllByTitle(string title);

        IQueryable<Image> GetAllByTag(string tagName);

        IQueryable<Image> GetAllByUlopadedOn(DateTime uploadedOn);
    }
}

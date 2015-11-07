namespace PSher.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PSher.Common.Constants;
    using PSher.Models;

    public interface IImagesService
    {
        IQueryable<Image> All(int page = 1, int pageSize = GlobalConstants.DefaultPageSize);

        IQueryable<Image> GetAllByUserName(string userName);

        IQueryable<Image> GetAllByTitle(string title);

        IQueryable<Image> GetAllByTag(string tagName);

        IQueryable<Image> GetAllByUlopadedOn(DateTime uploadedOn);

        // TODO: Add ImageInfo as method parameter
        // TODO: Add albums
        int Add(string title, string authorUserName, string description, bool isPrivate, ICollection<string> tags = null);
    }
}

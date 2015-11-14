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
        IQueryable<Album> All(
            int page = 1,
            int pageSize = GlobalConstants.DefaultPageSize,
            bool isAutorised = false,
            string authenticatedUserName = "");

        IQueryable<Album> GetAlbumById(
            int id,
            bool isAutorizedAcces,
            string userName);

        Task<int> Add(
            string name,
            string authorUserName,
            bool isPrivate,
            IEnumerable<Tag> albumTags = null,
            IEnumerable<Image> albumImages = null);

        Task<int> Update(
            int id,
            string name,
            string authenticatedUserName,
            bool isPrivate,
            IEnumerable<Tag> albumTags = null,
            IEnumerable<Image> albumImages = null);

        Task<int> Delete(int id);

        IQueryable<Album> AllByParamethers(
            string albumName,
            string albumCreator,
            IEnumerable<string> tags,
            int page, 
            int pageSize,
            bool isAuthorizedAccess,
            string authenticatedUserName);
    }
}

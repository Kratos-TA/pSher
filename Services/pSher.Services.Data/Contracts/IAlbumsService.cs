namespace PSher.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PSher.Common.Constants;
    using PSher.Models;

    public interface IAlbumsService
    {
        Task<IEnumerable<Album>> AlbumsFromCommaSeparatedValuesAndUserId(string albumsAsCommaSeparatedValues, string userId);

        IQueryable<Album> All(
            int page = 1,
            int pageSize = GlobalConstants.DefaultPageSize,
            bool isAuthorizedAccess = false,
            string authenticatedUserId = "");

        IQueryable<Album> GetAlbumById(int id);

        Task<int> Add(
            string name,
            string authorUserName,
            bool isPrivate,
            IEnumerable<Tag> albumTags = null,
            IEnumerable<Image> albumImages = null);

        Task<int> Update(
            int id,
            string name = null,
            bool? isPrivate = null,
            IEnumerable<Tag> albumTags = null,
            IEnumerable<Image> albumImages = null);

        Task<int> Delete(int id);

        IQueryable<Album> AllByParamethers(
            string albumName,
            string albumCreator,
            IEnumerable<string> tags,
            int page, 
            int pageSize,
            bool isAuthorizedAccess = false,
            string authenticatedUserId = "");

        Task<string> GetAlbumCreatorIdById(int id);
    }
}

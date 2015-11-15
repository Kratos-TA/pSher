namespace PSher.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PSher.Common.Constants;
    using PSher.Common.Models;
    using PSher.Models;

    public interface IImagesService : IService
    {
        Task<IEnumerable<Image>> ImagesFromCommaSeparatedIds(string tagsAsCommaSeparatedValues);

        Task<RawFile> ProcessImage(RawFile rawImages);

        IQueryable<Image> All(
            int page = 1,
            int pageSize = GlobalConstants.DefaultPageSize,
            bool isAuthorizedAccess = false,
            string authenticatedUserId = "");

        IQueryable<Image> AllByParamethers(
            string imageTitle,
            string imageAuthor,
            IEnumerable<string> imageTags,
            int page,
            int pageSize,
            bool isAuthorizedAccess = false,
            string authenticatedUserId = "");

        IQueryable<Image> GetImageById(
            int id,
            bool isAuthorizedAccess,
            string authenticatedUserId);

        Task<int> Update(
            int id, 
            string title, 
            string description, 
            IEnumerable<Tag> tags);

        Task<int> DeleteImage(int id);
       
        Task<int> Add(
            string title,
            string autenticatedUserId,
            string description,
            bool isPrivate,
            RawFile rawImage,
            IEnumerable<Tag> imageTags = null,
            IEnumerable<Album> imageAlbums = null);
    }
}

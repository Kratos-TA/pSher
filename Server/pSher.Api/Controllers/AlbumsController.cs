namespace PSher.Api.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNet.Identity;
    using PSher.Api.DataTransferModels.Album;
    using PSher.Common.Constants;
    using PSher.Common.Extensions;
    using PSher.Services.Data.Contracts;

    [RoutePrefix("api/albums")]
    public class AlbumsController : ApiController
    {
        private readonly IAlbumsService albumsService;
        private readonly ITagsService tagsService;
        private readonly IImagesService imagesService;

        public AlbumsController(IAlbumsService albumsService, ITagsService tagsService, IImagesService imagesService)
        {
            this.albumsService = albumsService;
            this.tagsService = tagsService;
            this.imagesService = imagesService;
        }

        public async Task<IHttpActionResult> Get(string name, string user = null, string tags = null, int page = 1, int pageSize = 10)
        {
            var isAutorisedAcces = this.User.Identity.IsAuthenticated;
            var currentUser = this.User.Identity.Name;
            var selectedTags = tags.GetEnumerableFromCommSeparatedString();

            var result = await this.albumsService
                .AllByParamethers(name, user, selectedTags, page, pageSize, isAutorisedAcces, currentUser)
                .ProjectTo<AlbumResponseModel>()
                .ToListAsync();

            return this.Ok(result);
        }

        public async Task<IHttpActionResult> Get(int page = 1, int pageSize = 10)
        {
            var isAutorisedAcces = this.User.Identity.IsAuthenticated;
            var currentUserId = this.User.Identity.GetUserId();

            var result = await this.albumsService
                .All(page, pageSize, isAutorisedAcces, currentUserId)
                .ProjectTo<AlbumResponseModel>()
                .ToListAsync();

            return this.Ok(result);
        }

        public IHttpActionResult Get(string id)
        {
            var isAutorisedAcces = this.User.Identity.IsAuthenticated;
            var currentUserId = this.User.Identity.GetUserId();

            var result = this.albumsService
                .GetAlbumById(int.Parse(id), isAutorisedAcces, currentUserId)
                .ProjectTo<AlbumDetailsResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        [Authorize]
        [EnableCors("*", "*", "*")]
        public async Task<IHttpActionResult> Post(SaveAlbumRequestModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                return this.BadRequest(string.Format(ErrorMessages.InvalidRequestModel, "SaveAlbumRequestModel"));
            }

            var currentUserId = this.User.Identity.GetUserId();
            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);
            var images = await this.imagesService.ImagesFromCommaSeparatedIds(model.ImagesIds);

            var addedAlbumId = await this.albumsService.Add(
                model.Name,
                currentUserId,
                model.IsPrivate,
                tags,
                images);

            return this.Ok(addedAlbumId);
        }

        [Authorize]
        [EnableCors("*", "*", "*")]
        public async Task<IHttpActionResult> Put(int id, UpdateAlbumRequestModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                return this.BadRequest(string.Format(ErrorMessages.InvalidRequestModel, "UpdateAlbumRequestModel"));
            }

            var currentUserId = this.User.Identity.GetUserId();
            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);
            var images = await this.imagesService.ImagesFromCommaSeparatedIds(model.ImagesIds);

            var changedAlbumId = await this.albumsService.UpdateAll(
                id,
                model.Name,
                currentUserId,
                model.IsPrivate,
                tags,
                images);

            return this.Ok(changedAlbumId);
        }

        [Authorize]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();

            var deletedAlbumId = await this.albumsService.Delete(id, currentUserId);

            return this.Ok(deletedAlbumId);
        }
    }
}
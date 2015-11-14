namespace PSher.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using AutoMapper.QueryableExtensions;
    using DataTransferModels.Album;
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

        public IHttpActionResult Get(string name, string user = null, string tags = null, int page = 1, int pageSize = 10)
        {
            var isAutorisedAcces = this.User.Identity.IsAuthenticated;
            var currentUser = this.User.Identity.Name;
            var selectedTags = tags.GetEnumerableFromCommSeparatedString();

            var result = this.albumsService
                .AllByParamethers(name, user, selectedTags, page, pageSize, isAutorisedAcces, currentUser)
                .ProjectTo<AlbumResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        public IHttpActionResult Get(int page = 1, int pageSize = 10)
        {
            var isAutorisedAcces = this.User.Identity.IsAuthenticated;
            var currentUser = this.User.Identity.Name;

            var result = this.albumsService
                .All(page, pageSize, isAutorisedAcces, currentUser)
                .ProjectTo<AlbumResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        public IHttpActionResult Get(string id)
        {
            var isAutorisedAcces = this.User.Identity.IsAuthenticated;
            var currentUser = this.User.Identity.Name;

            var result = this.albumsService
                .GetAlbumById(int.Parse(id), isAutorisedAcces, currentUser)
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

            var autenticatedUserName = this.User.Identity.Name;
            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);
            var images = await this.imagesService.ImagesFromCommaSeparatedIds(model.ImagesIds);

            var addedAlbumId = await this.albumsService.Add(
                model.Name,
                autenticatedUserName,
                model.IsPrivate,
                tags,
                images);

            return this.Ok(addedAlbumId);
        }

        [Authorize]
        [EnableCors("*", "*", "*")]
        public async Task<IHttpActionResult> Put(int id, SaveAlbumRequestModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                return this.BadRequest(string.Format(ErrorMessages.InvalidRequestModel, "SaveAlbumRequestModel"));
            }

            var authenticatedUser = this.User.Identity.Name;
            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);
            var images = await this.imagesService.ImagesFromCommaSeparatedIds(model.ImagesIds);

            var changedAlbumId = await this.albumsService.Update(
                id,
                model.Name,
                authenticatedUser,
                model.IsPrivate,
                tags,
                images);

            return this.Ok(changedAlbumId);
        }

        public IHttpActionResult Delete(int id)
        {
            var deletedAlbumId = this.albumsService.Delete(id);

            return this.Ok(deletedAlbumId);
        }
    }
}
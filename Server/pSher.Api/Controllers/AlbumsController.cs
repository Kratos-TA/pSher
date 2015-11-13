namespace PSher.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using AutoMapper.QueryableExtensions;
    using PSher.Common.Constants;
    using DataTransferModels.Album;
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

        public IHttpActionResult Get()
        {
            var result = this.albumsService
                .All()
                .ProjectTo<AlbumResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        public IHttpActionResult Get(int id)
        {
            var result = this.albumsService
                .GetAlbumById(id)
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

        //public IHttpActionResult Put()
        //{

        //}

        //public IHttpActionResult Delete()
        //{

        //}
    }
}
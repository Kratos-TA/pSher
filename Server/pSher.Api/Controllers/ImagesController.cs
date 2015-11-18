namespace PSher.Api.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Web.Http;

    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNet.Identity;
    using PSher.Api.DataTransferModels.Images;
    using PSher.Api.Validation;
    using PSher.Common.Constants;
    using PSher.Common.Extensions;
    using PSher.Services.Data.Contracts;
    
    [RoutePrefix("api/images")]
    public class ImagesController : ApiController
    {
        private readonly IAlbumsService albumssService;
        private readonly IImagesService imagesService;
        private readonly ITagsService tagsService;

        public ImagesController(
            IAlbumsService albumssService,
            IImagesService imagesService,
            ITagsService tagsService)
        {
            this.imagesService = imagesService;
            this.tagsService = tagsService;
            this.albumssService = albumssService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(
            string name,
            string user = null,
            string tags = null,
            int page = 1,
            int pageSize = 10)
        {
            var isAuthorizedAccess = this.User.Identity.IsAuthenticated;
            var currentUserId = this.User.Identity.GetUserId();
            var selectedTags = tags.GetEnumerableFromCommSeparatedString();

            var result = await this.imagesService
                .AllByParamethers(name, user, selectedTags, page, pageSize, isAuthorizedAccess, currentUserId)
                .ProjectTo<ImageResponseModel>()
                .ToListAsync();

            return this.Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int page = 1, int pageSize = 10)
        {
            var isAuthorizedAccess = this.User.Identity.IsAuthenticated;
            var currentUserId = this.User.Identity.GetUserId();

            var result = await this.imagesService
                .All(page, pageSize, isAuthorizedAccess, currentUserId)
                .ProjectTo<ImageResponseModel>()
                .ToListAsync();

            return this.Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            var isAuthorizedAccess = this.User.Identity.IsAuthenticated;
            var currentUserId = this.User.Identity.GetUserId();

            var resultImage = await this.imagesService
                .GetImageById(int.Parse(id), isAuthorizedAccess, currentUserId)
                .ProjectTo<ImageResponseModel>()
                .FirstOrDefaultAsync();

            if (resultImage == null)
            {
                return this.NotFound();
            }

            if (resultImage.IsPrivate
               && !isAuthorizedAccess
               && currentUserId == null
                   || currentUserId != resultImage.AuthorId)
            {
                return this.Unauthorized();
            }

            return this.Ok(resultImage);
        }

        [HttpPut]
        [Authorize]
        [ValidateModel]
        public async Task<IHttpActionResult> Put(int id, SaveImageRequestModel model)
        {
            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);

            var isUpdated = await this.imagesService
                .Update(
                id,
                model.Title,
                model.Description,
                tags);

            if (isUpdated != 0)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest("Invalid id");
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var isDeleted = await this.imagesService.DeleteImage(id);

            if (isDeleted != 0)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest("Invalid id");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateModel]
        public async Task<IHttpActionResult> Post(SaveImageRequestModel model)
        {
            var autenticatedUserId = this.User.Identity.GetUserId();
            var imageTags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);
            var imageAlbums = await this.albumssService.AlbumsFromCommaSeparatedValuesAndUserId(model.Albums, autenticatedUserId);
            var resizedRawFile = model.ImageInfo.ToRawFile();  // await this.imagesService.ProcessImage(model.ImageInfo.ToRawFile());

            var addedImageId = await this.imagesService.Add(
                model.Title,
                autenticatedUserId,
                model.Description,
                model.IsPrivate,
                resizedRawFile,
                imageTags,
                imageAlbums);

            return this.Ok(addedImageId);
        }
    }
}
namespace PSher.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    
    using AutoMapper.QueryableExtensions;
    using PSher.Api.DataTransferModels.Images;
    using PSher.Common.Constants;
    using PSher.Services.Data.Contracts;

    [RoutePrefix("api/images")]
    public class ImagesController : ApiController
    {
        private readonly IImagesService imagesService;
        private readonly ITagsService tagsService;

        public ImagesController(
            IImagesService imagesService,
            ITagsService tagsService)
        {
            this.imagesService = imagesService;
            this.tagsService = tagsService;
        }

        [EnableCors("*", "*", "*")]
        public IHttpActionResult Get(int page, int pageSize)
        {
            var result = this.imagesService
                .All(page, pageSize)
                .ProjectTo<ImageResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        [EnableCors("*", "*", "*")]
        public IHttpActionResult Get(int id)
        {
            var result = this.imagesService
                .GetImageById(id)
                .ProjectTo<ImageResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        [EnableCors("*", "*", "*")]
        public IHttpActionResult Get()
        {
            var result = this.imagesService
                .All()
                .ProjectTo<ImageResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        [EnableCors("*", "*", "*")]
        [Authorize]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, SaveImageRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);

            var isUpdated = this.imagesService
                .Update(
                id,
                model.Title,
                model.Description,
                tags);

            if (isUpdated)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest("Invalid id");
            }
        }

        public IHttpActionResult Delete(int id)
        {
            bool isDeleted = this.imagesService.DeleteImage(id);

            if (isDeleted)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest("Invalid id");
            }
        }

        [Authorize]
        [EnableCors("*", "*", "*")]
        public async Task<IHttpActionResult> Post(SaveImageRequestModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                return this.BadRequest(string.Format(ErrorMessages.InvalidRequestModel, "SaveAlbumRequestModel"));
            }

            var autenticatedUserName = this.User.Identity.Name;
            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);

            var addedImageId = await this.imagesService.Add(
                model.Title,
                autenticatedUserName,
                model.Description,
                model.IsPrivate,
                tags);

            return this.Ok(addedImageId);
        }
    }
}
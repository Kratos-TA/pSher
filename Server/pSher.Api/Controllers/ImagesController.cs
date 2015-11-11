namespace PSher.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    
    using AutoMapper.QueryableExtensions;
    using PSher.Api.DataTransferModels.Images;
    using PSher.Services.Data.Contracts;

    [RoutePrefix("api/Images")]
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
        public IHttpActionResult Get()
        {
            var result = this.imagesService
                .All()
                .ProjectTo<ImageResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        public async Task<IHttpActionResult> Post(SaveImageRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);

            var addedImageId = await this.imagesService.Add(
                model.Title,
                model.AuthorUserName,
                model.Description,
                model.IsPrivate,
                tags);

            return this.Ok(addedImageId);
        }

        [HttpGet]
        [Route("api/ImagesByUsername")]
        public IHttpActionResult GetImagesByUsername(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return this.BadRequest("Username cannot be null or empty!");
            }

            var imagesByUser = this.imagesService.GetAllByUserName(userName);

            return this.Ok(imagesByUser);
        }

        [HttpGet]
        [Route("api/ImagesByTitle")]
        public IHttpActionResult GetImagesByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return this.BadRequest("Title cannot be null or empty!");
            }

            var imagesByTitle = this.imagesService.GetAllByTitle(title);

            return this.Ok(imagesByTitle);
        }

        [HttpGet]
        [Route("api/ImagesByTagName")]
        public IHttpActionResult GetImagesByTag(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return this.BadRequest("Tag name cannot be null or empty!");
            }

            var imagesByTagName = this.imagesService.GetAllByTag(tagName);

            return this.Ok(imagesByTagName);
        }

        [HttpGet]
        public IHttpActionResult GetImagesByUploadDate(DateTime uploadedOn)
        {
            var imagesByUploadDate = this.imagesService.GetAllByUlopadedOn(uploadedOn);

            return this.Ok(imagesByUploadDate);
        }
    }
}
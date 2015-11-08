namespace PSher.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using AutoMapper.QueryableExtensions;
    using Models.Images;
    using Services.Data.Contracts;

    [RoutePrefix("api/Images")]
    public class ImagesController : ApiController
    {
        private readonly IImagesService images;

        public ImagesController(IImagesService imagesService)
        {
            this.images = imagesService;
        }

        [EnableCors("*", "*", "*")]
        public IHttpActionResult Get()
        {
            var result = this.images
                .All()
                .ProjectTo<ImageResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        public IHttpActionResult Post(SaveImageRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var createdProjectId = this.images.Add(
                model.Title, 
                model.AuthorUserName,
                model.Description,
                model.IsPrivate,
                model.Tags);

            return this.Ok(createdProjectId);
        }

        [HttpGet]
        [Route("api/ImagesByUsername")]
        public IHttpActionResult GetImagesByUsername(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return this.BadRequest("Username cannot be null or empty!");
            }

            var imagesByUser = this.images.GetAllByUserName(userName);

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

            var imagesByTitle = this.images.GetAllByTitle(title);

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

            var imagesByTagName = this.images.GetAllByTag(tagName);

            return this.Ok(imagesByTagName);
        }

        [HttpGet]
        public IHttpActionResult GetImagesByUploadDate(DateTime uploadedOn)
        {
            var imagesByUploadDate = this.images.GetAllByUlopadedOn(uploadedOn);

            return this.Ok(imagesByUploadDate);
        }

        [HttpPost]
        [Route("api/Add")]
        public IHttpActionResult Add(string title,
            string authorUserName,
            string description,
            bool isPrivate,
            ICollection tags,
            IDictionary<string,
            DateTime> albumsToAdd)
        {

        }

    }
}
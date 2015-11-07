namespace PSher.Api.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using AutoMapper.QueryableExtensions;
    using Services.Data.Contracts;

    using PSher.Api.Models.Images;

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
                model.Tags
                );

            return this.Ok(createdProjectId);
        }
    }
}
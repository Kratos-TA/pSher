namespace PSher.Api.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Common.Constants;
    using Services.Data.Contracts;

    public class ImagesController : ApiController
    {
        private readonly IImagesService images;

        public ImagesController(IImagesService imagesService)
        {
            this.images = imagesService;
        }
    }
}
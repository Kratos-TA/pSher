namespace PSher.Api.Controllers
{
    using AutoMapper.QueryableExtensions;
    using DataTransferModels.Album;
    using Services.Data.Contracts;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("api/albums")]
    public class AlbumsController : ApiController
    {
        private readonly IAlbumsService albumsService;

        public AlbumsController(IAlbumsService albumsService)
        {
            this.albumsService = albumsService;
        }

        public IHttpActionResult Get()
        {
            var result =  this.albumsService
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
                .ToList(); ;

            return this.Ok(result);
        }

        //public IHttpActionResult Post()
        //{

        //}

        //public IHttpActionResult Put()
        //{

        //}

        //public IHttpActionResult Delete()
        //{

        //}
    }
}
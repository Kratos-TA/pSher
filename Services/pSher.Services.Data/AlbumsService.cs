
namespace PSher.Services.Data
{
    using Common.Constants;
    using PSher.Data.Contracts;
    using PSher.Models;
    using Contracts;
    using System.Linq;

    public class AlbumsService : IAlbumsService
    {
        private readonly IRepository<Album> albums;
        private readonly IRepository<User> users;

        public AlbumsService(IRepository<Album> albumsRepo, IRepository<User> userRepo)
        {
            this.albums = albumsRepo;
            this.users = userRepo;
        }

        public IQueryable<Album> All(int page = 1, int pageSize = GlobalConstants.DefaultPageSize)
        {
            var allAlbums = this.albums
                   .All()
                   .OrderBy(a => a.Name)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize);

            return allAlbums;
        }

        public IQueryable<Album> GetAlbumById(int id)
        {
            var albumById = this.albums
                .All()
                .Where(a => a.Id == id);

            return albumById;
        }
    }
}

namespace PSher.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Contracts;
    using PSher.Common.Constants;
    using PSher.Common.Extensions;
    using PSher.Data.Contracts;
    using PSher.Models;

    public class AlbumsService : UserAutenticationDependService, IAlbumsService
    {
        private readonly IRepository<Album> albums;
        private readonly IRepository<Tag> tags;

        public AlbumsService(
            IRepository<Album> albumsRepo,
            IRepository<User> userRepo,
            IRepository<Tag> tagsRepo)
            : base(userRepo)
        {
            this.albums = albumsRepo;
            this.tags = tagsRepo;
        }

        public async Task<IEnumerable<Album>> AlbumsFromCommaSeparatedValuesAndUserId(string albumsAsCommaSeparatedValues, string userId)
        {
            if (string.IsNullOrWhiteSpace(albumsAsCommaSeparatedValues))
            {
                return Enumerable.Empty<Album>();
            }

            var albumsValues = new HashSet<string>();

            albumsAsCommaSeparatedValues.Split(new[] { GlobalConstants.CommaSeparatedCollectionSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(val =>
                {
                    albumsValues.Add(val.Trim());
                });

            var resultAlbum = await this.albums
                .All()
                .Where(a => (albumsValues.Contains(a.Name.ToLower()) ||
                                albumsValues.Contains(a.Id.ToString())) &&
                            a.Creator.Id == userId)
                .ToListAsync();

            (await this.albums
              .All()
              .Where(a => (albumsValues.Contains(a.Name.ToLower()) ||
                                albumsValues.Contains(a.Id.ToString())) &&
                            a.Creator.Id == userId)
              .Select(a => a.Name.ToLower())
              .ToListAsync())
              .ForEach(a => albumsValues.Remove(a));

            var idValues = new List<string>();

            albumsValues.ForEach(v =>
            {
                int id;
                var isId = int.TryParse(v, out id);
                if (isId)
                {
                    idValues.Add(v);
                }
            });

            idValues.ForEach(a => albumsValues.Remove(a));

            albumsValues.ForEach(tagName => resultAlbum.Add(new Album() { Name = tagName }));

            return resultAlbum;
        }

        public IQueryable<Album> All(
            int page = 1,
            int pageSize = GlobalConstants.DefaultPageSize,
            bool isAuthorizedAccess = false,
            string authenticatedUserId = "")
        {
            var currentUser = this.GetCurrentOrEmptyUserById(authenticatedUserId);

            var allAlbums = this.albums
                .All()
                .Where(a => (a.IsDeleted == false) &&
                    (a.IsPrivate == false ||
                        (a.Creator.Id == currentUser.Id &&
                            isAuthorizedAccess == true)))
                .OrderBy(a => a.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return allAlbums;
        }

        public IQueryable<Album> AllByParamethers(
            string albumName,
            string albumCreator,
            IEnumerable<string> albumTags,
            int page,
            int pageSize,
            bool isAuthorizedAccess = false,
            string authenticatedUserId = "")
        {
            var currentUser = this.GetCurrentOrEmptyUserById(authenticatedUserId);

            var allAlbums = this.albums
                .All()
                .Where(a => (a.IsDeleted == false) 
                    && (a.IsPrivate == false 
                        || (a.Creator.Id == currentUser.Id 
                            && isAuthorizedAccess == true)) 
                    && (a.Tags.Any(t => albumTags.Contains(t.Name)) 
                        || a.Name.Contains(albumName) 
                            || a.Creator.UserName.Contains(albumCreator)))
                .OrderBy(a => a.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return allAlbums;
        }

        public async Task<string> GetAlbumCreatorIdById(int id)
        {
            var imageAuthor = await this.albums
            .All()
            .Where(i => (i.IsDeleted == false) && i.Id == id)
            .Select(i => i.Creator.Id)
            .FirstOrDefaultAsync();

            return imageAuthor;
        }

        public IQueryable<Album> GetAlbumById(int id)
        {
            var albumById = this.albums
                .All()
                .Where(a => (a.IsDeleted == false) && a.Id == id);

            return albumById;
        }

        public async Task<int> Add(
            string name,
            string authorUserId,
            bool isPrivate,
            IEnumerable<Tag> albumTags = null,
            IEnumerable<Image> albumImages = null)
        {
            var currentUser = this.Users
                .All()
                .FirstOrDefault(u => u.Id == authorUserId && u.IsDeleted == false);

            if (currentUser == null)
            {
                return 0;
            }

            var newAlbum = new Album()
            {
                Name = name,
                Creator = currentUser,
                IsPrivate = isPrivate,
                CreatedOn = DateTime.Now
            };

            albumTags.ForEach(t =>
            {
                newAlbum.Tags.Add(t);
            });

            albumImages.ForEach(i =>
            {
                newAlbum.Images.Add(i);
            });

            this.albums.Add(newAlbum);
            await this.albums.SaveChangesAsync();

            return newAlbum.Id;
        }

        public async Task<int> UpdateAll(
            int id,
            string newName,
            string authenticatedUserId,
            bool? isPrivate,
            IEnumerable<Tag> newAlbumTags = null,
            IEnumerable<Image> newAlbumImages = null)
        {
            var albumToUpdate = this.albums
                .All()
                .FirstOrDefault(a => a.Id == id &&
                    a.IsDeleted == false &&
                    a.Creator.Id == authenticatedUserId);

            if (albumToUpdate == null)
            {
                return GlobalConstants.ItemNotFoundReturnValue;
            }

            if (!string.IsNullOrEmpty(newName))
            {
                albumToUpdate.Name = newName;
            }

            if (isPrivate != null)
            {
                albumToUpdate.IsPrivate = (bool)isPrivate;
            }

            albumToUpdate.Tags.Clear();
            newAlbumTags.ForEach(t => albumToUpdate.Tags.Add(t));

            albumToUpdate.Images.Clear();
            newAlbumImages.ForEach(i => albumToUpdate.Images.Add(i));

            this.albums.Update(albumToUpdate);
            var result = await this.albums.SaveChangesAsync();

            return result;
        }

        public async Task<int> Delete(int id, string userId)
        {
            var albumToDelete = this.albums
                .All()
                .FirstOrDefault(a => a.Id == id && a.Creator.Id == userId);

            if (albumToDelete != null)
            {
                return GlobalConstants.ItemNotFoundReturnValue;
            }

            albumToDelete.IsDeleted = true;

            await this.albums.SaveChangesAsync();

            return id;
        }
    }
}
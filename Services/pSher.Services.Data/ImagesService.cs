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

    public class ImagesService : IImagesService
    {
        private readonly IRepository<Image> images;
        private readonly IRepository<User> users;
        private readonly IRepository<Tag> tags;
        private readonly IRepository<Album> albums;

        public ImagesService(IRepository<Image> imagesRepo, IRepository<User> usersRepo, IRepository<Tag> tagsRepo, IRepository<Album> albumRepo)
        {
            this.images = imagesRepo;
            this.users = usersRepo;
            this.tags = tagsRepo;
            this.albums = albumRepo;
        }

        public async Task<IEnumerable<Image>> ImagesFromCommaSeparatedIds(string imageIdsAsCommaSeparatedValues)
        {
            if (string.IsNullOrWhiteSpace(imageIdsAsCommaSeparatedValues))
            {
                return Enumerable.Empty<Image>();
            }

            var imagesIds = new HashSet<int>();

            imageIdsAsCommaSeparatedValues.Split(new[] { GlobalConstants.CommaSeparatedCollectionSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(i =>
                {
                    imagesIds.Add(int.Parse(i));
                });

            var resultImages = await this.images
                .All()
                .Where(i => imagesIds.Contains(i.Id))
                .ToListAsync();

            return resultImages;
        }

        public IQueryable<Image> All(int page = 1, int pageSize = GlobalConstants.DefaultPageSize)
        {
            var allImages = this.images
                   .All()
                   .OrderByDescending(pr => pr.UploadedOn)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize);

            return allImages;
        }

        public IQueryable<Image> GetImageById(int id)
        {
            var imageById = this.images
                .All()
                .Where(i => i.Id == id);

            return imageById;
        }

        public bool Update(int id, string title, string description, IEnumerable<Tag> tags)
        {
            var image = this.images
                .GetById(id);

            image.Title = title;

            // For some reason this is necessary
            image.Author = image.Author;

            if (!string.IsNullOrEmpty(description))
            {
                image.Description = description;
            }

            if (tags.Count() > 0)
            {
                image.Tags = tags.ToList();
            }

            this.images.Update(image);

            this.images.SaveChanges();

            return true;
        }

        public bool DeleteImage(int id)
        {
            var image = this.images
                .GetById(id);

            image.IsDeleted = true;

            this.images.Update(image);

            this.images.SaveChanges();

            return true;
        }

        // TODO: Check in the end of the method
        public async Task<int> Add(string title, string authorUserName, string description, bool isPrivate, IEnumerable<Tag> imageTags, IDictionary<string, DateTime> albumsToAdd)
        {
            var currentUser = this.users
               .All()
               .FirstOrDefault(u => u.UserName == authorUserName);

            if (currentUser == null)
            {
                throw new ArgumentException(ErrorMessages.InvalidUser);
            }

            // TODO: TO Set DropBoxUrl !!!
            // TODO: ImageInfo to be added !!!
            // TODO: Make conversion befor dropBox upload and to check how and where to do that !!! 
            var newImage = new Image()
            {
                Title = title,
                Author = currentUser,
                Description = description,
                IsPrivate = isPrivate,
                UploadedOn = DateTime.Now
            };

            imageTags.ForEach(t =>
            {
                newImage.Tags.Add(t);
            });

            /*
            albumsToAdd.ForEach(t =>
            {
                var currentAlbum = this.albums.All().FirstOrDefault(album => album.Name == t.Key.ToLower());

                if (currentAlbum == null)
                {
                   currentAlbum = new Album()
                   {
                       Name = t.Key.ToLower(),
                       CreatedOn = t.Value
                    };
                }               

               newImage.Albums.Add(currentAlbum);
            });
           */

            this.images.Add(newImage);
            await this.images.SaveChangesAsync();

            return newImage.Id;
        }
    }
}

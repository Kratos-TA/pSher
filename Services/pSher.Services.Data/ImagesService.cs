namespace PSher.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Constants;
    using Common.Extensions;
    using PSher.Data.Contracts;
    using Models;
    using Contracts;

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

        public IQueryable<Image> All(int page = 1, int pageSize = GlobalConstants.DefaultPageSize)
        {
            var allImages = this.images
                   .All()
                   .OrderByDescending(pr => pr.UploadedOn)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize);

            return allImages;
        }

        public IQueryable<Image> GetAllByUserName(string userName)
        {
            var imagesByUser = this.images
                .All()
                .Where(i => i.Author.UserName == userName);

            return imagesByUser;
        }

        public IQueryable<Image> GetAllByTitle(string title)
        {
            var imagesByTitle = this.images
                .All()
                .Where(i => i.Title == title);

            return imagesByTitle;
        }

        public IQueryable<Image> GetAllByTag(string tagName)
        {
            var imagesByTag = this.images
                .All()
                .Where(i => i.Tags.Any(t => t.Name == tagName));

            return imagesByTag;
        }

        public IQueryable<Image> GetAllByUlopadedOn(DateTime uploadedOn)
        {
            var imagesByDate = this.images
                .All()
                .Where(i => DateTime.Compare(i.UploadedOn, uploadedOn) == 0);

            return imagesByDate;
        }

        // TODO: Check in the end of the method
        public int Add(string title, string authorUserName, string description, bool isPrivate, ICollection<string> imageTags, IDictionary<string, DateTime> albumsToAdd)
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
                var currentTag = tags.All().FirstOrDefault(existing => existing.Name == t.ToLower());

                if (currentTag == null)
                {
                    currentTag = new Tag()
                    {
                        Name = t
                    };
                }

                //this.tags.Add(currentTag); This should be necessary.
                newImage.Tags.Add(currentTag);
            });

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

                //this.albums.Add(currentAlbum); This should be necessary.
                newImage.Albums.Add(currentAlbum);
            });

            this.images.Add(newImage);
            this.images.SaveChanges();
            //this.tags.SaveChanges();
            //this.albums.SaveChanges();

            return newImage.Id;
        }
    }
}

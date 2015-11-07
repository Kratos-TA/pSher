namespace PSher.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PSher.Common.Constants;
    using PSher.Common.Extensions;
    using PSher.Data.Contracts;
    using PSher.Models;
    using PSher.Services.Data.Contracts;

    public class ImagesService : IImagesService
    {
        private readonly IRepository<Image> images;
        private readonly IRepository<User> users;
        private readonly IRepository<Tag> tags;

        public ImagesService(IRepository<Image> imagesRepo, IRepository<User> usersRepo, IRepository<Tag> tagsRepo)
        {
            this.images = imagesRepo;
            this.users = usersRepo;
            this.tags = tagsRepo;
        }

        public IQueryable<Image> All(int page = 1, int pageSize = GlobalConstants.DefaultPageSize)
        {
            var allImages =  this.images
                   .All()
                   .OrderByDescending(pr => pr.UploadedOn)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize);

            return allImages;
        }

        public IQueryable<Image> GetAllByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Image> GetAllByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Image> GetAllByTag(string tagName)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Image> GetAllByUlopadedOn(DateTime uploadedOn)
        {
            throw new NotImplementedException();
        }

        // TODO: Add albums
        public int Add(string title, string authorUserName,  string description, bool isPrivate, ICollection<string> imageTags)
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
                        Name = t.ToLower()
                    };
                }

                newImage.Tags.Add(currentTag);
            });

            this.images.Add(newImage);
            this.images.SaveChanges();

            return newImage.Id;
        }
    }
}

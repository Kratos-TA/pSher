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
    using PSher.Common.Models;
    using PSher.Data.Contracts;
    using PSher.Models;
    using PSher.Services.Common.Contracts;
    using PSher.Services.Logic.Contracts;

    using Spring.IO;

    public class ImagesService : UserAutenticationDependService, IImagesService
    {
        private readonly IRepository<Image> images;
        private readonly IRepository<Tag> tags;
        private readonly IRepository<Album> albums;
        private readonly IImageProcessorService imageProcessor;
        private readonly IDropboxService dropbox;
        private readonly INotificationService notifier;

        public ImagesService(
            IRepository<Image> imagesRepo,
            IRepository<User> usersRepo,
            IRepository<Tag> tagsRepo,
            IRepository<Album> albumRepo,
            IImageProcessorService imageProcessor,
            IDropboxService dropbox,
            INotificationService notifier)
            : base(usersRepo)
        {
            this.images = imagesRepo;
            this.tags = tagsRepo;
            this.albums = albumRepo;
            this.imageProcessor = imageProcessor;
            this.dropbox = dropbox;
            this.notifier = notifier;
        }

        public async Task<IEnumerable<Image>> ImagesFromCommaSeparatedIds(string imageIdsAsCommaSeparatedValues)
        {
            if (string.IsNullOrWhiteSpace(imageIdsAsCommaSeparatedValues))
            {
                return Enumerable.Empty<Image>();
            }

            var imagesIds = new HashSet<int>();

            imageIdsAsCommaSeparatedValues
                .Split(
                    new[] { GlobalConstants.CommaSeparatedCollectionSeparator },
                    StringSplitOptions.RemoveEmptyEntries)
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

        public IQueryable<Image> All(
            int page = 1,
            int pageSize = GlobalConstants.DefaultPageSize,
            bool isAuthorizedAccess = false,
            string authenticatedUserId = "")
        {
            var currentUser = this.GetCurrentOrEmptyUserById(authenticatedUserId);

            var allImages = this.images
                .All()
                .Where(i => (i.IsDeleted == false) &&
                    (i.IsPrivate == false ||
                        (i.Author.Id == currentUser.Id &&
                            isAuthorizedAccess == true)))
                .OrderBy(a => a.UploadedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return allImages;
        }

        public IQueryable<Image> AllByParamethers(
        string imageTitle,
        string imageAuthor,
        IEnumerable<string> imageTags,
        int page,
        int pageSize,
        bool isAuthorizedAccess = false,
        string authenticatedUserId = "")
        {
            var currentUser = this.GetCurrentOrEmptyUserById(authenticatedUserId);

            var allAlbums = this.images
                .All()
                .Where(i => (i.IsDeleted == false) &&
                    (i.IsPrivate == false || (i.Author.Id == currentUser.Id && isAuthorizedAccess == true)) &&
                    (i.Tags.Any(t => imageTags.Contains(t.Name)) ||
                        i.Author.UserName.Contains(imageAuthor) ||
                        i.Author.FirstName.Contains(imageAuthor) ||
                        i.Author.LastName.Contains(imageAuthor) ||
                        i.Title.Contains(imageTitle)))
                .OrderBy(i => i.UploadedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return allAlbums;
        }

        public IQueryable<Image> GetImageById(
            int id,
            bool isAuthorizedAccess,
            string authenticatedUserId)
        {
            var currentUser = this.GetCurrentOrEmptyUserById(authenticatedUserId);

            var imageById = this.images
                .All()
                .Where(i => (i.IsDeleted == false) &&
                        i.Id == id &&
                            (i.IsPrivate == false ||
                                (i.Author.Id == currentUser.Id &&
                                    isAuthorizedAccess == true)));

            return imageById;
        }

        public async Task<string> GetImageAuthorIdById(int id)
        {
            var imageAuthor = await this.images
                .All()
                .Where(i => (i.IsDeleted == false) && i.Id == id)
                .Select(i => i.Author.Id)
                .FirstOrDefaultAsync();

            return imageAuthor;
        }

        // TODO: Fix update
        public async Task<int> Update(
            int id,
            string title,
            string description,
            IEnumerable<Tag> tags)
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

            return 1;
        }

        // TODO: Fix delete
        public async Task<int> DeleteImage(int id)
        {
            var image = this.images
                .GetById(id);

            image.IsDeleted = true;

            this.images.Update(image);

            this.images.SaveChanges();

            return 1;
        }

        public async Task<int> Add(
            string title,
            string autenticatedUserId,
            string description,
            bool isPrivate,
            RawFile rawImage,
            IEnumerable<Tag> imageTags = null,
            IEnumerable<Album> imageAlbums = null)
        {
            var currentUser = this.Users
               .All()
               .FirstOrDefault(u => u.Id == autenticatedUserId || u.IsDeleted == false);

            if (currentUser == null)
            {
                throw new ArgumentException(ErrorMessages.InvalidUser);
            }

            var newImageInfo = new ImageInfo()
            {
                OriginalName = rawImage.OriginalFileName,
                OriginalExtension = rawImage.FileExtension,
            };

            var newImage = new Image()
            {
                Title = title,
                Author = currentUser,
                Description = description,
                IsPrivate = isPrivate,
                UploadedOn = DateTime.Now,
                ImageInfo = newImageInfo,
            };

            this.images.Add(newImage);
            await this.images.SaveChangesAsync();

            var fileToUpload = new ByteArrayResource(rawImage.Content);

            string fileNameWithExtension = newImage.Id + "." + rawImage.FileExtension;
            await this.dropbox.UploadImageToCloud(fileToUpload, fileNameWithExtension);

            string path = "/" + DropboxConstants.Collection + "/" + "2" + "." + "png";
            string dropboxUrl = await this.dropbox.GetImageUrl(path);

            newImage.DropboxUrl = dropboxUrl;

            imageTags.ForEach(t =>
            {
                newImage.Tags.Add(t);
            });

            imageAlbums.ForEach(a =>
            {
                newImage.Albums.Add(a);
            });

            this.images.Update(newImage);
            await this.images.SaveChangesAsync();

            string notification = string.Format("{0} added picture {1}", currentUser.UserName, title);
            this.notifier.Notify(notification);

            return newImage.Id;
        }

        public async Task<RawFile> ProcessImage(RawFile rawImage)
        {
            rawImage.Content = await this.imageProcessor.Resize(rawImage.Content, GlobalConstants.ResizedImageWidth);

            return rawImage;
        }
    }
}

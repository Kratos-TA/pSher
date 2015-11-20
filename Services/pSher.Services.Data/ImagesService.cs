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

    public class ImagesService : UserAutenticationDependService, IImagesService
    {
        public const string ThumbnailExtension = "-thumbnail";

        private readonly IRepository<Image> images;
        private readonly IRepository<Album> albums;
        private readonly IImageProcessorService imageProcessor;
        private readonly IWebStorageService webSrorageService;
        private readonly INotificationService notifier;

        public ImagesService(
            IRepository<Image> imagesRepo,
            IRepository<User> usersRepo,
            IRepository<Album> albumRepo,
            IImageProcessorService imageProcessor,
            IWebStorageService webSrorageService,
            INotificationService notifier)
            : base(usersRepo)
        {
            this.images = imagesRepo;
            this.albums = albumRepo;
            this.imageProcessor = imageProcessor;
            this.webSrorageService = webSrorageService;
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
                .OrderByDescending(a => a.UploadedOn)
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

            var allImages = this.images
                .All()
                .Where(i => (i.IsDeleted == false) &&
                    (i.IsPrivate == false || (i.Author.Id == currentUser.Id && isAuthorizedAccess == true)) &&
                    (i.Tags.Any(t => imageTags.Contains(t.Name)) ||
                        i.Author.UserName.Contains(imageAuthor) ||
                        i.Author.FirstName.Contains(imageAuthor) ||
                        i.Author.LastName.Contains(imageAuthor) ||
                        i.Title.Contains(imageTitle)))
                .OrderByDescending(i => i.UploadedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return allImages;
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
                .Where(i => i.Id == id
                    && i.IsDeleted == false
                    && i.Author.IsDeleted == false)
                .Select(i => i.Author.Id)
                .FirstOrDefaultAsync();

            return imageAuthor;
        }

        public async Task<int> Update(
            int id,
            string newTitle = null,
            bool? isPrivate = null,
            string newDescription = null,
            IEnumerable<Tag> newImageTags = null,
            IEnumerable<Album> newImageAlbums = null)
        {
            var imageToUpdate = this.images
                  .All()
                  .FirstOrDefault(i => i.Id == id);

            if (imageToUpdate == null)
            {
                return GlobalConstants.ItemNotFoundReturnValue;
            }

            if (!string.IsNullOrEmpty(newTitle))
            {
                imageToUpdate.Title = newTitle;
            }

            if (isPrivate != null)
            {
                imageToUpdate.IsPrivate = (bool)isPrivate;
            }

            imageToUpdate.Tags.Clear();
            newImageTags.ForEach(t => imageToUpdate.Tags.Add(t));

            var result = await this.images.SaveChangesAsync();

            return result;
        }

        public async Task<int> DeleteImage(int id)
        {
            var imageToDelete = this.images
                .All()
                .FirstOrDefault(a => a.Id == id);

            if (imageToDelete == null)
            {
                return GlobalConstants.ItemNotFoundReturnValue;
            }

            imageToDelete.IsDeleted = true;
            imageToDelete.Tags.Clear();

            var result = await this.albums.SaveChangesAsync();

            return result;
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
               .FirstOrDefault(u => u.Id == autenticatedUserId && u.IsDeleted == false);

            if (currentUser == null)
            {
                return GlobalConstants.ItemNotFoundReturnValue;
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

            var guid = Guid.NewGuid().ToString();

            newImage.Url = await this.webSrorageService
                .UploadImageToCloud(
                rawImage.Content,
                guid,
                rawImage.FileExtension,
                currentUser.Id);

            newImage.ThumbnailUrl = await this.webSrorageService
                .UploadImageToCloud(
                rawImage.PreviewContent,
                guid + ThumbnailExtension,
                rawImage.FileExtension,
                currentUser.Id);

            if (imageTags != null)
            {
                imageTags.ForEach(t =>
                    {
                        newImage.Tags.Add(t);
                    });
            }

            if (imageAlbums != null)
            {
                imageAlbums.ForEach(a =>
                    {
                        newImage.Albums.Add(a);
                    });
            }

            try
            {
                this.images.Add(newImage);
                await this.images.SaveChangesAsync();
            }
            catch (Exception e)
            {

              var er =  e.Message;
            }

     

            try
            {
                string notification = string.Format("{0} added picture {1}", currentUser.UserName, title);
                this.notifier.Notify(notification);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return newImage.Id;
        }

        public async Task<RawFile> ProcessImage(RawFile rawImage)
        {
            rawImage.Content = await this.imageProcessor
                .Resize(rawImage.Content, GlobalConstants.ResizedImageWidth);
            rawImage.PreviewContent = await this.imageProcessor
                .Resize(rawImage.Content, GlobalConstants.ResizedImageThumbnailWidth);

            return rawImage;
        }
    }
}

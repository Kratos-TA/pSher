﻿namespace PSher.Api.Controllers
{
    using System.Data.Entity;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNet.Identity;

    using PSher.Api.DataTransferModels.Album;
    using PSher.Api.Validation;
    using PSher.Common.Constants;
    using PSher.Common.Extensions;
    using PSher.Services.Data.Contracts;
    
    [RoutePrefix("api/albums")]
    [EnableCors("*", "*", "*")]
    public class AlbumsController : ApiController
    {
        public const string EntityName = "Album";

        private readonly IAlbumsService albumsService;
        private readonly ITagsService tagsService;
        private readonly IImagesService imagesService;

        public AlbumsController(
            IAlbumsService albumsService, 
            ITagsService tagsService, 
            IImagesService imagesService)
        {
            this.albumsService = albumsService;
            this.tagsService = tagsService;
            this.imagesService = imagesService;
        }

        public async Task<IHttpActionResult> Get(
            string name, 
            string user = null, 
            string tags = null, 
            int page = 1, 
            int pageSize = 10)
        {
            var isAuthorizedAccess = this.User.Identity.IsAuthenticated;
            var currentUserId = this.User.Identity.GetUserId();
            var selectedTags = tags.GetEnumerableFromCommSeparatedString();

            var result = await this.albumsService
                .AllByParamethers(name, user, selectedTags, page, pageSize, isAuthorizedAccess, currentUserId)
                .ProjectTo<AlbumResponseModel>()
                .ToListAsync();

            return this.Ok(result);
        }

        public async Task<IHttpActionResult> Get(int page = 1, int pageSize = 10)
        {
            var isAuthorizedAccess = this.User.Identity.IsAuthenticated;
            var currentUserId = this.User.Identity.GetUserId();

            var result = await this.albumsService
                .All(page, pageSize, isAuthorizedAccess, currentUserId)
                .ProjectTo<AlbumResponseModel>()
                .ToListAsync();

            return this.Ok(result);
        }

        public async Task<IHttpActionResult> Get(string id)
        {
            var isAuthorizedAccess = this.User.Identity.IsAuthenticated;
            var currentUserId = this.User.Identity.GetUserId();

            var resultAlbum = await this.albumsService
                .GetAlbumById(int.Parse(id))
                .ProjectTo<AlbumDetailsResponseModel>()
                .FirstOrDefaultAsync();

            if (resultAlbum.IsPrivate 
                && !isAuthorizedAccess 
                && currentUserId == null
                    || currentUserId != resultAlbum.CreatorId)
            {
                return this.Unauthorized(AuthenticationHeaderValue.
                    Parse(string.Format(ErrorMessages.UnoutorizedAccess, EntityName)));
            }

            return this.Ok(resultAlbum);
        }

        [Authorize]
        [ValidateModel]
        public async Task<IHttpActionResult> Post(SaveAlbumRequestModel model)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);
            var images = await this.imagesService.ImagesFromCommaSeparatedIds(model.ImagesIds);

            var addedAlbumId = await this.albumsService
                .Add(
                model.Name,
                currentUserId,
                model.IsPrivate,
                tags,
                images);

            if (addedAlbumId == GlobalConstants.ItemNotFoundReturnValue)
            {
                return this.BadRequest(ErrorMessages.InvalidUser);
            }

            return this.Ok(addedAlbumId);
        }

        [Authorize]
        [ValidateModel]
        public async Task<IHttpActionResult> Put(int id, UpdateAlbumRequestModel model)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var imageAuthorId = await this.albumsService.GetAlbumCreatorIdById(id);

            var isCurrenUserAlbum = currentUserId == imageAuthorId;

            if (!isCurrenUserAlbum)
            {
                return this.Unauthorized(AuthenticationHeaderValue.
                 Parse(string.Format(ErrorMessages.UnoutorizedAccess, EntityName)));
            }

            var tags = await this.tagsService.TagsFromCommaSeparatedValues(model.Tags);
            var images = await this.imagesService.ImagesFromCommaSeparatedIds(model.ImagesIds);

            var changesMade = await this.albumsService.Update(
                id,
                model.Name,
                model.IsPrivate,
                tags,
                images);

            if (changesMade == GlobalConstants.ItemNotFoundReturnValue)
            {
                return this.NotFound();
            }

            return this.Ok(changesMade);
        }

        [Authorize]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var imageAuthorId = await this.albumsService.GetAlbumCreatorIdById(id);

            var isCurrenUserAlbum = currentUserId == imageAuthorId;

            if (!isCurrenUserAlbum)
            {
                return this.Unauthorized(AuthenticationHeaderValue.
                   Parse(string.Format(ErrorMessages.UnoutorizedAccess, EntityName)));
            }

            var changesMade = await this.albumsService.Delete(id);

            if (changesMade == GlobalConstants.ItemNotFoundReturnValue)
            {
                return this.NotFound();
            }

            return this.Ok(changesMade);
        }
    }
}
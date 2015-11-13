﻿namespace PSher.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common.Constants;
    using Contracts;
    using PSher.Common.Extensions;
    using PSher.Data.Contracts;
    using PSher.Models;

    public class AlbumsService : IAlbumsService
    {
        private readonly IRepository<Album> albums;
        private readonly IRepository<User> users;
        private readonly IRepository<Tag> tags;

        public AlbumsService(IRepository<Album> albumsRepo, IRepository<User> userRepo, IRepository<Tag> tagsRepo)
        {
            this.albums = albumsRepo;
            this.users = userRepo;
            this.tags = tagsRepo;
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

        public async Task<int> Add(string name, string authorUserName, bool isPrivate, IEnumerable<Tag> albumTags = null, IEnumerable<Image> albumImages = null)
        {
            var currentUser = this.users
                .All()
                .FirstOrDefault(u => u.UserName == authorUserName);

            if (currentUser == null)
            {
                throw new ArgumentException(ErrorMessages.InvalidUser);
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

        public async Task<int> Update(int id, string name, string authenticatedUserName, bool isPrivate, IEnumerable<Tag> albumTags = null, IEnumerable<Image> albumImages = null)
        {
            var albumToChange = this.albums
                .All()
                .FirstOrDefault(a => a.Id == id);

            var currentUser = this.users
                .All()
                .FirstOrDefault(u => u.UserName == authenticatedUserName);

            albumToChange.Name = name;
            albumToChange.Creator = currentUser;
            albumToChange.IsPrivate = isPrivate;

            albumTags.ForEach(t =>
            {
                if (albumToChange.Tags.Contains(t))
                {
                    albumToChange.Tags.Remove(t);
                }
                else
                {
                    albumToChange.Tags.Add(t);
                }
            });

            albumImages.ForEach(i =>
            {
                if (albumToChange.Images.Contains(i))
                {
                    albumToChange.Images.Remove(i);
                }
                else
                {
                    albumToChange.Images.Add(i);
                }
            });

            this.albums.Update(albumToChange);
            await this.albums.SaveChangesAsync();

            return albumToChange.Id;
        }

        public async Task<int> Delete(int id)
        {
            var albumToDelete = this.albums
                .All()
                .FirstOrDefault(a => a.Id == id);

            albumToDelete.IsDeleted = true;
            albumToDelete.Creator = albumToDelete.Creator;

            this.albums.Update(albumToDelete);

            await this.albums.SaveChangesAsync();

            return id;
        }
    }
}
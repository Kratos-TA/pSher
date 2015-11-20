namespace PSher.Api.Tests.Setups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Common.Constants;
    using Constants;
    using Data.Contracts;
    using Moq;

    using PSher.Models;

    public sealed class Repositories
    {
        private List<User> users;
        private List<Image> images;
        private List<Album> albums;
        private List<Tag> tags;
        private List<Mark> marks;
        private List<Comment> comments;

        private static readonly Lazy<Repositories> lazy = 
            new Lazy<Repositories>(() => new Repositories());

        public static Repositories Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private Repositories()
        {
            this.users = this.GetCollectionOfUsers(TestsConstants.EntitiesPerRepesitory);
            this.marks = this.GetCollectionOfMarks(TestsConstants.EntitiesPerRepesitory);
            this.tags = this.GetCollectionOfTags(TestsConstants.EntitiesPerRepesitory);
            this.images = this.GetCollectionOfImages(TestsConstants.EntitiesPerRepesitory);
            this.albums = this.GetCollectionOfAlbums(TestsConstants.EntitiesPerRepesitory);
            this.comments = this.GetCollectionOfComments(TestsConstants.EntitiesPerRepesitory);
        }

        public IRepository<Album> GetAlbumsRepository()
        {
            var repository = new Mock<IRepository<Album>>();
            repository.Setup(r => r.All())
                .Returns(() =>
                {

                    return this.albums.AsQueryable();
                });

            return repository.Object;
        }

        public IRepository<Image> GetImagesRepository()
        {
            var repository = new Mock<IRepository<Image>>();
            repository.Setup(r => r.All())
               .Returns(() =>
               {
                   return this.images.AsQueryable();
               });

            return repository.Object;
        }

        public IRepository<User> GetUsersRepository()
        {
            var repository = new Mock<IRepository<User>>();
            repository.Setup(r => r.All())
           .Returns(() =>
           {
               return this.users.AsQueryable();
           });

            return repository.Object;
        }

        public IRepository<Mark> GetMarksRepository()
        {
            var repository = new Mock<IRepository<Mark>>();
            repository.Setup(r => r.All())
               .Returns(() =>
               {
                   return this.marks.AsQueryable();
               });

            return repository.Object;
        }

        public IRepository<Comment> GetCommentsRepository()
        {
            var repository = new Mock<IRepository<Comment>>();
            repository.Setup(r => r.All())
               .Returns(() =>
               {
                   return this.comments.AsQueryable();
               });

            return repository.Object;
        }

        public IRepository<Tag> GetTagsRepository()
        {
            var repository = new Mock<IRepository<Tag>>();
            repository.Setup(r => r.All())
               .Returns(() =>
               {
                   return this.tags.AsQueryable();
               });

            return repository.Object;
        }

        private List<Album> GetCollectionOfAlbums(int count)
        {
            var collection = new List<Album>();

            for (int i = 1; i <= count; i++)
            {
                collection.Add(this.GetSingleAlbum(i));
            }

            return collection;
        }

        private List<Album> GetCollectionOfExistingAlbums(int count)
        {
            var collection = new List<Album>();

            for (int i = 1; i <= count; i++)
            {
                collection.Add(this.albums[i % this.albums.Count]);
            }

            return collection;
        }

        private Album GetSingleAlbum(int id)
        {
            return new Album()
            {
                Id = id,
                Name = TestsConstants.AlbumBaseName + id,
                IsDeleted = this.GetOddNumberAsFalse(id),
                Tags = this.GetCollectionOfExistingTags(TestsConstants.TagsPerImage),
                Images = this.GetCollectionOfExistingImages(TestsConstants.ImagesPerAlbum),
                IsPrivate = this.GetOddNumberAsFalse(id)
            };
        }

        private List<Image> GetCollectionOfImages(int count)
        {
            var collection = new List<Image>();

            for (int i = 1; i <= count; i++)
            {
                collection.Add(this.GetSingleImage(i));
            }

            return collection;
        }

        private List<Image> GetCollectionOfExistingImages(int count)
        {
            var collection = new List<Image>();

            for (int i = 1; i <= count; i++)
            {
                collection.Add(this.images[i % this.images.Count]);
            }

            return collection;
        }

        private Image GetSingleImage(int id)
        {
            return new Image()
            {
                Id = id,
                Title = TestsConstants.ImageBaseTitle + id,
                Description = TestsConstants.DescriptionBaseTextr + id,
                UploadedOn = DateTime.Now.AddDays(id),
                Url = string.Format(TestsConstants.ImageBasyeUrl, id),
                ThumbnailUrl = string.Format(TestsConstants.ImageBaseThumbnailUrl, id),
                Author = this.users[id % users.Count],
                Tags = this.GetCollectionOfExistingTags(TestsConstants.TagsPerImage),
                ImageInfo = new ImageInfo()
                {
                    Id = id,
                    OriginalExtension = TestsConstants.ImageBaseExstnsion,
                    OriginalName = TestsConstants.ImageInfoBaseOriginalName + id,
                    IsDeleted = this.GetOddNumberAsFalse(id),
                    CreatedOn = DateTime.Now.AddDays(id)
                },
                IsDeleted = GetOddNumberAsFalse(id),
                Rating = GetSingleRating(id)
            };
        }

        private Rating GetSingleRating(int id)
        {
            return new Rating()
            {
                Id = id,
                IsDeleted = this.GetOddNumberAsFalse(id),
                Marks = this.GetCollectionOfMarks(TestsConstants.MarksPerImage)
            };
        }

        private List<User> GetCollectionOfUsers(int count)
        {
            var collection = new List<User>();

            for (int i = 1; i <= count; i++)
            {
                collection.Add(GetSingleUser(i));
            }

            return collection;
        }

        private User GetSingleUser(int id)
        {
            return new User()
            {
                Id = id.ToString(),
                UserName = TestsConstants.UserBaseUserName + id,
                FirstName = TestsConstants.UserBaseFirstName + id,
                LastName = TestsConstants.UserBaseLastName + id,
                Email = string.Format(TestsConstants.UserBaseEmail, id),
                IsDeleted = this.GetOddNumberAsFalse(id)
            };
        }

        private List<Mark> GetCollectionOfMarks(int count)
        {
            var collection = new List<Mark>();

            for (int i = 1; i <= count; i++)
            {
                collection.Add(this.GetSingleMark(i));
            }

            return collection;
        }

        private Mark GetSingleMark(int id)
        {
            return new Mark()
            {
                Id = id,
                GivenBy = this.users[id % this.users.Count],
                IsDeleted = this.GetOddNumberAsFalse(id),
                Value = id % ValidationConstants.MaxMarkValue
            };
        }

        private List<Comment> GetCollectionOfComments(int count)
        {
            var collection = new List<Comment>();

            for (int i = 1; i <= count; i++)
            {
                collection.Add(this.GetSingleComment(i));
            }

            return collection;
        }

        private Comment GetSingleComment(int id)
        {
            return new Comment()
            {
                Id = id,
                PostedOn = DateTime.Now.AddDays(id),
                Text = TestsConstants.CommentBaseText + id,
                Author = this.users[id % this.users.Count],
                Likes = id % TestsConstants.CommonModulDivisor,
                Dislikes = (id % TestsConstants.CommonModulDivisor) / 2,
                IsDeleted = this.GetOddNumberAsFalse(id)
            };
        }

        private List<Tag> GetCollectionOfTags(int count)
        {
            var collection = new List<Tag>();

            for (int i = 1; i <= count; i++)
            {
                collection.Add(GetSingleTag(i));
            }

            return collection;
        }

        private List<Tag> GetCollectionOfExistingTags(int count)
        {
            var collection = new List<Tag>();

            for (int i = 1; i <= count; i++)
            {
                collection.Add(this.tags[i % this.tags.Count]);
            }

            return collection;
        }

        private Tag GetSingleTag(int id)
        {
            return new Tag()
            {
                Id = id,
                Name = TestsConstants.TagBaseName + id,
                IsDeleted = this.GetOddNumberAsFalse(id)
            };
        }

        private bool GetOddNumberAsFalse(int number)
        {
            return number % 2 == 0 ? true : false;
        }
    }
}

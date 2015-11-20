namespace PSher.Api.Tests.Setups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Constants;
    using Constants;
    using Data.Contracts;
    using Moq;

    using PSher.Models;

    public static class Repositories
    {
        public static IRepository<Album> GetAlbumsRepository()
        {
            var repository = new Mock<IRepository<Album>>();
            repository.Setup(r => r.All())
                .Returns(() =>
                {
                    var albums = GetCollectionOfAlbums(TestsConstants.EntitiesPerRepesitory);
                    return albums.AsQueryable();
                });

            return repository.Object;
        }

        public static IRepository<Image> GetImagesRepository()
        {
            var repository = new Mock<IRepository<Image>>();
            repository.Setup(r => r.All())
               .Returns(() =>
               {
                   var albums = GetCollectionOfImages(TestsConstants.EntitiesPerRepesitory);
                   return albums.AsQueryable();
               });

            return repository.Object;
        }

        public static IRepository<User> GetUsersRepository()
        {
            var repository = new Mock<IRepository<User>>();
            repository.Setup(r => r.All())
           .Returns(() =>
           {
               var albums = GetCollectionOfUsers(TestsConstants.EntitiesPerRepesitory);
               return albums.AsQueryable();
           });

            return repository.Object;
        }

        public static IRepository<Mark> GetMarksRepository()
        {
            var repository = new Mock<IRepository<Mark>>();
            repository.Setup(r => r.All())
               .Returns(() =>
               {
                   var albums = GetCollectionOfMarks(TestsConstants.EntitiesPerRepesitory);
                   return albums.AsQueryable();
               });

            return repository.Object;
        }

        public static IRepository<Comment> GetCommentsRepository()
        {
            var repository = new Mock<IRepository<Comment>>();
            repository.Setup(r => r.All())
               .Returns(() =>
               {
                   var albums = GetCollectionOfComments(TestsConstants.EntitiesPerRepesitory);
                   return albums.AsQueryable();
               });

            return repository.Object;
        }

        public static IRepository<Tag> GetTagsRepository()
        {
            var repository = new Mock<IRepository<Tag>>();
            repository.Setup(r => r.All())
               .Returns(() =>
               {
                   var albums = GetCollectionOfTags(TestsConstants.EntitiesPerRepesitory);
                   return albums.AsQueryable();
               });

            return repository.Object;
        }

        private static List<Album> GetCollectionOfAlbums(int count = TestsConstants.EntitiesPerRepesitory, int startingId = 1)
        {
            var collection = new List<Album>();

            for (int i = startingId; i <= count + startingId; i++)
            {
                collection.Add(GetSingleAlbum(i));
            }

            return collection;
        }

        private static Album GetSingleAlbum(int id)
        {
            return new Album()
            {
                Id = id,
                Name = "Some Album " + id,
                Tags = GetCollectionOfTags(TestsConstants.TagsPerAlbum, id * TestsConstants.TagsPerAlbum),
                Images = GetCollectionOfImages(TestsConstants.ImagesPerAlbum, id * TestsConstants.ImagesPerAlbum),
                IsDeleted = GetOddNumberAsFalse(id),
            };
        }

        private static List<Image> GetCollectionOfImages(int count = TestsConstants.EntitiesPerRepesitory, int startingId = 1)
        {
            var collection = new List<Image>();

            for (int i = startingId; i <= count + startingId; i++)
            {
                collection.Add(GetSingleImage(i));
            }

            return collection;
        }

        private static Image GetSingleImage(int id)
        {
            return new Image()
            {
                Id = id,
                Title = "Some Image " + id,
                Description = "Some Description " + id,
                UploadedOn = DateTime.Now.AddDays(id),
                Url = "http://someurl.com/" + id + ".jpg",
                ThumbnailUrl = "http://someurl.com/" + id + "-thmbnail.jpg",
                Author = GetSingleUser(id),
                Tags = GetCollectionOfTags(TestsConstants.TagsPerImage, id * TestsConstants.TagsPerImage),
                Albums = GetCollectionOfAlbums(TestsConstants.AlbumsPerImage, id * TestsConstants.AlbumsPerImage),
                ImageInfo = new ImageInfo()
                {
                    Id = id,
                    OriginalExtension = ".jpg",
                    OriginalName = "some-image-name" + id,
                    IsDeleted = GetOddNumberAsFalse(id),
                    CreatedOn = DateTime.Now.AddDays(id)
                },
                IsDeleted = GetOddNumberAsFalse(id),
                Comments = GetCollectionOfComments(TestsConstants.CommentsPerImage, id * TestsConstants.CommentsPerImage),
                Rating = GetSingleRating(id)
            };
        }

        private static Rating GetSingleRating(int id)
        {
            return new Rating()
            {
                Id = id,
                IsDeleted = GetOddNumberAsFalse(id),
                Marks = GetCollectionOfMarks(TestsConstants.MarksPerImage, id * TestsConstants.MarksPerImage)
            };
        }

        private static List<User> GetCollectionOfUsers(int count = TestsConstants.EntitiesPerRepesitory, int startingId = 1)
        {
            var collection = new List<User>();

            for (int i = startingId; i <= count + startingId; i++)
            {
                collection.Add(GetSingleUser(i));
            }

            return collection;
        }

        private static User GetSingleUser(int id)
        {
            return new User()
            {
                Id = id.ToString(),
                UserName = "Some UserName " + id,
                FirstName = "Some FirstName " + id,
                LastName = "Some LastName " + id,
                Email = "some-mail" + id + "@pesho.net",
                Albums = GetCollectionOfAlbums(TestsConstants.AlbumsPerUser, id * TestsConstants.AlbumsPerUser),
                Images = GetCollectionOfImages(TestsConstants.ImagesPerUser, id * TestsConstants.ImagesPerUser),
                IsDeleted = GetOddNumberAsFalse(id)
            };
        }

        private static List<Mark> GetCollectionOfMarks(int count = TestsConstants.EntitiesPerRepesitory, int startingId = 1)
        {
            var collection = new List<Mark>();

            for (int i = startingId; i <= count + startingId; i++)
            {
                collection.Add(GetSingleMark(i));
            }

            return collection;
        }

        private static Mark GetSingleMark(int id)
        {
            return new Mark()
            {
                Id = id,
                GivenBy = GetSingleUser(id),
                IsDeleted = GetOddNumberAsFalse(id),
                Value = id % ValidationConstants.MaxMarkValue
            };
        }

        private static List<Comment> GetCollectionOfComments(int count = TestsConstants.EntitiesPerRepesitory, int startingId = 1)
        {
            var collection = new List<Comment>();

            for (int i = startingId; i <= count + startingId; i++)
            {
                collection.Add(GetSingleComment(i));
            }

            return collection;
        }

        private static Comment GetSingleComment(int id)
        {
            return new Comment()
            {
                Id = id,
                PostedOn = DateTime.Now.AddDays(id),
                Text = "Some comment text " + id,
                Author = GetSingleUser(id),
                Likes = id % TestsConstants.CommonModulDivisor,
                Dislikes = (id % TestsConstants.CommonModulDivisor) / 2,
                IsDeleted = GetOddNumberAsFalse(id)
            };
        }

        private static List<Tag> GetCollectionOfTags(int count = TestsConstants.EntitiesPerRepesitory, int startingId = 1)
        {
            var collection = new List<Tag>();

            for (int i = startingId; i <= count + startingId; i++)
            {
                collection.Add(GetSingleTag(i));
            }

            return collection;
        }

        private static Tag GetSingleTag(int id)
        {
            return new Tag()
            {
                Id = id,
                Name = "Some Tag " + id,
                Albums = GetCollectionOfAlbums(TestsConstants.AlbumPerTag),
                Images = GetCollectionOfImages(TestsConstants.ImagesPerTag),
                IsDeleted = GetOddNumberAsFalse(id)
            };
        }

        private static bool GetOddNumberAsFalse(int number)
        {
            return number % 2 == 0 ? true : false;
        }
    }
}

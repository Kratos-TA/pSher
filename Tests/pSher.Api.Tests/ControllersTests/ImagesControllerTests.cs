namespace PSher.Api.Tests.ControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Constants;
    using Constants;
    using Controllers;
    using DataTransferModels.Images;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyTested.WebApi;
    using MyTested.WebApi.Exceptions;
    using Setups;

    [TestClass]
    public class ImagesControllerTests
    {
        private IControllerBuilder<ImagesController> controller;

        [TestInitialize]
        public void Init()
        {
            this.controller = MyWebApi
                .Controller<ImagesController>()
                .WithResolvedDependencies(DataServices.GetImagesService())
                .WithResolvedDependencies(DataServices.GetAlbumService())
                .WithResolvedDependencies(DataServices.GetTagsService());
        }

        [TestMethod]
        public void GetWithPageParamrtersShoudReturnDefautPageSizeImages()
        {
            controller
                .CallingAsync(c => c.Get(1, GlobalConstants.DefaultPageSize))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<List<ImageResponseModel>>()
                .Passing(model =>
                {
                    Assert.AreEqual(GlobalConstants.DefaultPageSize, model.Count);
                });
        }

        [TestMethod]
        public void GetWithIdShoudReturnExaxtImage()
        {
            controller
                .CallingAsync(c => c.Get("3"))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<ImageResponseModel>()
                .Passing(model =>
                {
                    Assert.AreEqual(3, model.Id);
                });
        }

        [TestMethod]
        public void GetWithNameUserAndTagShouldReturnThreeDifferentImages()
        {
            var name = TestsConstants.ImageBaseTitle;
            var user = TestsConstants.UserBaseUserName;
            var tags = TestsConstants.TagBaseName + "," + TestsConstants.TagBaseName;
            controller
                .CallingAsync(c => c.Get(name, user, tags, 1, 10))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<List<ImageResponseModel>>()
                .Passing(model =>
                {
                    Assert.IsTrue(model.Any(i => i.Title.IndexOf(name) > -1));
                    Assert.IsTrue(model.Any(i => i.AuthorName.IndexOf(user) > -1));
                    Assert.IsTrue(model.Any(i => i.Tags.Any(t => t.IndexOf(TestsConstants.TagBaseName.ToLower()) > -1)));
                });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCallAssertionException))]
        public void ShoudThrowWhenTryngToAddImageAsync()
        {
            byte[] bytes = File.ReadAllBytes(@"./MediaContent/image.jpg");

            var newImageInfo = new ImageInfoRequstModel()
            {
                OriginalName = TestsConstants.ImageInfoBaseOriginalName,
                OriginalExtension = TestsConstants.ImageBaseExstnsion,
                Base64Content = Convert.ToBase64String(bytes)
            };

            var imageRequestModel = new SaveImageRequestModel()
            {
                Title = TestsConstants.ImageBaseTitle,
                Description = TestsConstants.DescriptionBaseTextr,
                IsPrivate = false,
                Tags = TestsConstants.TagBaseName + "," + TestsConstants.TagBaseName + "Pesho",
                ImageInfo = newImageInfo
            };


            controller
                .CallingAsync(c => c.Post(imageRequestModel))
                .ShouldReturn()
                .HttpResponseMessage();
        }
    }
}



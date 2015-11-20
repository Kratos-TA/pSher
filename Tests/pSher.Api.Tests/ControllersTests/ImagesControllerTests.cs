namespace PSher.Api.Tests.ControllersTests
{
    using System.Collections.Generic;
    using Common.Constants;
    using Controllers;
    using DataTransferModels.Images;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyTested.WebApi;
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
        public void GetWithParamrtersShoudReturnDefautPageSize()
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
    }
}

namespace PSher.Api.Tests
{
    using Setups;
    using System.Reflection;

    using App_Start;
    using Common.Constants;
    using Data.Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using MyTested.WebApi;

    [TestClass]
    public class TestInit
    {
        [AssemblyInitialize]
        public static void Init(TestContext testContext)
        {
            //var repositories =  Repositories.Instance;

            //NinjectConfig.DependenciesRegistration = kernel =>
            //{
            //    kernel.Bind<IRepository<User>>()
            //    .ToConstant(repositories.GetUsersRepository());
            //};

            AutoMapperConfig.RegisterMappings(Assembly.Load(Assemblies.WebApi));
            MyWebApi.IsRegisteredWith(WebApiConfig.Register);
        }
    }
}

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PSher.Api.App_Start.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PSher.Api.App_Start.NinjectConfig), "Stop")]

namespace PSher.Api.App_Start
{
    using System;
    using System.Data.Entity;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using PSher.Api.Infrastructure;
    using PSher.Common.Constants;
    using PSher.Data;
    using PSher.Data.Contracts;

    public static class NinjectConfig
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                ObjectFactory.Initialize(kernel);
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel
                .Bind<DbContext>()
                .To<PSherDbContext>()
                .InRequestScope();

            kernel
                .Bind(typeof(IRepository<>))
                .To(typeof(EfGenericRepository<>));

            kernel
                .Bind(b => b.From(Assemblies.DataServices)
                    .SelectAllClasses()
                    .BindDefaultInterface());

            kernel
                .Bind(b => b.From(Assemblies.CommonServices)
                    .SelectAllClasses()
                    .BindDefaultInterface());

            kernel
             .Bind(b => b.From(Assemblies.LogicServices)
                 .SelectAllClasses()
                 .BindDefaultInterface());
        }
    }
}

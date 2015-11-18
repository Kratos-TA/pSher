
using System.Reflection;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using PSher.Api.App_Start;
using PSher.Common.Constants;

[assembly: OwinStartup(typeof(PSher.Api.Startup))]

namespace PSher.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            AutoMapperConfig.RegisterMappings(Assembly.Load(Assemblies.WebApi));

            this.ConfigureAuth(app);

            var httpConfig = new HttpConfiguration();

            WebApiConfig.Register(httpConfig);

            httpConfig.EnsureInitialized();

            app
            .UseNinjectMiddleware(NinjectConfig.CreateKernel)
            .UseNinjectWebApi(httpConfig);
        }
    }
}

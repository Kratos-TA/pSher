namespace PSher.Api
{
    using System.Reflection;
    using System.Web;
    using System.Web.Http;

    using PSher.Common.Constants;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            DatabaseConfig.Initialize();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}

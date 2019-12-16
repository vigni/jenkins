using Owin;
using Swashbuckle.Application;
using System.Web.Http;

namespace Cnam.UEGLG101.Journey.App
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            
            config.EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
                .EnableSwaggerUi();

            appBuilder.UseWebApi(config);
        }
    }
}

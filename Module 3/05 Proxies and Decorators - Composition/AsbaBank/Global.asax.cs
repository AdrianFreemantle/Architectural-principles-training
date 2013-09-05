using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Mvc.App_Start;

namespace AsbaBank.Presentation.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //As some members of our class are not yet familiar with Entity framework, we use a in memeory repository. 
        //This is declared static on the Mvc Applicatioon so that state is not lost between requests. 
        public static InMemoryDataStore DataStore { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitSeedData();
        }

        private void InitSeedData()
        {
            DataStore = new InMemoryDataStore();
            var unitOfWork = new InMemoryUnitOfWork(DataStore);

            unitOfWork.GetRepository<Client>()
                      .Add(new Client("Adrian Freemantle", "0845140900"));

            unitOfWork.Commit();
        }
    }
}
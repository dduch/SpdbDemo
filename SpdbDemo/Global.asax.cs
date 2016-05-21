using Castle.Windsor;
using Castle.Windsor.Installer;
using SpdbDemo.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Navigation.DataProviders;

namespace SpdbDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly WindsorContainer container;

        public MvcApplication()
        {
            this.container = new WindsorContainer();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            container.Install(FromAssembly.This());
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(container.Kernel);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GeoDataProvider.Initialize(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/stationsRoutesDB"));
        }
    }
}

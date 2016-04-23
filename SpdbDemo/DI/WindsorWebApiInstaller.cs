using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NavigationResolver.DataModels;
using NavigationResolver.DataProviders;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SpdbDemo.DI
{
    public class WindsorWebApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleScoped());

            container.Register(
                   Component.For<IGeoDataProvider>().ImplementedBy<GeoDataProvider>(),
                   Component.For<INetwork>().ImplementedBy<Network>(),
                   Component.For<SuperGraph>().ImplementedBy<SuperGraph>(),
                   Component.For<TravelMetric>().ImplementedBy<TravelMetric>()
                );
        }
    }
}
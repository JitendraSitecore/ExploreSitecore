using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Pipelines;
using System.Web.Routing;
using System.Web.Mvc;

namespace Sitecore.Commerce.Learning
{
    public class RegisterCustomRoutes
    {

        public void Process(PipelineArgs pipelineArgs)
        {
            RouteTable.Routes.MapRoute(
                "ProductSearch",
                "api/sitecore/ProductSearch/{action}/{category}/{pageNo}",
                new {Controller = "ProductSearch", pageNo = UrlParameter.Optional }
                );
        }

    }
}
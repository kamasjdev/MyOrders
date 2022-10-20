using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;

namespace MyOrders.Infrastructure.Conventions
{
    internal static class Extensions
    {
        public static MvcOptions UseDashedConventionInRouting(this MvcOptions options)
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new DashedConvention()));
            return options;
        }
    }
}

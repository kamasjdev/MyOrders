using Humanizer;
using Microsoft.AspNetCore.Routing;

namespace MyOrders.Infrastructure.Conventions
{
    internal class DashedConvention : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null) { return null; }

            var routeName = value.ToString().Kebaberize();

            return routeName;
        }
    }
}

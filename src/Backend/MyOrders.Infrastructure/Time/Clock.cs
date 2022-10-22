using MyOrders.Core.Services;

namespace MyOrders.Infrastructure.Time
{
    internal sealed class Clock : IClock
    {
        public DateTime CurrentDateTime()
            => DateTime.UtcNow;
    }
}

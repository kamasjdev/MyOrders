using MyOrders.Core.Services;
using System;

namespace MyOrders.UnitTests.Stubs
{
    internal sealed class ClockStub : IClock
    {
        public DateTime CurrentDateTime()
        {
            return new DateTime(2022, 10, 22, 16, 0, 10);
        }
    }
}

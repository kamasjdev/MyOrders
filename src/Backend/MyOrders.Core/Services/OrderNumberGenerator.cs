using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using System.Text;

namespace MyOrders.Core.Services
{
    internal sealed class OrderNumberGenerator : IOrderNumberGenerator
    {
        private readonly IClock _clock;

        public OrderNumberGenerator(IClock clock)
        {
            _clock = clock;
        }

        public OrderNumber Generate(OrderNumber lastOrderNumber)
        {
            // PATTERN: ORDER/year/month/day/numbers
            long number = 1;

            if (lastOrderNumber is not null)
            {
                if (lastOrderNumber.Value.Length < 18)
                {
                    throw new DomainException($"Given invalid OrderNumber: '{lastOrderNumber}'");
                }

                var stringNumber = lastOrderNumber.Value[17..];//18
                var parsed = long.TryParse(stringNumber, out number);

                if (!parsed)
                {
                    throw new DomainException($"Given invalid OrderNumber: '{lastOrderNumber}'");
                }

                number++;
            }

            var currentDate = _clock.CurrentDateTime();
            var orderNumber = new StringBuilder("ORDER/")
                .Append(currentDate.Year).Append('/').Append(currentDate.Month.ToString("d2"))
                .Append('/').Append(currentDate.Day.ToString("00")).Append('/').Append(number).ToString();

            return orderNumber;
        }
    }
}

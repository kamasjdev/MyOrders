namespace MyOrders.Application.DTO
{
    public record CustomerDetailsDto : CustomerDto
    {
        public AddressDto Address { get; init; }
        public ContactDataDto ContactData { get; init; }

        public CustomerDetailsDto(int id, string firstName, string lastName, AddressDto address, ContactDataDto contactData)
            : base(id, firstName, lastName)
        {
            Address = address;
            ContactData = contactData;
        }
    }
}

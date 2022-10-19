namespace MyOrders.Application.DTO
{
    public record UpdateCustomerDto(int Id, string FirstName, string LastName, int AddressId , int ContactDataId);
}

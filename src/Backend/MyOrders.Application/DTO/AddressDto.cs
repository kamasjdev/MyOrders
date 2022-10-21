namespace MyOrders.Application.DTO
{
    public record AddressDto(int Id, string StreetName, string CityName, string CountryName, int BuildingNumber, string ZipCode, int? FlatNumber = null);
}

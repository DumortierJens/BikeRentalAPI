namespace BikeRentalAPI.GraphQL.Queries;

public class Queries
{
    public async Task<List<Rental>> GetRentalsByLocation([Service] IRentalService rentalService, string locationId) => await rentalService.GetRentalsByLocation(locationId);
    public async Task<List<RentalLocation>> GetLocations([Service] IRentalLocationService rentalLocationService) => await rentalLocationService.GetLocations();
    public async Task<List<BikePrice>> LocationBikePrices([Service] IRentalLocationService rentalLocationService, string locationId) => await rentalLocationService.GetBikePricesByLocation(locationId);
}

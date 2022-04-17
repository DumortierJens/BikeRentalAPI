namespace BikeRentalAPI.GraphQL.Queries;

public class Queries
{
    public async Task<List<Rental>> GetRentalsByLocation([Service] IRentalService rentalService, string locationId) => await rentalService.GetRentalsByLocation(locationId);
    // public async Task<Rental> GetRental([Service] IRentalService rentalService, string rentalId) => await rentalService.GetRental(rentalId);
}

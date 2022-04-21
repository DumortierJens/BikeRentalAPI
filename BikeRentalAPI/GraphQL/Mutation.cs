namespace BikeRentalAPI.GraphQL.Mutations;

public class Mutation
{
    public async Task<StartRentalPayload> StartRental([Service] IRentalService rentalService, StartRentalInput input)
    {
        var newRental = new Rental()
        {
            Name = input.name,
            Tel = input.tel,
            Location = new RentalLocation() { Id = input.locationId },
            Bike = new Bike() { Id = input.bikeId }
        };
        var rental = await rentalService.StartRental(newRental);
        return new StartRentalPayload(rental);
    }

    public async Task<StopRentalPayload> StopRental([Service] IRentalService rentalService, StopRentalInput input)
    {
        var rental = await rentalService.StopRental(input.rentalId);
        return new StopRentalPayload(rental);
    }

    public async Task<UpdateRentalDetailsPayload> UpdateRentalDetails([Service] IRentalService rentalService, UpdateRentalDetailsInput input)
    {
        var updatedRentalDetails = new Rental()
        {
            Id = input.rentalId,
            Name = input.name,
            Tel = input.tel
        };
        var rental = await rentalService.UpdateRentalDetails(updatedRentalDetails);
        return new UpdateRentalDetailsPayload(rental);
    }
}

namespace BikeRentalAPI.GraphQL.Mutations;

public class Mutation
{
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

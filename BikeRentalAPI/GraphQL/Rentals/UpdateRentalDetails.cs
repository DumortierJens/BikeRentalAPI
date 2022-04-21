namespace BikeRentalAPI.GraphQL.Mutations;
public record UpdateRentalDetailsInput(string rentalId, string name, string tel);
public record UpdateRentalDetailsPayload(Rental rental);
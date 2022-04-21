namespace BikeRentalAPI.GraphQL.Mutations;
public record StopRentalInput(string rentalId);
public record StopRentalPayload(Rental rental);
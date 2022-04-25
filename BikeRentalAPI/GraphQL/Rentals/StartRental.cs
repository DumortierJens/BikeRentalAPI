namespace BikeRentalAPI.GraphQL.Mutations;
public record StartRentalInput(string name, string tel, string locationId, string bikeId);
public record StartRentalPayload(Rental rental);
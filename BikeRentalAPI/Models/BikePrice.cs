namespace BikeRentalAPI.Models;

public class BikePrice
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? LocationId { get; set; }
    public string? BikeId { get; set; }
    public Prices? Prices { get; set; }
}


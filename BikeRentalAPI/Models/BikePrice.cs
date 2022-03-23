namespace BikeRentalAPI.Models;

public class BikePrice
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public Location? Location { get; set; }
    public Bike? Bike { get; set; }
    public Prices? Prices { get; set; }
}


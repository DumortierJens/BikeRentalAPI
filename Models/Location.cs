namespace BikeRentalAPI.Models;

public class Location
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? City { get; set; }
}


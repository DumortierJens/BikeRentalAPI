namespace BikeRentalAPI.Models;

public class BikeType
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, float> Prices { get; set; }
}


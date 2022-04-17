namespace BikeRentalAPI.Models;

public class Rental
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Tel { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public RentalLocation? Location { get; set; }
    public Bike? Bike { get; set; }
    public double? Price { get; set; }
}


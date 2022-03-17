namespace BikeRentalAPI.Configuration;

public class DatabaseSettings
{
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? BikeTypeCollection { get; set; }
    public string? LocationCollection { get; set; }
    public string? RentalCollection { get; set; }
}
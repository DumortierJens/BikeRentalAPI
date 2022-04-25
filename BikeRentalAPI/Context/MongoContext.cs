namespace BikeRentalAPI.Context;

public interface IMongoContext
{
    IMongoClient Client { get; }
    IMongoDatabase Database { get; }
    IMongoCollection<Bike> BikeCollection { get; }
    IMongoCollection<BikePrice> BikePriceCollection { get; }
    IMongoCollection<RentalLocation> LocationCollection { get; }
    IMongoCollection<Rental> RentalCollection { get; }
}

public class MongoContext : IMongoContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly DatabaseSettings _settings;

    public IMongoClient Client
    {
        get
        {
            return _client;
        }
    }
    public IMongoDatabase Database => _database;

    public MongoContext(IOptions<DatabaseSettings> settings)
    {
        _settings = settings.Value;
        _client = new MongoClient(_settings.ConnectionString);
        _database = _client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<Bike> BikeCollection
    {
        get
        {
            return _database.GetCollection<Bike>(_settings.BikeCollection);
        }
    }

    public IMongoCollection<BikePrice> BikePriceCollection
    {
        get
        {
            return _database.GetCollection<BikePrice>(_settings.BikePriceCollection);
        }
    }

    public IMongoCollection<RentalLocation> LocationCollection
    {
        get
        {
            return _database.GetCollection<RentalLocation>(_settings.LocationCollection);
        }
    }

    public IMongoCollection<Rental> RentalCollection
    {
        get
        {
            return _database.GetCollection<Rental>(_settings.RentalCollection);
        }
    }
}
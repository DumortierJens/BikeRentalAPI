namespace BikeRentalAPI.Context;

public interface IMongoContext
{
    IMongoClient Client { get; }
    IMongoDatabase Database { get; }
    IMongoCollection<BikeType> BikeTypeCollection { get; }
    IMongoCollection<Location> LocationCollection { get; }
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

    public IMongoCollection<BikeType> BikeTypeCollection
    {
        get
        {
            return _database.GetCollection<BikeType>(_settings.BikeTypeCollection);
        }
    }

    public IMongoCollection<Location> LocationCollection
    {
        get
        {
            return _database.GetCollection<Location>(_settings.LocationCollection);
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
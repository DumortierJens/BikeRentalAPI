namespace BikeRentalAPI.Repositories;

public interface ILocationRepository
{
    Task<Location> AddLocation(Location location);
    Task<Location> GetLocation(string id);
    Task<List<Location>> GetLocations();
    Task<Location> UpdateLocation(Location location);
}

public class LocationRepository : ILocationRepository
{
    private readonly IMongoContext _context;

    public LocationRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<Location>> GetLocations() => await _context.LocationCollection.Find(_ => true).ToListAsync();

    public async Task<Location> GetLocation(string id) => await _context.LocationCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();

    public async Task<Location> AddLocation(Location location)
    {
        await _context.LocationCollection.InsertOneAsync(location);
        return location;
    }

    public async Task<Location> UpdateLocation(Location location)
    {
        var filter = Builders<Location>.Filter.Eq("Id", location.Id);
        var update = Builders<Location>.Update.Set("Name", location.Name).Set("City", location.City);
        var opts = new FindOneAndUpdateOptions<Location>() { ReturnDocument = ReturnDocument.After };
        return await _context.LocationCollection.FindOneAndUpdateAsync(filter, update, opts);
    }
}


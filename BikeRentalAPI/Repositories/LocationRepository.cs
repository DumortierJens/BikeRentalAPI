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
        var filter = Builders<Location>.Filter.Eq("id", location.Id);
        var update = Builders<Location>.Update.Set("name", location.Name);
        var result = await _context.LocationCollection.UpdateOneAsync(filter, update);
        return location;
    }
}


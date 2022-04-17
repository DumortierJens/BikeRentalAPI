namespace BikeRentalAPI.Repositories;

public interface ILocationRepository
{
    Task<RentalLocation> AddLocation(RentalLocation location);
    Task<RentalLocation> GetLocation(string id);
    Task<List<RentalLocation>> GetLocations();
    Task<RentalLocation> UpdateLocation(RentalLocation location);
}

public class LocationRepository : ILocationRepository
{
    private readonly IMongoContext _context;

    public LocationRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<RentalLocation>> GetLocations() => await _context.LocationCollection.Find(_ => true).ToListAsync();

    public async Task<RentalLocation> GetLocation(string id) => await _context.LocationCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();

    public async Task<RentalLocation> AddLocation(RentalLocation location)
    {
        await _context.LocationCollection.InsertOneAsync(location);
        return location;
    }

    public async Task<RentalLocation> UpdateLocation(RentalLocation location)
    {
        var filter = Builders<RentalLocation>.Filter.Eq("Id", location.Id);
        var update = Builders<RentalLocation>.Update.Set("Name", location.Name).Set("City", location.City);
        var opts = new FindOneAndUpdateOptions<RentalLocation>() { ReturnDocument = ReturnDocument.After };
        return await _context.LocationCollection.FindOneAndUpdateAsync(filter, update, opts);
    }
}


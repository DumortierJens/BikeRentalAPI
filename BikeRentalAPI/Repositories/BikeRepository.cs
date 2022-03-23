namespace BikeRentalAPI.Repositories;

public interface IBikeRepository
{
    Task<Bike> AddBike(Bike bike);
    Task<Bike> GetBike(string id);
    Task<List<Bike>> GetBikes();
    Task<Bike> UpdateBike(Bike bike);
}

public class BikeRepository : IBikeRepository
{
    private readonly IMongoContext _context;

    public BikeRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<Bike>> GetBikes() => await _context.BikeCollection.Find(_ => true).ToListAsync();

    public async Task<Bike> GetBike(string id) => await _context.BikeCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();

    public async Task<Bike> AddBike(Bike bikeType)
    {
        await _context.BikeCollection.InsertOneAsync(bikeType);
        return bikeType;
    }

    public async Task<Bike> UpdateBike(Bike bikeType)
    {
        var filter = Builders<Bike>.Filter.Eq("id", bikeType.Id);
        var update = Builders<Bike>.Update.Set("name", bikeType.Name);
        var result = await _context.BikeCollection.UpdateOneAsync(filter, update);
        return bikeType;
    }
}
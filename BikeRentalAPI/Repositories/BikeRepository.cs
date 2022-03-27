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

    public async Task<Bike> AddBike(Bike bike)
    {
        await _context.BikeCollection.InsertOneAsync(bike);
        return bike;
    }

    public async Task<Bike> UpdateBike(Bike bike)
    {
        var filter = Builders<Bike>.Filter.Eq("Id", bike.Id);
        var update = Builders<Bike>.Update.Set("Name", bike.Name);
        var opts = new FindOneAndUpdateOptions<Bike>() { ReturnDocument = ReturnDocument.After };
        return await _context.BikeCollection.FindOneAndUpdateAsync(filter, update, opts);
    }
}
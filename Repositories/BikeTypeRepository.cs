namespace BikeRentalAPI.Repositories;

public interface IBikeTypeRepository
{
    Task<BikeType> AddBikeTypeAsync(BikeType bikeType);
    Task<List<BikeType>> GetBikeTypesAsync();
    Task<BikeType> UpdateBikeTypeAsync(BikeType bikeType);
}

public class BikeTypeRepository : IBikeTypeRepository
{
    private readonly IMongoContext _context;

    public BikeTypeRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<BikeType>> GetBikeTypesAsync() => await _context.BikeTypeCollection.Find(_ => true).ToListAsync();

    public async Task<BikeType> AddBikeTypeAsync(BikeType bikeType)
    {
        await _context.BikeTypeCollection.InsertOneAsync(bikeType);
        return bikeType;
    }

    public async Task<BikeType> UpdateBikeTypeAsync(BikeType bikeType)
    {
        var filter = Builders<BikeType>.Filter.Eq("id", bikeType.Id);
        var update = Builders<BikeType>.Update.Set("name", bikeType.Name).Set("prices", bikeType.Prices);
        var result = await _context.BikeTypeCollection.UpdateOneAsync(filter, update);
        return bikeType;
    }
}
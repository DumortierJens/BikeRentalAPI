namespace BikeRentalAPI.Repositories;

public interface IBikePriceRepository
{
    Task<BikePrice> AddBikePrice(BikePrice bikePrice);
    Task<BikePrice> GetBikePrice(string locationId, string bikeId);
    Task<List<BikePrice>> GetBikePricesByLocation(string locationId);
    Task<BikePrice> UpdateBikePrice(BikePrice bikePrice);
}

public class BikePriceRepository : IBikePriceRepository
{
    private readonly IMongoContext _context;

    public BikePriceRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<BikePrice>> GetBikePricesByLocation(string locationId) => await _context.BikePriceCollection.Find(_ => (_.LocationId == locationId)).ToListAsync();

    public async Task<BikePrice> GetBikePrice(string locationId, string bikeId) => await _context.BikePriceCollection.Find(_ => (_.LocationId == locationId && _.BikeId == bikeId)).FirstOrDefaultAsync();

    public async Task<BikePrice> AddBikePrice(BikePrice bikePrice)
    {
        await _context.BikePriceCollection.InsertOneAsync(bikePrice);
        return bikePrice;
    }

    public async Task<BikePrice> UpdateBikePrice(BikePrice bikePrice)
    {
        var filter = Builders<Bike>.Filter.Eq("id", bikePrice.Id);
        var update = Builders<Bike>.Update.Set("prices", bikePrice.Prices);
        var result = await _context.BikeCollection.UpdateOneAsync(filter, update);
        return bikePrice;
    }
}

namespace BikeRentalAPI.Repositories;

public interface IBikePriceRepository
{
    Task<BikePrice> AddBikePrice(BikePrice bikePrice);
    Task<BikePrice> GetBikePrice(Location location, Bike bike);
    Task<List<BikePrice>> GetBikePricesByLocation(Location location);
    Task<BikePrice> UpdateBikePrice(BikePrice bikePrice);
}

public class BikePriceRepository : IBikePriceRepository
{
    private readonly IMongoContext _context;

    public BikePriceRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<BikePrice>> GetBikePricesByLocation(Location location) => await _context.BikePriceCollection.Find(_ => (_.Location != null && _.Location.Id == location.Id)).ToListAsync();

    public async Task<BikePrice> GetBikePrice(Location location, Bike bike) => await _context.BikePriceCollection.Find(_ => (_.Location != null && _.Location.Id == location.Id && _.Bike != null && _.Bike.Id == bike.Id)).FirstOrDefaultAsync();

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

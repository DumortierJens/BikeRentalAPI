namespace BikeRentalAPI.Repositories;

public interface IRentalRepository
{
    Task<Rental> AddRental(Rental rental);
    Task<Rental> EndRental(Rental rental);
    Task<Rental> GetRental(string id);
    Task<List<Rental>> GetRentalsByLocation(string locationId);
    Task<Rental> UpdateRentalDetails(Rental rental);
}

public class RentalRepository : IRentalRepository
{
    private readonly IMongoContext _context;

    public RentalRepository(IMongoContext context)
    {
        _context = context;
    }

    public Task<List<Rental>> GetRentalsByLocation(string locationId) => _context.RentalCollection.Find(_ => (_.Location != null && _.Location.Id == locationId)).ToListAsync();

    public Task<Rental> GetRental(string id) => _context.RentalCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();

    public async Task<Rental> AddRental(Rental rental)
    {
        await _context.RentalCollection.InsertOneAsync(rental);
        return rental;
    }

    public async Task<Rental> EndRental(Rental rental)
    {
        var filter = Builders<Rental>.Filter.Eq("Id", rental.Id);
        var update = Builders<Rental>.Update.Set("EndTime", rental.EndTime).Set("Price", rental.Price);
        var opts = new FindOneAndUpdateOptions<Rental>() { ReturnDocument = ReturnDocument.After };
        return await _context.RentalCollection.FindOneAndUpdateAsync(filter, update, opts);
    }

    public async Task<Rental> UpdateRentalDetails(Rental rental)
    {
        var filter = Builders<Rental>.Filter.Eq("Id", rental.Id);
        var update = Builders<Rental>.Update.Set("Name", rental.Name).Set("Tel", rental.Tel);
        var opts = new FindOneAndUpdateOptions<Rental>() { ReturnDocument = ReturnDocument.After };
        return await _context.RentalCollection.FindOneAndUpdateAsync(filter, update, opts);
    }
}


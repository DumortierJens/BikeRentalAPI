namespace BikeRentalAPI_Testing.Helpers;

public class FakeRentalRepository : IRentalRepository
{
    public static List<Rental> _rentals = new List<Rental>();

    public static void AddFakeRental(Rental rental) => _rentals.Add(rental);

    public Task<List<Rental>> GetRentalsByLocation(string locationId) => Task.FromResult(_rentals.FindAll(_ => (_.Location != null && _.Location.Id == locationId)));

    public Task<Rental> GetRental(string id) => Task.FromResult(_rentals.FirstOrDefault(_ => _.Id == id));

    public Task<Rental> AddRental(Rental rental)
    {
        rental.Id = ObjectId.GenerateNewId().ToString();
        _rentals.Add(rental);
        return Task.FromResult(rental);
    }

    public Task<Rental> EndRental(Rental rental)
    {
        var oldRental = _rentals.FirstOrDefault(_ => _.Id == rental.Id);
        if (oldRental != null)
        {
            oldRental.EndTime = rental.EndTime;
            oldRental.Price = rental.Price;
        }
        return Task.FromResult(oldRental);
    }

    public Task<Rental> UpdateRentalDetails(Rental rental)
    {
        var oldRental = _rentals.FirstOrDefault(_ => _.Id == rental.Id);
        if (oldRental != null)
        {
            oldRental.Name = rental.Name;
            oldRental.Tel = rental.Tel;
        }
        return Task.FromResult(oldRental);
    }
}


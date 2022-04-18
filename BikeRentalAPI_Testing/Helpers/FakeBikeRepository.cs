namespace BikeRentalAPI_Testing.Helpers;

public class FakeBikeRepository : IBikeRepository
{
    public static List<Bike> _bikes = new List<Bike>();

    public static void AddFakeBike(Bike bike) => _bikes.Add(bike);

    public Task<List<Bike>> GetBikes() => Task.FromResult(_bikes);

    public Task<Bike> GetBike(string id) => Task.FromResult(_bikes.FirstOrDefault(_ => _.Id == id));

    public Task<Bike> AddBike(Bike bike)
    {
        _bikes.Add(bike);
        return Task.FromResult(bike);
    }

    public Task<Bike> UpdateBike(Bike bike)
    {
        var oldBike = _bikes.FirstOrDefault(_ => _.Id == bike.Id);
        if (oldBike != null) oldBike.Name = bike.Name;
        return Task.FromResult(oldBike);
    }
}


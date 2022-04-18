namespace BikeRentalAPI_Testing.Helpers;

public class FakeLocationRepository : ILocationRepository
{
    public static List<RentalLocation> _locations = new List<RentalLocation>();

    public static void AddFakeLocation(RentalLocation location) => _locations.Add(location);

    public Task<List<RentalLocation>> GetLocations() => Task.FromResult(_locations);

    public Task<RentalLocation> GetLocation(string id) => Task.FromResult(_locations.FirstOrDefault(_ => _.Id == id));

    public Task<RentalLocation> AddLocation(RentalLocation location)
    {
        _locations.Add(location);
        return Task.FromResult(location);
    }

    public Task<RentalLocation> UpdateLocation(RentalLocation location)
    {
        var oldLocation = _locations.FirstOrDefault(_ => _.Id == location.Id);
        if (oldLocation != null) oldLocation.Name = location.Name;
        return Task.FromResult(oldLocation);
    }
}


namespace BikeRentalAPI.Services;

public interface IRentalService
{
    Task<Bike> AddBike(Bike bike);
    Task<Location> AddLocation(Location location);
    Task<Bike> GetBike(string id);
    Task<List<Bike>> GetBikes();
    Task<Location> GetLocation(string id);
    Task<List<Location>> GetLocations();
    Task<Bike> UpdateBike(Bike bike);
    Task<Location> UpdateLocation(Location location);
}

public class RentalService : IRentalService
{
    private readonly IBikeRepository _bikeRepository;
    private readonly ILocationRepository _locationRepository;

    public RentalService(IBikeRepository bikeRepository, ILocationRepository locationRepository)
    {
        _bikeRepository = bikeRepository;
        _locationRepository = locationRepository;
    }


    #region Bikes

    public async Task<List<Bike>> GetBikes() => await _bikeRepository.GetBikes();

    public async Task<Bike> GetBike(string id) => await _bikeRepository.GetBike(id);

    public async Task<Bike> AddBike(Bike bike)
    {
        bike.Id = null;
        return await _bikeRepository.AddBike(bike);
    }

    public async Task<Bike> UpdateBike(Bike bike) => await _bikeRepository.UpdateBike(bike);

    #endregion

    #region Locations

    public async Task<List<Location>> GetLocations() => await _locationRepository.GetLocations();

    public async Task<Location> GetLocation(string id) => await _locationRepository.GetLocation(id);

    public async Task<Location> AddLocation(Location location)
    {
        location.Id = null;
        return await _locationRepository.AddLocation(location);
    }

    public async Task<Location> UpdateLocation(Location location) => await _locationRepository.UpdateLocation(location);

    #endregion
}


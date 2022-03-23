namespace BikeRentalAPI.Services;

public interface IRentalService
{
    Task<BikeType> AddBikeTypeAsync(BikeType bikeType);
    Task<Location> AddLocationAsync(Location location);
    Task<BikeType> GetBikeTypeAsync(string id);
    Task<List<BikeType>> GetBikeTypesAsync();
    Task<Location> GetLocationAsync(string id);
    Task<List<Location>> GetLocationsAsync();
    Task<BikeType> UpdateBikeTypeAsync(BikeType bikeType);
    Task<Location> UpdateLocationAsync(Location location);
}

public class RentalService : IRentalService
{
    private readonly IBikeTypeRepository _bikeTypeRepository;
    private readonly ILocationRepository _locationRepository;

    public RentalService(IBikeTypeRepository bikeTypeRepository, ILocationRepository locationRepository)
    {
        _bikeTypeRepository = bikeTypeRepository;
        _locationRepository = locationRepository;
    }

    #region Bike Types

    public async Task<List<BikeType>> GetBikeTypesAsync() => await _bikeTypeRepository.GetBikeTypesAsync();

    public async Task<BikeType> GetBikeTypeAsync(string id) => await _bikeTypeRepository.GetBikeTypeAsync(id);

    public async Task<BikeType> AddBikeTypeAsync(BikeType bikeType)
    {
        bikeType.Id = null;
        return await _bikeTypeRepository.AddBikeTypeAsync(bikeType);
    }

    public async Task<BikeType> UpdateBikeTypeAsync(BikeType bikeType) => await _bikeTypeRepository.UpdateBikeTypeAsync(bikeType);

    #endregion

    #region Locations

    public async Task<List<Location>> GetLocationsAsync() => await _locationRepository.GetLocationsAsync();

    public async Task<Location> GetLocationAsync(string id) => await _locationRepository.GetLocationAsync(id);

    public async Task<Location> AddLocationAsync(Location location)
    {
        location.Id = null;
        return await _locationRepository.AddLocationAsync(location);
    }

    public async Task<Location> UpdateLocationAsync(Location location) => await _locationRepository.UpdateLocationAsync(location);

    #endregion
}


namespace BikeRentalAPI.Services;

public interface IRentalLocationService
{
    Task<Bike> AddBike(Bike bike);
    Task<Location> AddLocation(Location location);
    Task<Bike> GetBike(string id);
    Task<BikePrice> GetBikePrice(Location location, Bike bike);
    Task<List<BikePrice>> GetBikePricesByLocation(Location location);
    Task<List<Bike>> GetBikes();
    Task<Location> GetLocation(string id);
    Task<List<Location>> GetLocations();
    Task<Bike> UpdateBike(Bike bike);
    Task<Location> UpdateLocation(Location location);
    Task<BikePrice> UpdateOrAddBikePrice(BikePrice bikePrice);
}

public class RentalLocationService : IRentalLocationService
{
    private readonly IBikeRepository _bikeRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IBikePriceRepository _bikePriceRepository;

    public RentalLocationService(IBikeRepository bikeRepository, ILocationRepository locationRepository, IBikePriceRepository bikePriceRepository)
    {
        _bikeRepository = bikeRepository;
        _locationRepository = locationRepository;
        _bikePriceRepository = bikePriceRepository;
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



    #region Bike Prices

    public async Task<List<BikePrice>> GetBikePricesByLocation(Location location) => await _bikePriceRepository.GetBikePricesByLocation(location);

    public async Task<BikePrice> GetBikePrice(Location location, Bike bike) => await _bikePriceRepository.GetBikePrice(location, bike);

    public async Task<BikePrice> UpdateOrAddBikePrice(BikePrice bikePrice)
    {
        BikePrice _bikePrice = await _bikePriceRepository.GetBikePrice(bikePrice.Location, bikePrice.Bike);

        if (_bikePrice == null)
        {
            bikePrice.Id = null;
            return await _bikePriceRepository.AddBikePrice(bikePrice);
        }
        else
        {
            return await _bikePriceRepository.UpdateBikePrice(bikePrice);
        }
    }

    #endregion
}


namespace BikeRentalAPI.Services;

public interface IRentalLocationService
{
    Task<Bike> AddBike(Bike bike);
    Task<BikePrice> AddBikePrice(BikePrice bikePrice);
    Task<RentalLocation> AddLocation(RentalLocation location);
    Task<Bike> GetBike(string id);
    Task<BikePrice> GetBikePrice(string locationId, string bikeId);
    Task<List<BikePrice>> GetBikePricesByLocation(string locationId);
    Task<List<Bike>> GetBikes();
    Task<RentalLocation> GetLocation(string id);
    Task<List<RentalLocation>> GetLocations();
    Task<Bike> UpdateBike(Bike bike);
    Task<BikePrice> UpdateBikePrice(BikePrice bikePrice);
    Task<RentalLocation> UpdateLocation(RentalLocation location);
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

    public async Task<List<RentalLocation>> GetLocations() => await _locationRepository.GetLocations();

    public async Task<RentalLocation> GetLocation(string id) => await _locationRepository.GetLocation(id);

    public async Task<RentalLocation> AddLocation(RentalLocation location)
    {
        location.Id = null;
        return await _locationRepository.AddLocation(location);
    }

    public async Task<RentalLocation> UpdateLocation(RentalLocation location) => await _locationRepository.UpdateLocation(location);

    #endregion



    #region Bike Prices

    public async Task<List<BikePrice>> GetBikePricesByLocation(string locationId) => await _bikePriceRepository.GetBikePricesByLocation(locationId);

    public async Task<BikePrice> GetBikePrice(string locationId, string bikeId) => await _bikePriceRepository.GetBikePrice(locationId, bikeId);

    public async Task<BikePrice> AddBikePrice(BikePrice bikePrice)
    {
        bikePrice.Id = null;

        // Check for bikeprice
        if (await _bikePriceRepository.GetBikePrice(bikePrice.LocationId, bikePrice.BikeId) != null)
            throw new ArgumentException("Bikeprice already exists");

        // Check BikeId
        if (await _bikeRepository.GetBike(bikePrice.BikeId) == null)
            throw new ArgumentException("Bike not found");

        // Check LocationId
        if (await _locationRepository.GetLocation(bikePrice.LocationId) == null)
            throw new ArgumentException("Location not found");

        return await _bikePriceRepository.AddBikePrice(bikePrice);
    }

    public async Task<BikePrice> UpdateBikePrice(BikePrice bikePrice) => await _bikePriceRepository.UpdateBikePrice(bikePrice);

    #endregion
}


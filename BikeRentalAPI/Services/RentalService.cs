namespace BikeRentalAPI.Services;

public interface IRentalService
{

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

}


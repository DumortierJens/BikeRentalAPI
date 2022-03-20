namespace BikeRentalAPI.Services;

public interface IRentalService
{
    Task<BikeType> AddBikeTypeAsync(BikeType bikeType);
    Task<BikeType> GetBikeTypeAsync(string id);
    Task<List<BikeType>> GetBikeTypesAsync();
    Task<BikeType> UpdateBikeTypeAsync(BikeType bikeType);
}

public class RentalService : IRentalService
{
    private readonly IBikeTypeRepository _bikeTypeRepository;

    public RentalService(IBikeTypeRepository bikeTypeRepository)
    {
        _bikeTypeRepository = bikeTypeRepository;
    }

    #region Bike Types

    public async Task<List<BikeType>> GetBikeTypesAsync() => await _bikeTypeRepository.GetBikeTypesAsync();

    public async Task<BikeType> GetBikeTypeAsync(string id) => await _bikeTypeRepository.GetBikeTypeAsync(id);

    public async Task<BikeType> AddBikeTypeAsync(BikeType bikeType) => await _bikeTypeRepository.AddBikeTypeAsync(bikeType);

    public async Task<BikeType> UpdateBikeTypeAsync(BikeType bikeType) => await _bikeTypeRepository.UpdateBikeTypeAsync(bikeType);

    #endregion
}


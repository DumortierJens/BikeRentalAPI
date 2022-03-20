namespace BikeRentalAPI_Testing.Helpers;

public class FakeBikeTypeRepository : IBikeTypeRepository
{
    public static List<BikeType> _bikeTypes = new List<BikeType>();

    public Task<BikeType> AddBikeTypeAsync(BikeType bikeType)
    {
        _bikeTypes.Add(bikeType);
        return Task.FromResult(bikeType);
    }

    public Task<BikeType?> GetBikeTypeAsync(string id)
    {
        var bikeType = _bikeTypes.SingleOrDefault(_ => _.Id == id);
        return Task.FromResult(bikeType);
    }

    public Task<List<BikeType>> GetBikeTypesAsync()
    {
        return Task.FromResult(_bikeTypes);
    }

    public Task<BikeType> UpdateBikeTypeAsync(BikeType bikeType)
    {
        _bikeTypes.Remove(bikeType);
        _bikeTypes.Add(bikeType);
        return Task.FromResult(bikeType);
    }
}


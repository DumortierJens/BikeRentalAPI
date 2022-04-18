namespace BikeRentalAPI_Testing.Helpers;

public class FakeBikePriceRepository : IBikePriceRepository
{
    public static List<BikePrice> _bikePrices = new List<BikePrice>();

    public static void AddFakeBikePrice(BikePrice bikePrice) => _bikePrices.Add(bikePrice);

    public Task<List<BikePrice>> GetBikePricesByLocation(string locationId) => Task.FromResult(_bikePrices.FindAll(_ => _.LocationId == locationId));

    public Task<BikePrice> GetBikePrice(string locationId, string bikeId) => Task.FromResult(_bikePrices.FirstOrDefault(_ => (_.LocationId == locationId && _.BikeId == bikeId)));

    public Task<BikePrice> AddBikePrice(BikePrice bikePrice)
    {
        _bikePrices.Add(bikePrice);
        return Task.FromResult(bikePrice);
    }

    public Task<BikePrice> UpdateBikePrice(BikePrice bikePrice)
    {
        var oldBikePrice = _bikePrices.FirstOrDefault(_ => _.Id == bikePrice.Id);
        if (oldBikePrice != null) oldBikePrice.Prices = bikePrice.Prices;
        return Task.FromResult(oldBikePrice);
    }
}


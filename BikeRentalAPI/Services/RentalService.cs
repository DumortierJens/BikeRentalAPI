namespace BikeRentalAPI.Services;

public interface IRentalService
{
    Task<Rental> StopRental(string rentalId);
    Task<Rental> GetRental(string id);
    Task<List<Rental>> GetRentalsByLocation(string locationId);
    Task<Rental> StartRental(Rental rental);
    Task<Rental> UpdateRentalDetails(Rental rental);
    Task<double> CalculatePrice(Rental rental);
}

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IBikeRepository _bikeRepository;
    private readonly IBikePriceRepository _bikePriceRepository;

    public RentalService(IRentalRepository rentalRepository, ILocationRepository locationRepository, IBikeRepository bikeRepository, IBikePriceRepository bikePriceRepository)
    {
        _rentalRepository = rentalRepository;
        _locationRepository = locationRepository;
        _bikeRepository = bikeRepository;
        _bikePriceRepository = bikePriceRepository;
    }

    public async Task<List<Rental>> GetRentalsByLocation(string locationId) => await _rentalRepository.GetRentalsByLocation(locationId);

    public async Task<Rental> GetRental(string id) => await _rentalRepository.GetRental(id);

    public async Task<Rental> StartRental(Rental rental)
    {
        rental.Id = null;
        rental.StartTime = DateTime.Now;
        rental.EndTime = null;
        rental.Price = null;

        if (rental.Location != null) rental.Location = await _locationRepository.GetLocation(rental.Location.Id);
        if (rental.Location == null)
            throw new ArgumentException("Location not found");

        if (rental.Bike != null) rental.Bike = await _bikeRepository.GetBike(rental.Bike.Id);
        if (rental.Bike == null)
            throw new ArgumentException("Bike not found");

        var bikePrice = await _bikePriceRepository.GetBikePrice(rental.Location.Id, rental.Bike.Id);
        if (bikePrice == null)
            throw new ArgumentException("Bike not found in location");

        return await _rentalRepository.AddRental(rental);
    }

    public async Task<Rental> StopRental(string rentalId)
    {
        if (rentalId == null)
            throw new ArgumentException();

        var rental = await GetRental(rentalId);
        if (rental == null)
            return null;

        rental.EndTime = DateTime.Now;
        rental.Price = await CalculatePrice(rental);

        return await _rentalRepository.EndRental(rental);
    }

    public async Task<Rental> UpdateRentalDetails(Rental rental) => await _rentalRepository.UpdateRentalDetails(rental);

    public async Task<double> CalculatePrice(Rental rental)
    {
        double price;

        if (rental.StartTime == DateTime.MinValue)
            throw new ArgumentException("StartTime not found");

        if (rental.EndTime == DateTime.MinValue)
            throw new ArgumentException("EndTime not found");

        var bikePrice = await _bikePriceRepository.GetBikePrice(rental.Location.Id, rental.Bike.Id);
        if (bikePrice == null)
            throw new ArgumentException("Bike not found in location");

        var time = (DateTime)rental.EndTime - (DateTime)rental.StartTime;
        if (time < new TimeSpan(0, 0, 0))
            throw new ArgumentException("Endtime is before Starttime");
        else if (time <= new TimeSpan(12, 0, 0))
            price = bikePrice.Prices.HalfDay;
        else if (time <= new TimeSpan(24, 0, 0))
            price = bikePrice.Prices.Day;
        else if (time <= new TimeSpan(48, 0, 0))
            price = bikePrice.Prices.TwoDays;
        else if (time <= new TimeSpan(72, 0, 0))
            price = bikePrice.Prices.TreeDays;
        else
            price = bikePrice.Prices.TreeDays + (bikePrice.Prices.ExtraDay * (Math.Ceiling(time.TotalDays) - 3));

        return price;
    }
}


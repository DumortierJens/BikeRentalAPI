namespace BikeRentalAPI_Testing;

public class RentalLocationServiceUnitTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("000000000000000000000000", "000000000000000000000000")]
    [InlineData("62404058637894569c2dfe8a", null)]
    [InlineData("62404058637894569c2dfe8a", "000000000000000000000000")]
    [InlineData(null, "6240408b637894569c2dfe8c")]
    [InlineData("000000000000000000000000", "6240408b637894569c2dfe8c")]
    [InlineData("62404058637894569c2df888", "6240408b637894569c2df888")]
    public async void Add_BikePrice_Argument_Exeption(string bikeId, string locationId)
    {
        var rentalService = Helper.CreateRentalService();

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = "62404058637894569c2df888", LocationId = "6240408b637894569c2df888" };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var rental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = bikeId },
            Location = new RentalLocation() { Id = locationId }
        };
        await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.StartRental(rental));
    }

    [Fact]
    public async void Add_BikePrice_Argument_Exeption_Bike()
    {
        var rentalService = Helper.CreateRentalService();

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var rental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            // Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.StartRental(rental));
    }

    [Fact]
    public async void Add_BikePrice_Argument_Exeption_Location()
    {
        var rentalService = Helper.CreateRentalService();

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var rental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" }
            // Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.StartRental(rental));
    }
}


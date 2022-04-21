namespace BikeRentalAPI_Testing;

public class RentalServiceUnitTests
{
    [Fact]
    public async void Start_Rental_Argument_Exeption_Location()
    {
        var rentalService = Helper.CreateRentalService();

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var rental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            // Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.StartRental(rental));

        var rental2 = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "000000000000000000000000" }
        };
        await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.StartRental(rental2));
    }

    [Fact]
    public async void Start_Rental_Argument_Exeption_Bike()
    {
        var rentalService = Helper.CreateRentalService();

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var rental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            // Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.StartRental(rental));

        var rental2 = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "000000000000000000000000" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.StartRental(rental2));
    }

    // [Fact]
    // public async void Start_Rental_Argument_Exeption_BikePrice()
    // {
    //     var rentalService = Helper.CreateRentalService();

    //     var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
    //     FakeBikeRepository.AddFakeBike(fakeBike);
    //     var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
    //     FakeLocationRepository.AddFakeLocation(fakeLocation);
    //     // var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
    //     // FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

    //     var rental = new Rental()
    //     {
    //         Name = "Jens Dumortier",
    //         Tel = "0412345678",
    //         Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
    //         Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
    //     };
    //     await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.StartRental(rental));
    // }

    public static readonly object[][] TimeData =
    {
            new object[] { null, null },
            new object[] { new DateTime(2022, 4, 1, 0, 0, 0), null },
            new object[] { null, new DateTime(2022, 4, 2, 0, 0, 0) },
            new object[] { new DateTime(2022, 4, 2, 0, 0, 0), new DateTime(2022, 4, 1, 0, 0, 0) }
        };

    [Theory, MemberData(nameof(TimeData))]
    public async void Calculate_Price_Argument_Exeption_Time(DateTime startTime, DateTime endTime)
    {
        var rentalService = Helper.CreateRentalService();

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice()
        {
            Id = "62406ced9870f98e93c2b5d0",
            BikeId = fakeBike.Id,
            LocationId = fakeLocation.Id,
            Prices = new Prices()
            {
                HalfDay = 7,
                Day = 12,
                TwoDays = 22,
                TreeDays = 30,
                ExtraDay = 9
            }
        };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var rental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" },
            StartTime = startTime,
            EndTime = endTime
        };
        await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.CalculatePrice(rental));
    }

    // [Fact]
    // public async void Calculate_Price_Argument_Exeption_BikePrice()
    // {
    //     var rentalService = Helper.CreateRentalService();

    //     var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
    //     FakeBikeRepository.AddFakeBike(fakeBike);
    //     var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
    //     FakeLocationRepository.AddFakeLocation(fakeLocation);
    //     // var fakeBikePrice = new BikePrice()
    //     // {
    //     //     Id = "62406ced9870f98e93c2b5d0",
    //     //     BikeId = fakeBike.Id,
    //     //     LocationId = fakeLocation.Id,
    //     //     Prices = new Prices()
    //     //     {
    //     //         HalfDay = 7,
    //     //         Day = 12,
    //     //         TwoDays = 22,
    //     //         TreeDays = 30,
    //     //         ExtraDay = 9
    //     //     }
    //     // };
    //     // FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

    //     var rental = new Rental()
    //     {
    //         Name = "Jens Dumortier",
    //         Tel = "0412345678",
    //         Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
    //         Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" },
    //         StartTime = new DateTime(2022, 4, 1, 0, 0, 0),
    //         EndTime = new DateTime(2022, 4, 2, 0, 0, 0)
    //     };
    //     await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.CalculatePrice(rental));
    // }

    // [Theory]
    // [InlineData(10, 7)]
    // [InlineData(20, 12)]
    // [InlineData(40, 22)]
    // [InlineData(60, 30)]
    // [InlineData(80, 39)]
    // [InlineData(100, 48)]
    // public async void Calculate_Price(int hours, double correctPrice)
    // {
    //     var rentalService = Helper.CreateRentalService();

    //     var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
    //     FakeBikeRepository.AddFakeBike(fakeBike);
    //     var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
    //     FakeLocationRepository.AddFakeLocation(fakeLocation);
    //     var fakeBikePrice = new BikePrice()
    //     {
    //         Id = "62406ced9870f98e93c2b5d0",
    //         BikeId = fakeBike.Id,
    //         LocationId = fakeLocation.Id,
    //         Prices = new Prices()
    //         {
    //             HalfDay = 7,
    //             Day = 12,
    //             TwoDays = 22,
    //             TreeDays = 30,
    //             ExtraDay = 9
    //         }
    //     };
    //     FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

    //     var rental = new Rental()
    //     {
    //         Name = "Jens Dumortier",
    //         Tel = "0412345678",
    //         Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
    //         Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" },
    //         StartTime = new DateTime(2022, 4, 1, 0, 0, 0),
    //         EndTime = new DateTime(2022, 4, 1, 0, 0, 0).AddHours(hours)
    //     };

    //     double price = await rentalService.CalculatePrice(rental);
    //     Assert.Equal(correctPrice, price);
    // }

    [Fact]
    public async void Stop_Rental_Argument_Exeption_RentalId()
    {
        var rentalService = Helper.CreateRentalService();
        await Assert.ThrowsAsync<ArgumentException>(async () => await rentalService.StopRental(null));
    }

    [Fact]
    public async void Stop_Rental_Null()
    {
        var rentalService = Helper.CreateRentalService();
        var res = await rentalService.StopRental("6240408b637894569c2dfe8c");
        Assert.Null(res);
    }
}


namespace BikeRentalAPI_Testing;

public class BikePriceIntegrationTests
{

    [Fact]
    public async void Return_BikePrices_Of_Location_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var result = await client.GetAsync("/locations/6240408b637894569c2dfe8c/prices");
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var bikePrices = await result.Content.ReadFromJsonAsync<List<BikePrice>>();
        Assert.NotNull(bikePrices);
        Assert.IsType<List<BikePrice>>(bikePrices);
    }

    [Fact]
    public async void Return_BikePrice_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBikePrice = new BikePrice()
        {
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "62339d87ac01f7ff39b2d06b",
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

        var result = await client.GetAsync($"/locations/{fakeBikePrice.LocationId}/bikes/{fakeBikePrice.BikeId}/prices");
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var bikePrice = await result.Content.ReadFromJsonAsync<BikePrice>();
        Assert.NotNull(bikePrice);
        Assert.IsType<BikePrice>(bikePrice);
    }

    [Fact]
    public async void Return_BikePrice_NOT_FOUND()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBikePrice = new BikePrice()
        {
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "62339d87ac01f7ff39b2d06b",
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

        var result = await client.GetAsync($"/locations/000000000000000000000000/bikes/000000000000000000000000/prices");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Add_BikePrice_CREATED()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);

        var newBikePrice = new BikePrice()
        {
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "62339d87ac01f7ff39b2d06b",
            Prices = new Prices()
            {
                HalfDay = 7,
                Day = 12,
                TwoDays = 22,
                TreeDays = 30,
                ExtraDay = 9
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var bikePrice = await result.Content.ReadFromJsonAsync<BikePrice>();
        Assert.NotNull(bikePrice);
        Assert.IsType<BikePrice>(bikePrice);
    }

    [Fact]
    public async void Add_BikePrice_BAD_LOCATIONID()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var newBikePrice = new BikePrice()
        {
            LocationId = "000000000000000000000000",
            BikeId = "62339d87ac01f7ff39b2d06b",
            Prices = new Prices()
            {
                HalfDay = 7,
                Day = 12,
                TwoDays = 22,
                TreeDays = 30,
                ExtraDay = 9
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Add_BikePrice_BAD_EMPTY_LOCATIONID()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var newBikePrice = new BikePrice()
        {
            BikeId = "62339d87ac01f7ff39b2d06b",
            Prices = new Prices()
            {
                HalfDay = 7,
                Day = 12,
                TwoDays = 22,
                TreeDays = 30,
                ExtraDay = 9
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Add_BikePrice_BAD_BIKEID()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var newBikePrice = new BikePrice()
        {
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "000000000000000000000000",
            Prices = new Prices()
            {
                HalfDay = 7,
                Day = 12,
                TwoDays = 22,
                TreeDays = 30,
                ExtraDay = 9
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Add_BikePrice_BAD_EMPTY_BIKEID()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var newBikePrice = new BikePrice()
        {
            LocationId = "6240408b637894569c2dfe8c",
            Prices = new Prices()
            {
                HalfDay = 7,
                Day = 12,
                TwoDays = 22,
                TreeDays = 30,
                ExtraDay = 9
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Add_BikePrice_BAD_EMPTY_PRICES()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var newBikePrice = new BikePrice()
        {
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "62339d87ac01f7ff39b2d06b"
        };
        var content = new StringContent(JsonConvert.SerializeObject(newBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Add_BikePrice_BAD_EMPTY_PRICES_ITEMS()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var newBikePrice = new BikePrice()
        {
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "62339d87ac01f7ff39b2d06b",
            Prices = new Prices() { }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(10, errors.Count); // 5x empty / 5x greater than 0
    }

    [Fact]
    public async void Update_BikePrice_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBikePrice = new BikePrice()
        {
            Id = "6240408b637894569c2d1234",
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "62339d87ac01f7ff39b2d06b",
            Prices = new Prices()
            {
                HalfDay = 10,
                Day = 18,
                TwoDays = 26,
                TreeDays = 32,
                ExtraDay = 5
            }
        };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var updatedBikePrice = new BikePrice()
        {
            Id = "6240408b637894569c2d1234",
            Prices = new Prices()
            {
                HalfDay = 5,
                Day = 9,
                TwoDays = 13,
                TreeDays = 16,
                ExtraDay = 2.5
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(updatedBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var bikePrice = await result.Content.ReadFromJsonAsync<BikePrice>();
        Assert.NotNull(bikePrice);
        Assert.IsType<BikePrice>(bikePrice);
        Assert.Equal(updatedBikePrice.Prices.Day, bikePrice.Prices.Day);
    }

    [Fact]
    public async void Update_BikePrice_NOT_FOUND()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBikePrice = new BikePrice()
        {
            Id = "6240408b637894569c2d1234",
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "62339d87ac01f7ff39b2d06b",
            Prices = new Prices()
            {
                HalfDay = 10,
                Day = 18,
                TwoDays = 26,
                TreeDays = 32,
                ExtraDay = 5
            }
        };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var updatedBikePrice = new BikePrice()
        {
            Id = "000000000000000000000000",
            Prices = new Prices()
            {
                HalfDay = 5,
                Day = 9,
                TwoDays = 13,
                TreeDays = 16,
                ExtraDay = 2.5
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(updatedBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Update_BikePrice_BAD_EMPTY_PRICES()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBikePrice = new BikePrice()
        {
            Id = "6240408b637894569c2d1234",
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "62339d87ac01f7ff39b2d06b",
            Prices = new Prices()
            {
                HalfDay = 10,
                Day = 18,
                TwoDays = 26,
                TreeDays = 32,
                ExtraDay = 5
            }
        };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var updateBikePrice = new BikePrice()
        {
            Id = fakeBikePrice.Id
        };
        var content = new StringContent(JsonConvert.SerializeObject(updateBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Update_BikePrice_BAD_EMPTY_PRICES_ITEMS()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeBikePrice = new BikePrice()
        {
            Id = "6240408b637894569c2d1234",
            LocationId = "6240408b637894569c2dfe8c",
            BikeId = "62339d87ac01f7ff39b2d06b",
            Prices = new Prices()
            {
                HalfDay = 10,
                Day = 18,
                TwoDays = 26,
                TreeDays = 32,
                ExtraDay = 5
            }
        };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var newBikePrice = new BikePrice()
        {
            Id = fakeBikePrice.Id,
            Prices = new Prices() { }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newBikePrice), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/prices", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(10, errors.Count); // 5x empty / 5x greater than 0
    }

}


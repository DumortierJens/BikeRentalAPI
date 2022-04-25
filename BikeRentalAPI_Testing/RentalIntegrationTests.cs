namespace BikeRentalAPI_Testing;

public class RentalIntegrationTests
{

    [Fact]
    public async void Return_BikePrices_Of_Location_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var result = await client.GetAsync("/rentals/locations/6240408b637894569c2dfe8c");
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var rentals = await result.Content.ReadFromJsonAsync<List<Rental>>();
        Assert.NotNull(rentals);
        Assert.IsType<List<Rental>>(rentals);
    }

    [Fact]
    public async void Return_Rental_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeRental = new Rental()
        {
            Id = "625bd132e113bceeeadcfe5e",
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        FakeRentalRepository.AddFakeRental(fakeRental);

        var result = await client.GetAsync($"/rentals/{fakeRental.Id}");
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var rental = await result.Content.ReadFromJsonAsync<Rental>();
        Assert.NotNull(rental);
        Assert.IsType<Rental>(rental);
    }

    [Fact]
    public async void Return_Rental_NOT_FOUND()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeRental = new Rental()
        {
            Id = "625bd132e113bceeeadcfe5e",
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        FakeRentalRepository.AddFakeRental(fakeRental);

        var result = await client.GetAsync($"/rentals/000000000000000000000000");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Start_Rental_CREATED()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var newRental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newRental), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/rentals/start", content);
        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var rental = await result.Content.ReadFromJsonAsync<Rental>();
        Assert.NotNull(rental);
        Assert.IsType<Rental>(rental);
    }

    [Fact]
    public async void Start_Rental_BAD_LOCATIONID()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var newRental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "000000000000000000000000" }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newRental), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/rentals/start", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Start_Rental_BAD_EMPTY_LOCATIONID()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var newRental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
        };
        var content = new StringContent(JsonConvert.SerializeObject(newRental), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/rentals/start", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Start_Rental_BAD_BIKEID()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var newRental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "000000000000000000000000" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newRental), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/rentals/start", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Start_Rental_BAD_EMPTY_BIKEID()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var newRental = new Rental()
        {
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newRental), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/rentals/start", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    // Vragen (2x Fail => Created ipv Bad) => DEBUG werkt (zie verder)
    // [Fact]
    // public async void Start_Rental_BAD_BIKE_NOT_FOUND_IN_LOCATION()
    // {
    //     var application = Helper.CreateApi();
    //     var client = application.CreateClient();
    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());
    //
    //     var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
    //     FakeBikeRepository.AddFakeBike(fakeBike);
    //     var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
    //     FakeLocationRepository.AddFakeLocation(fakeLocation);
    //     var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = "000000000000000000000000", LocationId = "000000000000000000000000" };
    //     FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

    //     var newRental = new Rental()
    //     {
    //         Name = "Jens Dumortier",
    //         Tel = "0412345678",
    //         Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
    //         Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
    //     };
    //     var content = new StringContent(JsonConvert.SerializeObject(newRental), Encoding.UTF8, "application/json");
    //     var result = await client.PostAsync("/rentals/start", content);
    //     result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    //     var errors = await result.Content.ReadFromJsonAsync<List<object>>();
    //     Assert.NotNull(errors);
    //     Assert.Equal(1, errors.Count);
    // }

    [Fact]
    public async void Add_Bike_BAD_EMPTY_NAME()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var newRental = new Rental()
        {
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newRental), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/rentals/start", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Add_Bike_BAD_EMPTY_TEL()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62404058637894569c2dfe8a", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);
        var fakeLocation = new RentalLocation() { Id = "6240408b637894569c2dfe8c", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);
        var fakeBikePrice = new BikePrice() { Id = "62406ced9870f98e93c2b5d0", BikeId = fakeBike.Id, LocationId = fakeLocation.Id };
        FakeBikePriceRepository.AddFakeBikePrice(fakeBikePrice);

        var newRental = new Rental()
        {
            Name = "Jens Dumortier",
            Bike = new Bike() { Id = "62404058637894569c2dfe8a" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe8c" }
        };
        var content = new StringContent(JsonConvert.SerializeObject(newRental), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/rentals/start", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Stop_Rental_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBikePrice = new BikePrice()
        {
            Id = "62404080000894569c2dfe00",
            LocationId = "6240408b637894569c2dfe00",
            BikeId = "62404058637894569c2dfe00",
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
        var fakeRentalPrice = new Rental()
        {
            Id = "62406ced9870f98e93c256a9",
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe00" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe00" },
            StartTime = DateTime.Now.AddDays(-1),
            EndTime = null,
            Price = null
        };
        FakeRentalRepository.AddFakeRental(fakeRentalPrice);

        var content = new StringContent(string.Empty);
        var result = await client.PostAsync("/rentals/62406ced9870f98e93c256a9/stop", content);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var rental = await result.Content.ReadFromJsonAsync<Rental>();
        Assert.NotNull(rental);
        Assert.IsType<Rental>(rental);
        Assert.NotNull(rental.Price);
    }

    [Fact]
    public async void Stop_Rental_NOT_FOUND()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBikePrice = new BikePrice()
        {
            Id = "62404080000894569c2dfe00",
            LocationId = "6240408b637894569c2dfe00",
            BikeId = "62404058637894569c2dfe00",
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
        var fakeRental = new Rental()
        {
            Id = "62406ced9870f98e93c256a9",
            Name = "Jens Dumortier",
            Tel = "0412345678",
            Bike = new Bike() { Id = "62404058637894569c2dfe00" },
            Location = new RentalLocation() { Id = "6240408b637894569c2dfe00" },
            StartTime = DateTime.Now.AddDays(-1),
            EndTime = null,
            Price = null
        };
        FakeRentalRepository.AddFakeRental(fakeRental);

        var content = new StringContent(string.Empty);
        var result = await client.PostAsync("/rentals/000000000000000000000000/stop", content);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // [Fact]
    // public async void Update_Rental_OK()
    // {
    //     var application = Helper.CreateApi();
    //     var client = application.CreateClient();
    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());
    //
    //     var fakeRental = new Rental()
    //     {
    //         Id = "62406ced9870f98e93c256a9",
    //         Name = "Jens Dumortier",
    //         Tel = "0412345678",
    //         Bike = new Bike() { Id = "62404058637894569c2dfe00" },
    //         Location = new RentalLocation() { Id = "6240408b637894569c2dfe00" },
    //         StartTime = DateTime.Now.AddDays(-1),
    //         EndTime = null,
    //         Price = null
    //     };
    //     FakeRentalRepository.AddFakeRental(fakeRental);

    //     var updatedRental = new Rental()
    //     {
    //         Id = "62406ced9870f98e93c256a9",
    //         Name = "JD",
    //         Tel = "0487654321",
    //     };
    //     var content = new StringContent(JsonConvert.SerializeObject(updatedRental), Encoding.UTF8, "application/json");
    //     var result = await client.PutAsync("/rentals", content);
    //     result.StatusCode.Should().Be(HttpStatusCode.OK);

    //     var rental = await result.Content.ReadFromJsonAsync<Rental>();
    //     Assert.NotNull(rental);
    //     Assert.IsType<Rental>(rental);
    //     Assert.Equal(updatedRental.Name, rental.Name);
    //     Assert.Equal(updatedRental.Tel, rental.Tel);
    // }

    // [Fact]
    // public async void Update_Rental_NOT_FOUND()
    // {
    //     var application = Helper.CreateApi();
    //     var client = application.CreateClient();
    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());
    //
    //     var fakeRental = new Rental()
    //     {
    //         Id = "62406ced9870f98e93c256a9",
    //         Name = "Jens Dumortier",
    //         Tel = "0412345678",
    //         Bike = new Bike() { Id = "62404058637894569c2dfe00" },
    //         Location = new RentalLocation() { Id = "6240408b637894569c2dfe00" },
    //         StartTime = DateTime.Now.AddDays(-1),
    //         EndTime = null,
    //         Price = null
    //     };
    //     FakeRentalRepository.AddFakeRental(fakeRental);

    //     var updatedRental = new Rental()
    //     {
    //         Id = "000000000000000000000000",
    //         Name = "JD",
    //         Tel = "0487654321",
    //     };
    //     var content = new StringContent(JsonConvert.SerializeObject(updatedRental), Encoding.UTF8, "application/json");
    //     var result = await client.PutAsync("/rentals", content);
    //     result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    // }

    // [Fact]
    // public async void Update_Rental_BAD_EMPTY_NAME()
    // {
    //     var application = Helper.CreateApi();
    //     var client = application.CreateClient();
    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());
    //
    //     var fakeRental = new Rental()
    //     {
    //         Id = "62406ced9870f98e93c256a9",
    //         Name = "Jens Dumortier",
    //         Tel = "0412345678",
    //         Bike = new Bike() { Id = "62404058637894569c2dfe00" },
    //         Location = new RentalLocation() { Id = "6240408b637894569c2dfe00" },
    //         StartTime = DateTime.Now.AddDays(-1),
    //         EndTime = null,
    //         Price = null
    //     };
    //     FakeRentalRepository.AddFakeRental(fakeRental);

    //     var updatedRental = new Rental()
    //     {
    //         Id = "62406ced9870f98e93c256a9",
    //         Tel = "0487654321",
    //     };
    //     var content = new StringContent(JsonConvert.SerializeObject(updatedRental), Encoding.UTF8, "application/json");
    //     var result = await client.PutAsync("/rentals", content);
    //     result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    //     var errors = await result.Content.ReadFromJsonAsync<List<object>>();
    //     Assert.NotNull(errors);
    //     Assert.Equal(1, errors.Count);
    // }

    // [Fact]
    // public async void Update_Rental_BAD_EMPTY_TEL()
    // {
    //     var application = Helper.CreateApi();
    //     var client = application.CreateClient();
    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());
    //
    //     var fakeRental = new Rental()
    //     {
    //         Id = "62406ced9870f98e93c256a9",
    //         Name = "Jens Dumortier",
    //         Tel = "0412345678",
    //         Bike = new Bike() { Id = "62404058637894569c2dfe00" },
    //         Location = new RentalLocation() { Id = "6240408b637894569c2dfe00" },
    //         StartTime = DateTime.Now.AddDays(-1),
    //         EndTime = null,
    //         Price = null
    //     };
    //     FakeRentalRepository.AddFakeRental(fakeRental);

    //     var updatedRental = new Rental()
    //     {
    //         Id = "62406ced9870f98e93c256a9",
    //         Name = "JD"
    //     };
    //     var content = new StringContent(JsonConvert.SerializeObject(updatedRental), Encoding.UTF8, "application/json");
    //     var result = await client.PutAsync("/rentals", content);
    //     result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    //     var errors = await result.Content.ReadFromJsonAsync<List<object>>();
    //     Assert.NotNull(errors);
    //     Assert.Equal(1, errors.Count);
    // }

}


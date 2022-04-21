namespace BikeRentalAPI_Testing;

public class BikeIntegrationTests
{

    [Fact]
    public async void Return_Bikes_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var result = await client.GetAsync("/bikes");
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var bikes = await result.Content.ReadFromJsonAsync<List<Bike>>();
        Assert.NotNull(bikes);
        Assert.IsType<List<Bike>>(bikes);
    }

    [Fact]
    public async void Return_Bike_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);

        var result = await client.GetAsync($"/bikes/{fakeBike.Id}");
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var bike = await result.Content.ReadFromJsonAsync<Bike>();
        Assert.NotNull(bike);
        Assert.IsType<Bike>(bike);
    }

    [Fact]
    public async void Return_Bike_NOT_FOUND()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);

        var result = await client.GetAsync($"/bikes/000000000000000000000000");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Add_Bike_CREATED()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var newBike = new Bike() { Name = "City bike" };
        var content = new StringContent(JsonConvert.SerializeObject(newBike), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/bikes", content);
        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var bike = await result.Content.ReadFromJsonAsync<Bike>();
        Assert.NotNull(bike);
        Assert.IsType<Bike>(bike);
    }

    [Fact]
    public async void Add_Bike_BAD_EMPTY_NAME()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var newBike = new Bike() { };
        var content = new StringContent(JsonConvert.SerializeObject(newBike), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/bikes", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Update_Bike_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);

        var updatedBike = new Bike() { Id = fakeBike.Id, Name = "Electric bike" };
        var content = new StringContent(JsonConvert.SerializeObject(updatedBike), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/bikes", content);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var bike = await result.Content.ReadFromJsonAsync<Bike>();
        Assert.NotNull(bike);
        Assert.IsType<Bike>(bike);
        Assert.Equal(updatedBike.Name, bike.Name);
    }

    [Fact]
    public async void Update_Bike_NOT_FOUND()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);

        var updatedBike = new Bike() { Id = "000000000000000000000000", Name = "Electric bike" };
        var content = new StringContent(JsonConvert.SerializeObject(updatedBike), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/bikes", content);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Update_Bike_BAD_EMPTY_NAME()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GenerateBearerToken());

        var fakeBike = new Bike() { Id = "62339d87ac01f7ff39b2d06b", Name = "City Bike" };
        FakeBikeRepository.AddFakeBike(fakeBike);

        var updatedBike = new Bike() { Id = fakeBike.Id };
        var content = new StringContent(JsonConvert.SerializeObject(updatedBike), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/bikes", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

}


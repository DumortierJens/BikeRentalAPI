using System.Net.Http;
using System.Text;

namespace BikeRentalAPI_Testing;

public class LocationIntegrationTests
{
    [Fact]
    public async void Return_Locations_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var result = await client.GetAsync("/locations");
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var locations = await result.Content.ReadFromJsonAsync<List<RentalLocation>>();
        Assert.NotNull(locations);
        Assert.IsType<List<RentalLocation>>(locations);
    }

    [Fact]
    public async void Return_Location_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeLocation = new RentalLocation() { Id = "62339d87ac01f7ff39b2d06b", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var result = await client.GetAsync($"/locations/{fakeLocation.Id}");
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var location = await result.Content.ReadFromJsonAsync<RentalLocation>();
        Assert.NotNull(location);
        Assert.IsType<RentalLocation>(location);
    }

    [Fact]
    public async void Return_Location_NOT_FOUND()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeLocation = new RentalLocation() { Id = "62339d87ac01f7ff39b2d06b", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var result = await client.GetAsync($"/locations/000000000000000000000000");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Add_Location_CREATED()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var newLocation = new RentalLocation() { Name = "Roeselare Bikes", City = "Roeselare" };
        var content = new StringContent(JsonConvert.SerializeObject(newLocation), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/locations", content);
        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = await result.Content.ReadFromJsonAsync<RentalLocation>();
        Assert.NotNull(location);
        Assert.IsType<RentalLocation>(location);
    }

    [Fact]
    public async void Add_Location_BAD_EMPTY_NAME()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var newLocation = new RentalLocation() { City = "Roeselare" };
        var content = new StringContent(JsonConvert.SerializeObject(newLocation), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/locations", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Add_Location_BAD_EMPTY_CITY()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var newLocation = new RentalLocation() { Name = "Roeselare Bikes" };
        var content = new StringContent(JsonConvert.SerializeObject(newLocation), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/locations", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public async void Update_Location_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeLocation = new RentalLocation() { Id = "62339d87ac01f7ff39b2d06b", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var updatedLocation = new RentalLocation() { Id = fakeLocation.Id, Name = "Roeseloare Bikes" };
        var content = new StringContent(JsonConvert.SerializeObject(updatedLocation), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/locations", content);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var location = await result.Content.ReadFromJsonAsync<RentalLocation>();
        Assert.NotNull(location);
        Assert.IsType<RentalLocation>(location);
        Assert.Equal(updatedLocation.Name, location.Name);
    }

    [Fact]
    public async void Update_Location_NOT_FOUND()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeLocation = new RentalLocation() { Id = "62339d87ac01f7ff39b2d06b", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var updatedLocation = new RentalLocation() { Id = "000000000000000000000000", Name = "Roeseloare Bikes" };
        var content = new StringContent(JsonConvert.SerializeObject(updatedLocation), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/locations", content);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Update_Location_BAD_EMPTY_NAME()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var fakeLocation = new RentalLocation() { Id = "62339d87ac01f7ff39b2d06b", Name = "Roeselare Bikes", City = "Roeselare" };
        FakeLocationRepository.AddFakeLocation(fakeLocation);

        var updatedLocation = new RentalLocation() { Id = fakeLocation.Id };
        var content = new StringContent(JsonConvert.SerializeObject(updatedLocation), Encoding.UTF8, "application/json");
        var result = await client.PutAsync("/locations", content);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await result.Content.ReadFromJsonAsync<List<object>>();
        Assert.NotNull(errors);
        Assert.Equal(1, errors.Count);
    }
}


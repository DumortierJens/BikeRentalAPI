namespace BikeRentalAPI_Testing;

public class IntegrationTests
{
    public class BikeTypeModel
    {
        [Fact]
        public async void Return_BikeTypes()
        {
            var application = Helper.CreateApi();
            var client = application.CreateClient();

            var result = await client.GetAsync("/biketypes");
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var items = await result.Content.ReadFromJsonAsync<List<BikeType>>();
            Assert.NotNull(items);
            Assert.IsType<List<BikeType>>(items);
        }

        [Fact]
        public async void Return_BikeType()
        {
            var application = Helper.CreateApi();
            var client = application.CreateClient();

            var fakeBikeType = new BikeType() { Id = "62339d87ac01f7ff39b2d06b" };
            FakeBikeTypeRepository.AddFakeBikeType(fakeBikeType);

            var result = await client.GetAsync($"/biketypes/{fakeBikeType.Id}");
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var item = await result.Content.ReadFromJsonAsync<BikeType>();
            Assert.NotNull(item);
            Assert.IsType<BikeType>(item);
        }
    }
}


using System.Net.Http;
using System.Text;

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

            var fakeBikeType = new BikeType() { Id = "62339d87ac01f7ff39b2d06b" };
            FakeBikeTypeRepository.AddFakeBikeType(fakeBikeType);

            var result = await client.GetAsync("/biketypes");
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var items = await result.Content.ReadFromJsonAsync<List<BikeType>>();
            Assert.NotNull(items);
            Assert.IsType<List<BikeType>>(items);
            Assert.True(items.Count > 0);
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

        [Fact]
        public async void Return_BikeType_NotFound()
        {
            var application = Helper.CreateApi();
            var client = application.CreateClient();

            var fakeBikeType = new BikeType() { Id = "62339d87ac01f7ff39b2d06b" };
            FakeBikeTypeRepository.AddFakeBikeType(fakeBikeType);

            var result = await client.GetAsync($"/biketypes/00039d87ac01f7ff39b2d000");
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        // [Fact]
        // public async void Add_BikeType_Created()
        // {
        //     var application = Helper.CreateApi();
        //     var client = application.CreateClient();

        //     var payload = new BikeType()
        //     {
        //         Id = "1",
        //         Name = "Electric bike",
        //         Prices = new PriceList()
        //         {
        //             HalfDay = 18,
        //             Day = 23,
        //             Days2 = 35,
        //             Days3 = 41,
        //             ExtraDay = 6
        //         }
        //     };
        //     var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        //     var result = await client.PostAsync("/biketypes", content);
        //     result.StatusCode.Should().Be(HttpStatusCode.Created);

        //     var item = await result.Content.ReadFromJsonAsync<BikeType>();
        //     Assert.NotNull(item);
        //     Assert.IsType<BikeType>(item);
        //     Assert.Equal(payload, item);
        // }
    }
}


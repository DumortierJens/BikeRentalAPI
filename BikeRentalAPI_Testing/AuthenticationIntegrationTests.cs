namespace BikeRentalAPI_Testing;

public class AuthenticationIntegrationTests
{
    [Fact]
    public async void Authenticate_OK()
    {
        var application = Helper.CreateApi();
        var client = application.CreateClient();

        var login = new AuthenticationRequestBody("Admin", "Admin");
        var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/auth", content);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var token = await result.Content.ReadFromJsonAsync<string>();
        Assert.NotEmpty(token);
        Assert.IsType<string>(token);
    }
}


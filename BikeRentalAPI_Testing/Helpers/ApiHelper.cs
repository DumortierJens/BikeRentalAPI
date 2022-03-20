namespace BikeRentalAPI_Testing.Helpers;

public class Helper
{
    public static WebApplicationFactory<Program> CreateApi()
    {
        var application = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {

            });
        });

        return application;
    }
}


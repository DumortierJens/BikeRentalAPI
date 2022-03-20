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
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IBikeTypeRepository));
                if (descriptor != null) services.Remove(descriptor);
                var fakeBikeTypeRepository = new ServiceDescriptor(typeof(IBikeTypeRepository), typeof(FakeBikeTypeRepository), ServiceLifetime.Transient);
                services.Add(fakeBikeTypeRepository);
            });
        });

        return application;
    }
}


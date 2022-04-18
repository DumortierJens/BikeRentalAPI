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
                var descriptorBikeRepository = services.SingleOrDefault(d => d.ServiceType == typeof(IBikeRepository));
                if (descriptorBikeRepository != null) services.Remove(descriptorBikeRepository);
                var fakeBikeRepository = new ServiceDescriptor(typeof(IBikeRepository), typeof(FakeBikeRepository), ServiceLifetime.Transient);
                services.Add(fakeBikeRepository);

                var descriptorLocationRepository = services.SingleOrDefault(d => d.ServiceType == typeof(ILocationRepository));
                if (descriptorLocationRepository != null) services.Remove(descriptorLocationRepository);
                var fakeLocationRepository = new ServiceDescriptor(typeof(ILocationRepository), typeof(FakeLocationRepository), ServiceLifetime.Transient);
                services.Add(fakeLocationRepository);

                var descriptorBikePriceRepository = services.SingleOrDefault(d => d.ServiceType == typeof(IBikePriceRepository));
                if (descriptorBikePriceRepository != null) services.Remove(descriptorBikePriceRepository);
                var fakeBikePriceRepository = new ServiceDescriptor(typeof(IBikePriceRepository), typeof(FakeBikePriceRepository), ServiceLifetime.Transient);
                services.Add(fakeBikePriceRepository);
            });
        });

        return application;
    }
}


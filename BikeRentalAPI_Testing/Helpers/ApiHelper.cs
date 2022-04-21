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
                services.Remove(descriptorBikeRepository);
                var fakeBikeRepository = new ServiceDescriptor(typeof(IBikeRepository), typeof(FakeBikeRepository), ServiceLifetime.Transient);
                services.Add(fakeBikeRepository);

                var descriptorLocationRepository = services.SingleOrDefault(d => d.ServiceType == typeof(ILocationRepository));
                services.Remove(descriptorLocationRepository);
                var fakeLocationRepository = new ServiceDescriptor(typeof(ILocationRepository), typeof(FakeLocationRepository), ServiceLifetime.Transient);
                services.Add(fakeLocationRepository);

                var descriptorBikePriceRepository = services.SingleOrDefault(d => d.ServiceType == typeof(IBikePriceRepository));
                services.Remove(descriptorBikePriceRepository);
                var fakeBikePriceRepository = new ServiceDescriptor(typeof(IBikePriceRepository), typeof(FakeBikePriceRepository), ServiceLifetime.Transient);
                services.Add(fakeBikePriceRepository);

                var descriptorRentalRepository = services.SingleOrDefault(d => d.ServiceType == typeof(IRentalRepository));
                services.Remove(descriptorRentalRepository);
                var fakeRentalRepository = new ServiceDescriptor(typeof(IRentalRepository), typeof(FakeRentalRepository), ServiceLifetime.Transient);
                services.Add(fakeRentalRepository);
            });
        });

        return application;
    }

    public static IRentalService CreateRentalService() => CreateApi().Services.GetService<IRentalService>();
}


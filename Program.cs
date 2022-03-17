var builder = WebApplication.CreateBuilder(args);

var mongoSettings = builder.Configuration.GetSection("MongoConnection");
builder.Services.Configure<DatabaseSettings>(mongoSettings);
builder.Services.AddTransient<IMongoContext, MongoContext>();

builder.Services.AddTransient<IBikeTypeRepository, BikeTypeRepository>();

builder.Services.AddTransient<IRentalService, RentalService>();

var app = builder.Build();
app.MapGet("/", () => "API is working!");


#region Bike Types

app.MapGet("/biketypes", async (IRentalService rentalService) =>
{
    return Results.Ok(await rentalService.GetBikeTypesAsync());
});

#endregion


app.Run("http://localhost:3000");

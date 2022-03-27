var builder = WebApplication.CreateBuilder(args);

var mongoSettings = builder.Configuration.GetSection("MongoConnection");
builder.Services.Configure<DatabaseSettings>(mongoSettings);
builder.Services.AddTransient<IMongoContext, MongoContext>();

builder.Services.AddTransient<IBikeRepository, BikeRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();
builder.Services.AddTransient<IBikePriceRepository, BikePriceRepository>();

builder.Services.AddTransient<IRentalLocationService, RentalLocationService>();
builder.Services.AddTransient<IRentalService, RentalService>();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BikeValidation>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LocationValidation>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BikePriceValidation>());

var app = builder.Build();
app.MapGet("/", () => "API is working!");


#region Bikes

app.MapGet("/bikes", async (IRentalLocationService rentalLocationService) =>
{
    return Results.Ok(await rentalLocationService.GetBikes());
});

app.MapGet("/bikes/{id}", async (IRentalLocationService rentalLocationService, string id) =>
{
    var bike = await rentalLocationService.GetBike(id);

    if (bike == null)
        return Results.NotFound();

    return Results.Ok(bike);
});

app.MapPost("/bikes", async (BikeValidation validator, IRentalLocationService rentalLocationService, Bike bike) =>
{
    var validationResult = validator.Validate(bike);
    if (validationResult.IsValid)
    {
        bike = await rentalLocationService.AddBike(bike);
        return Results.Created($"/biketype/{bike.Id}", bike);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPut("/bikes", async (BikeValidation validator, IRentalLocationService rentalLocationService, Bike bike) =>
{
    var validationResult = validator.Validate(bike);
    if (validationResult.IsValid && bike.Id != null)
    {
        bike = await rentalLocationService.UpdateBike(bike);

        if (bike == null)
            return Results.NotFound();

        return Results.Ok(bike);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

#endregion

#region Locations

app.MapGet("/locations", async (IRentalLocationService rentalLocationService) =>
{
    return Results.Ok(await rentalLocationService.GetLocations());
});

app.MapGet("/locations/{id}", async (IRentalLocationService rentalLocationService, string id) =>
{
    var location = await rentalLocationService.GetLocation(id);

    if (location == null)
        return Results.NotFound();

    return Results.Ok(location);
});

app.MapPost("/locations", async (LocationValidation validator, IRentalLocationService rentalLocationService, Location location) =>
{
    var validationResult = validator.Validate(location);
    if (validationResult.IsValid)
    {
        location = await rentalLocationService.AddLocation(location);
        return Results.Created($"/biketype/{location.Id}", location);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPut("/locations", async (LocationValidation validator, IRentalLocationService rentalLocationService, Location location) =>
{
    var validationResult = validator.Validate(location);
    if (validationResult.IsValid && location.Id != null)
    {
        location = await rentalLocationService.UpdateLocation(location);

        if (location == null)
            return Results.NotFound();

        return Results.Ok(location);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

#endregion

#region Bike Prices

app.MapGet("/locations/{locationId}/prices", async (IRentalLocationService rentalLocationService, string locationId) =>
{
    return Results.Ok(await rentalLocationService.GetBikePricesByLocation(locationId));
});

app.MapGet("/locations/{locationId}/bikes/{bikeId}/prices", async (IRentalLocationService rentalLocationService, string locationId, string bikeId) =>
{
    var bikePrice = await rentalLocationService.GetBikePrice(locationId, bikeId);

    if (bikePrice == null)
        return Results.NotFound();

    return Results.Ok(bikePrice);
});

app.MapPost("/prices", async (BikePriceValidation validator, IRentalLocationService rentalLocationService, BikePrice bikePrice) =>
{
    var validationResult = validator.Validate(bikePrice);
    if (validationResult.IsValid)
    {
        try
        {
            bikePrice = await rentalLocationService.AddBikePrice(bikePrice);
            return Results.Created($"/locations/{bikePrice.LocationId}/bikes/{bikePrice.BikeId}/prices", bikePrice);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex);
        }
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPut("/prices", async (BikePriceValidation validator, IRentalLocationService rentalLocationService, BikePrice bikePrice) =>
{
    var validationResult = validator.Validate(bikePrice);
    if (validationResult.IsValid)
    {
        bikePrice = await rentalLocationService.UpdateBikePrice(bikePrice);
        return Results.Ok(bikePrice);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

#endregion


app.Run("http://localhost:3000");
// app.Run();

// For Xunit testing
public partial class Program { }
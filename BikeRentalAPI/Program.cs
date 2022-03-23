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

var app = builder.Build();
app.MapGet("/", () => "API is working!");


#region Bikes

app.MapGet("/bikes", async (IRentalService rentalService) =>
{
    return Results.Ok(await rentalService.GetBikes());
});

app.MapGet("/bikes/{id}", async (IRentalService rentalService, string id) =>
{
    var bike = await rentalService.GetBike(id);

    if (bike == null)
        return Results.NotFound();

    return Results.Ok(bike);
});

app.MapPost("/bikes", async (BikeValidation validator, IRentalService rentalService, Bike bike) =>
{
    var validationResult = validator.Validate(bike);
    if (validationResult.IsValid)
    {
        bike = await rentalService.AddBike(bike);
        return Results.Created($"/biketype/{bike.Id}", bike);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPut("/bikes", async (BikeValidation validator, IRentalService rentalService, Bike bike) =>
{
    var validationResult = validator.Validate(bike);
    if (validationResult.IsValid && bike.Id != null)
    {
        bike = await rentalService.UpdateBike(bike);

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

app.MapGet("/locations", async (IRentalService rentalService) =>
{
    return Results.Ok(await rentalService.GetLocations());
});

app.MapGet("/locations/{id}", async (IRentalService rentalService, string id) =>
{
    var location = await rentalService.GetLocation(id);

    if (location == null)
        return Results.NotFound();

    return Results.Ok(location);
});

app.MapPost("/locations", async (LocationValidation validator, IRentalService rentalService, Location location) =>
{
    var validationResult = validator.Validate(location);
    if (validationResult.IsValid)
    {
        location = await rentalService.AddLocation(location);
        return Results.Created($"/biketype/{location.Id}", location);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPut("/locations", async (LocationValidation validator, IRentalService rentalService, Location location) =>
{
    var validationResult = validator.Validate(location);
    if (validationResult.IsValid && location.Id != null)
    {
        location = await rentalService.UpdateLocation(location);

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


app.Run("http://localhost:3000");
// app.Run();

// For Xunit testing
public partial class Program { }
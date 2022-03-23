var builder = WebApplication.CreateBuilder(args);

var mongoSettings = builder.Configuration.GetSection("MongoConnection");
builder.Services.Configure<DatabaseSettings>(mongoSettings);
builder.Services.AddTransient<IMongoContext, MongoContext>();

builder.Services.AddTransient<IBikeTypeRepository, BikeTypeRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();

builder.Services.AddTransient<IRentalService, RentalService>();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BikeTypeValidation>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LocationValidation>());

var app = builder.Build();
app.MapGet("/", () => "API is working!");


#region Bike Types

app.MapGet("/biketypes", async (IRentalService rentalService) =>
{
    return Results.Ok(await rentalService.GetBikeTypesAsync());
});

app.MapGet("/biketypes/{id}", async (IRentalService rentalService, string id) =>
{
    var bikeType = await rentalService.GetBikeTypeAsync(id);

    if (bikeType == null)
        return Results.NotFound();

    return Results.Ok(bikeType);
});

app.MapPost("/biketypes", async (BikeTypeValidation validator, IRentalService rentalService, BikeType bikeType) =>
{
    var validationResult = validator.Validate(bikeType);
    if (validationResult.IsValid)
    {
        bikeType = await rentalService.AddBikeTypeAsync(bikeType);
        return Results.Created($"/biketype/{bikeType.Id}", bikeType);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPut("/biketypes", async (BikeTypeValidation validator, IRentalService rentalService, BikeType bikeType) =>
{
    var validationResult = validator.Validate(bikeType);
    if (validationResult.IsValid && bikeType.Id != null)
    {
        bikeType = await rentalService.UpdateBikeTypeAsync(bikeType);

        if (bikeType == null)
            return Results.NotFound();

        return Results.Ok(bikeType);
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
    return Results.Ok(await rentalService.GetLocationsAsync());
});

app.MapGet("/locations/{id}", async (IRentalService rentalService, string id) =>
{
    var location = await rentalService.GetLocationAsync(id);

    if (location == null)
        return Results.NotFound();

    return Results.Ok(location);
});

app.MapPost("/locations", async (LocationValidation validator, IRentalService rentalService, Location location) =>
{
    var validationResult = validator.Validate(location);
    if (validationResult.IsValid)
    {
        location = await rentalService.AddLocationAsync(location);
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
        location = await rentalService.UpdateLocationAsync(location);

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
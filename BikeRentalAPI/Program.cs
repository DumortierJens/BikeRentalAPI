var builder = WebApplication.CreateBuilder(args);

// Mongo Context
var mongoSettings = builder.Configuration.GetSection("MongoConnection");
builder.Services.Configure<DatabaseSettings>(mongoSettings);
builder.Services.AddTransient<IMongoContext, MongoContext>();

// Repositories
builder.Services.AddTransient<IBikeRepository, BikeRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();
builder.Services.AddTransient<IBikePriceRepository, BikePriceRepository>();
builder.Services.AddTransient<IRentalRepository, RentalRepository>();

// Services
builder.Services.AddTransient<IRentalLocationService, RentalLocationService>();
builder.Services.AddTransient<IRentalService, RentalService>();

// Validation
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BikeValidation>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LocationValidation>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateLocationValidation>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BikePriceValidation>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RentalValidation>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RentalDetailsValidation>());

// GraphQL 
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Queries>();
// .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
// .AddMutationType<Mutation>();

// Swagger Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App
var app = builder.Build();
app.MapGraphQL();
app.MapSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "API is working!");

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerFeature>()
        ?.Error;
    if (exception is not null)
    {
        var response = new { error = exception.Message };
        context.Response.StatusCode = 400;

        await context.Response.WriteAsJsonAsync(response);
    }
}));

#region Bikes

app.MapGet("/bikes", async (IRentalLocationService rentalLocationService) =>
{
    return Results.Ok(await rentalLocationService.GetBikes());
});

app.MapGet("/bikes/{bikeId}", async (IRentalLocationService rentalLocationService, string bikeId) =>
{
    var bike = await rentalLocationService.GetBike(bikeId);

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
        return Results.Created($"/bikes/{bike.Id}", bike);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { error = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPut("/bikes", async (BikeValidation validator, IRentalLocationService rentalLocationService, Bike bike) =>
{
    if (bike.Id == null)
        return Results.NotFound();

    var validationResult = validator.Validate(bike);
    if (validationResult.IsValid)
    {
        bike = await rentalLocationService.UpdateBike(bike);

        if (bike == null)
            return Results.NotFound();

        return Results.Ok(bike);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { error = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

#endregion

#region Locations

app.MapGet("/locations", async (IRentalLocationService rentalLocationService) =>
{
    return Results.Ok(await rentalLocationService.GetLocations());
});

app.MapGet("/locations/{locationId}", async (IRentalLocationService rentalLocationService, string locationId) =>
{
    var location = await rentalLocationService.GetLocation(locationId);

    if (location == null)
        return Results.NotFound();

    return Results.Ok(location);
});

app.MapPost("/locations", async (LocationValidation validator, IRentalLocationService rentalLocationService, RentalLocation location) =>
{
    var validationResult = validator.Validate(location);
    if (validationResult.IsValid)
    {
        location = await rentalLocationService.AddLocation(location);
        return Results.Created($"/locations/{location.Id}", location);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { error = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPut("/locations", async (UpdateLocationValidation validator, IRentalLocationService rentalLocationService, RentalLocation location) =>
{
    if (location.Id == null)
        return Results.NotFound();

    var validationResult = validator.Validate(location);
    if (validationResult.IsValid)
    {
        location = await rentalLocationService.UpdateLocation(location);

        if (location == null)
            return Results.NotFound();

        return Results.Ok(location);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { error = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

#endregion

#region Bike Prices

app.MapGet("/locations/{locationId}/prices", async (IRentalLocationService rentalLocationService, string locationId) =>
{
    var bikePrices = await rentalLocationService.GetBikePricesByLocation(locationId);

    if (bikePrices == null)
        return Results.NotFound();

    return Results.Ok(bikePrices);
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
        var errors = validationResult.Errors.Select(x => new { error = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPut("/prices", async (BikePriceValidation validator, IRentalLocationService rentalLocationService, BikePrice bikePrice) =>
{
    if (bikePrice.Id == null)
        return Results.NotFound();

    var validationResult = validator.Validate(bikePrice);
    if (validationResult.IsValid)
    {
        bikePrice = await rentalLocationService.UpdateBikePrice(bikePrice);
        return Results.Ok(bikePrice);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { error = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

#endregion

#region Rentals

app.MapGet("/rentals/locations/{locationId}", async (IRentalService rentalService, string locationId) =>
{
    var rentals = await rentalService.GetRentalsByLocation(locationId);

    if (rentals == null)
        return Results.NotFound();

    return Results.Ok(rentals);
});

app.MapGet("/rentals/{rentalId}", async (IRentalService rentalService, string rentalId) =>
{
    var rental = await rentalService.GetRental(rentalId);

    if (rental == null)
        return Results.NotFound();

    return Results.Ok(rental);
});

app.MapPost("/rentals/start", async (RentalValidation validator, IRentalService rentalService, Rental rental) =>
{
    var validationResult = validator.Validate(rental);
    if (validationResult.IsValid)
    {
        try
        {
            rental = await rentalService.StartRental(rental);
            return Results.Created($"/rentals/{rental.Id}", rental);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { error = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPost("/rentals/{rentalId}/stop", async (IRentalService rentalService, string rentalId) =>
{
    try
    {
        var rental = await rentalService.StopRental(rentalId);
        return Results.Ok(rental);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPut("/rentals", async (RentalDetailsValidation validator, IRentalService rentalService, Rental rental) =>
{
    var validationResult = validator.Validate(rental);
    if (validationResult.IsValid)
    {
        rental = await rentalService.UpdateRentalDetails(rental);
        return Results.Ok(rental);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { error = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

#endregion

// app.Run("http://localhost:3000");
app.Run();

// For XUnit testing
public partial class Program { }
var builder = WebApplication.CreateBuilder(args);

var mongoSettings = builder.Configuration.GetSection("MongoConnection");
builder.Services.Configure<DatabaseSettings>(mongoSettings);
builder.Services.AddTransient<IMongoContext, MongoContext>();

builder.Services.AddTransient<IBikeTypeRepository, BikeTypeRepository>();

builder.Services.AddTransient<IRentalService, RentalService>();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BikeTypeValidation>());

var app = builder.Build();
app.MapGet("/", () => "API is working!");


#region Bike Types

app.MapGet("/biketypes", async (IRentalService rentalService) =>
{
    return Results.Ok(await rentalService.GetBikeTypesAsync());
});

app.MapGet("/biketypes/{id}", async (IRentalService rentalService, string id) =>
{
    return Results.Ok(await rentalService.GetBikeTypeAsync(id));
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
        return Results.Ok(bikeType);
    }
    else
    {
        var errors = validationResult.Errors.Select(x => new { errors = x.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

#endregion


app.Run("http://localhost:3000");

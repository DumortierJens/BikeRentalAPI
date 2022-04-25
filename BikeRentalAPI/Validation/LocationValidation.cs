namespace BikeRentalAPI.Validation;

public class LocationValidation : AbstractValidator<RentalLocation>
{
    public LocationValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}

public class UpdateLocationValidation : AbstractValidator<RentalLocation>
{
    public UpdateLocationValidation()
    {
        RuleFor(x => x.Id).NotEmpty().Matches(@"^[0-9a-f]{24}$").WithMessage("Id has not a valid 24 digit hex string");
        RuleFor(x => x.Name).NotEmpty();
    }
}
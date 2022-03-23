namespace BikeRentalAPI.Validation;

public class LocationValidation : AbstractValidator<Location>
{
    public LocationValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}



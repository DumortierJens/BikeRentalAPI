namespace BikeRentalAPI.Validation;

public class LocationValidation : AbstractValidator<RentalLocation>
{
    public LocationValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}

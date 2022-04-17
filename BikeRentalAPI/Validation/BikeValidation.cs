namespace BikeRentalAPI.Validation;

public class BikeValidation : AbstractValidator<Bike>
{
    public BikeValidation()
    {
        RuleFor(x => x.Id).Matches(@"^[0-9a-f]{24}$").WithMessage("Id has not a valid 24 digit hex string").When(x => x.Id != null);
        RuleFor(x => x.Name).NotEmpty();
    }
}


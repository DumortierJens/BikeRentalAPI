namespace BikeRentalAPI.Validation;

public class RentalValidation : AbstractValidator<Rental>
{
    public RentalValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Tel).NotEmpty();
        RuleFor(x => x.Bike).NotEmpty();
        RuleFor(x => x.Bike.Id).NotEmpty().Matches(@"^[0-9a-f]{24}$").WithMessage("Id has not a valid 24 digit hex string").When(x => x.Bike != null);
        RuleFor(x => x.Location).NotEmpty();
        RuleFor(x => x.Location.Id).NotEmpty().Matches(@"^[0-9a-f]{24}$").WithMessage("Id has not a valid 24 digit hex string").When(x => x.Location != null);
    }
}

public class RentalDetailsValidation : AbstractValidator<Rental>
{
    public RentalDetailsValidation()
    {
        RuleFor(x => x.Id).NotEmpty().Matches(@"^[0-9a-f]{24}$").WithMessage("Id has not a valid 24 digit hex string");
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Tel).NotEmpty();
    }
}

namespace BikeRentalAPI.Validation;

public class BikePriceValidation : AbstractValidator<BikePrice>
{
    public BikePriceValidation()
    {
        RuleFor(x => x.Id).Matches(@"^[0-9a-f]{24}$").WithMessage("Id has not a valid 24 digit hex string").When(x => x.Id != null);
        RuleFor(x => x.BikeId).NotEmpty().Matches(@"^[0-9a-f]{24}$").WithMessage("BikeId has not a valid 24 digit hex string");
        RuleFor(x => x.LocationId).NotEmpty().Matches(@"^[0-9a-f]{24}$").WithMessage("LocationId has not a valid 24 digit hex string");
        RuleFor(x => x.Prices).NotEmpty().SetValidator(new PricesValidation());
    }
}

public class PricesValidation : AbstractValidator<Prices>
{
    public PricesValidation()
    {
        RuleFor(x => x.HalfDay).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Day).NotEmpty().GreaterThan(0);
        RuleFor(x => x.TwoDays).NotEmpty().GreaterThan(0);
        RuleFor(x => x.TreeDays).NotEmpty().GreaterThan(0);
        RuleFor(x => x.ExtraDay).NotEmpty().GreaterThan(0);
    }
}
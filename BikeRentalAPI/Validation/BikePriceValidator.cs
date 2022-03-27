namespace BikeRentalAPI.Validation;

public class BikePriceValidation : AbstractValidator<BikePrice>
{
    public BikePriceValidation()
    {
        RuleFor(x => x.LocationId).NotEmpty();
        RuleFor(x => x.BikeId).NotEmpty();
        RuleFor(x => x.Prices).NotEmpty().SetValidator(new PricesValidation()).When(x => x.Prices != null);
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
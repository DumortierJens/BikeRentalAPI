namespace BikeRentalAPI.Validation;

public class BikePriceValidation : AbstractValidator<BikePrice>
{
    public BikePriceValidation()
    {
        RuleFor(x => (x.Location)).NotEmpty().SetValidator(new LocationValidation());
        RuleFor(x => x.Bike).NotEmpty().SetValidator(new BikeValidation());
        RuleFor(x => x.Prices).NotEmpty().SetValidator(new PricesValidation());
    }
}

public class BikeValidation : AbstractValidator<Bike>
{
    public BikeValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class LocationValidation : AbstractValidator<Location>
{
    public LocationValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
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
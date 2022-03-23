namespace BikeRentalAPI.Validation;

public class BikeValidation : AbstractValidator<Bike>
{
    public BikeValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
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


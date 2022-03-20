namespace BikeRentalAPI.Validation;

public class BikeTypeValidation : AbstractValidator<BikeType>
{
    public BikeTypeValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Prices).NotEmpty().SetInheritanceValidator(v =>
        {
            v.Add<PriceList>(new PriceListValidation());
        });
    }
}

public class PriceListValidation : AbstractValidator<PriceList>
{
    public PriceListValidation()
    {
        RuleFor(x => x.HalfDay).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Day).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Days2).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Days3).NotEmpty().GreaterThan(0);
        RuleFor(x => x.ExtraDay).NotEmpty().GreaterThan(0);
    }
}


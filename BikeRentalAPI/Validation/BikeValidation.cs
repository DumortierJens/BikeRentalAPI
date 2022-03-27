namespace BikeRentalAPI.Validation;

public class BikeValidation : AbstractValidator<Bike>
{
    public BikeValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}


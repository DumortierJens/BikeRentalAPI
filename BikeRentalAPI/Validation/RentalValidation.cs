namespace BikeRentalAPI.Validation;

public class RentalValidation : AbstractValidator<Rental>
{
    public RentalValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Tel).NotEmpty();
        RuleFor(x => x.Bike).NotEmpty();
        RuleFor(x => x.Bike.Id).NotEmpty().When(x => x.Bike != null);
        RuleFor(x => x.Location).NotEmpty();
        RuleFor(x => x.Location.Id).NotEmpty().When(x => x.Location != null);
    }
}


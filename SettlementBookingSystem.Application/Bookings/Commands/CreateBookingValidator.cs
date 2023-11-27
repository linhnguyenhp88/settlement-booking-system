using FluentValidation;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingValidator()
        {
            RuleFor(b => b.Name).NotEmpty()
                .Matches(@"^[A-Za-z\s]*$")
                .WithMessage("Name should be correct format example: Kevin .")
                .Length(2, 20);

            RuleFor(x => x.BookingTime).NotEmpty()
                .Matches("[0-9]{1,2}:[0-9][0-9]")
                .WithMessage("BookingTime should be correct format example: [09:00]");
        }
    }
}

using FluentValidation;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingValidator()
        {
            RuleFor(b => b.Name).NotEmpty().WithMessage("Name shoud not be empty");
            When(b => !string.IsNullOrEmpty(b.BookingTime), () =>
            {
                RuleFor(b => b.ParseStartTime()).GreaterThanOrEqualTo(new System.TimeSpan(9, 0, 0));
                RuleFor(b => b.ParseStartTime()).LessThanOrEqualTo(new System.TimeSpan(16, 0, 0));
            });
           
        }
    }
}

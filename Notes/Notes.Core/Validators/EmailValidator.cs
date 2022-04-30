using FluentValidation;

namespace Notes.Core.Validators
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(email => email).NotEmpty().EmailAddress();
        }
    }
}

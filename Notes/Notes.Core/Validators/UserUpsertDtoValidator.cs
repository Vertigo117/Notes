using FluentValidation;
using Notes.Core.Contracts;

namespace Notes.Core.Validators
{
    public class UserUpsertDtoValidator : AbstractValidator<UserUpsertDto>
    {
        public UserUpsertDtoValidator()
        {
            RuleFor(user => user.Password).NotEmpty().MinimumLength(6);
            RuleFor(user => user.ConfirmPassword).NotEmpty().Equal(user => user.Password);
            RuleFor(user => user.Email).NotEmpty().EmailAddress();
            RuleFor(user => user.Name).NotEmpty().MaximumLength(30);
        }
    }
}

using FluentValidation;

namespace Notes.Core.Validators
{
    public class IdValidator : AbstractValidator<int>
    {
        public IdValidator()
        {
            RuleFor(id => id).NotEmpty().InclusiveBetween(1, int.MaxValue);
        }
    }
}

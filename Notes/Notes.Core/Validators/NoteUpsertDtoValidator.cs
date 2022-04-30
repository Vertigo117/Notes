using FluentValidation;
using Notes.Core.Contracts;

namespace Notes.Core.Validators
{
    public class NoteUpsertDtoValidator : AbstractValidator<NoteUpsertDto>
    {
        public NoteUpsertDtoValidator()
        {
            RuleFor(note => note.Name).NotEmpty().MaximumLength(100);
            RuleFor(note => note.Text).MaximumLength(1000);
        }
    }
}

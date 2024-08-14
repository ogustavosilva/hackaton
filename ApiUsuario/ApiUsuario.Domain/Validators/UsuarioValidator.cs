using FluentValidation;

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(u => u.Nome).NotEmpty().WithMessage("Nome � obrigat�rio.");
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email � obrigat�rio.").EmailAddress().WithMessage("Email inv�lido.");
        RuleFor(u => u.Senha).NotEmpty().WithMessage("Senha � obrigat�ria.");
    }
}

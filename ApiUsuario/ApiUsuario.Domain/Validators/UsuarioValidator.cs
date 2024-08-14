using FluentValidation;

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(u => u.Nome).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email é obrigatório.").EmailAddress().WithMessage("Email inválido.");
        RuleFor(u => u.Senha).NotEmpty().WithMessage("Senha é obrigatória.");
    }
}

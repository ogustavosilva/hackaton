using FluentValidation;
using ApiUsuario.Domain.Interfaces;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IValidator<Usuario> _validator;

    /// <summary>
    /// Construtor da classe UsuarioService.
    /// </summary>
    /// <param name="usuarioRepository">Repositório de usuários injetado.</param>
    /// <param name="validator">Validador de usuários injetado.</param>
    public UsuarioService(IUsuarioRepository usuarioRepository, IValidator<Usuario> validator)
    {
        _usuarioRepository = usuarioRepository;
        _validator = validator;
    }

    /// <summary>
    /// Obtém todos os usuários.
    /// </summary>
    /// <returns>Lista de usuários.</returns>
    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _usuarioRepository.GetAllAsync();
    }

    /// <summary>
    /// Obtém um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <returns>Usuário correspondente ao ID.</returns>
    public async Task<Usuario> GetByIdAsync(Guid id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Insere um novo usuário.
    /// </summary>
    /// <param name="usuario">Dados do usuário a ser inserido.</param>
    /// <returns>Tarefa assíncrona.</returns>
    /// <exception cref="ValidationException">Lançada quando a validação do usuário falha.</exception>
    public async Task InsertAsync(Usuario usuario)
    {
        var validationResult = await _validator.ValidateAsync(usuario);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        usuario.Id = Guid.NewGuid();
        await _usuarioRepository.InsertAsync(usuario);
    }

    /// <summary>
    /// Atualiza um usuário existente.
    /// </summary>
    /// <param name="usuario">Dados do usuário a serem atualizados.</param>
    /// <returns>Tarefa assíncrona.</returns>
    /// <exception cref="ValidationException">Lançada quando a validação do usuário falha.</exception>
    public async Task UpdateAsync(Usuario usuario)
    {
        var validationResult = await _validator.ValidateAsync(usuario);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _usuarioRepository.UpdateAsync(usuario);
    }

    /// <summary>
    /// Deleta um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser deletado.</param>
    /// <returns>Tarefa assíncrona.</returns>
    public async Task DeleteAsync(Guid id)
    {
        await _usuarioRepository.DeleteAsync(id);
    }
}

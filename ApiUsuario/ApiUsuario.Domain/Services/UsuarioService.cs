using FluentValidation;
using ApiUsuario.Domain.Interfaces;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IValidator<Usuario> _validator;

    /// <summary>
    /// Construtor da classe UsuarioService.
    /// </summary>
    /// <param name="usuarioRepository">Reposit�rio de usu�rios injetado.</param>
    /// <param name="validator">Validador de usu�rios injetado.</param>
    public UsuarioService(IUsuarioRepository usuarioRepository, IValidator<Usuario> validator)
    {
        _usuarioRepository = usuarioRepository;
        _validator = validator;
    }

    /// <summary>
    /// Obt�m todos os usu�rios.
    /// </summary>
    /// <returns>Lista de usu�rios.</returns>
    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _usuarioRepository.GetAllAsync();
    }

    /// <summary>
    /// Obt�m um usu�rio pelo ID.
    /// </summary>
    /// <param name="id">ID do usu�rio.</param>
    /// <returns>Usu�rio correspondente ao ID.</returns>
    public async Task<Usuario> GetByIdAsync(Guid id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Insere um novo usu�rio.
    /// </summary>
    /// <param name="usuario">Dados do usu�rio a ser inserido.</param>
    /// <returns>Tarefa ass�ncrona.</returns>
    /// <exception cref="ValidationException">Lan�ada quando a valida��o do usu�rio falha.</exception>
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
    /// Atualiza um usu�rio existente.
    /// </summary>
    /// <param name="usuario">Dados do usu�rio a serem atualizados.</param>
    /// <returns>Tarefa ass�ncrona.</returns>
    /// <exception cref="ValidationException">Lan�ada quando a valida��o do usu�rio falha.</exception>
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
    /// Deleta um usu�rio pelo ID.
    /// </summary>
    /// <param name="id">ID do usu�rio a ser deletado.</param>
    /// <returns>Tarefa ass�ncrona.</returns>
    public async Task DeleteAsync(Guid id)
    {
        await _usuarioRepository.DeleteAsync(id);
    }
}

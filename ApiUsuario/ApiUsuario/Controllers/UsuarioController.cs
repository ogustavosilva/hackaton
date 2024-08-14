using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using ApiUsuario.Domain.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    /// <summary>
    /// Construtor da classe UsuarioController.
    /// </summary>
    /// <param name="usuarioService">Serviço de usuário injetado.</param>
    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Obtém todos os usuários.
    /// </summary>
    /// <returns>Lista de usuários.</returns>
    [HttpGet("get")]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _usuarioService.GetAllAsync();
        return Ok(usuarios);
    }

    /// <summary>
    /// Obtém um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <returns>Usuário correspondente ao ID.</returns>
    [HttpGet("get-by/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return Ok(usuario);
    }

    /// <summary>
    /// Insere um novo usuário.
    /// </summary>
    /// <param name="usuario">Dados do usuário a ser inserido.</param>
    /// <returns>Resultado da inserção.</returns>
    [HttpPost("insert")]
    public async Task<IActionResult> Insert([FromBody] Usuario usuario)
    {
        try
        {
            await _usuarioService.InsertAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }

    /// <summary>
    /// Atualiza um usuário existente.
    /// </summary>
    /// <param name="id">ID do usuário a ser atualizado.</param>
    /// <param name="usuario">Dados do usuário a serem atualizados.</param>
    /// <returns>Resultado da atualização.</returns>
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return BadRequest("ID do usuário não corresponde.");
        }

        try
        {
            await _usuarioService.UpdateAsync(usuario);
            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }

    /// <summary>
    /// Deleta um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser deletado.</param>
    /// <returns>Resultado da deleção.</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _usuarioService.DeleteAsync(id);
        return NoContent();
    }
}

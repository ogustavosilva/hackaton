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
    /// <param name="usuarioService">Servi�o de usu�rio injetado.</param>
    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Obt�m todos os usu�rios.
    /// </summary>
    /// <returns>Lista de usu�rios.</returns>
    [HttpGet("get")]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _usuarioService.GetAllAsync();
        return Ok(usuarios);
    }

    /// <summary>
    /// Obt�m um usu�rio pelo ID.
    /// </summary>
    /// <param name="id">ID do usu�rio.</param>
    /// <returns>Usu�rio correspondente ao ID.</returns>
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
    /// Insere um novo usu�rio.
    /// </summary>
    /// <param name="usuario">Dados do usu�rio a ser inserido.</param>
    /// <returns>Resultado da inser��o.</returns>
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
    /// Atualiza um usu�rio existente.
    /// </summary>
    /// <param name="id">ID do usu�rio a ser atualizado.</param>
    /// <param name="usuario">Dados do usu�rio a serem atualizados.</param>
    /// <returns>Resultado da atualiza��o.</returns>
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return BadRequest("ID do usu�rio n�o corresponde.");
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
    /// Deleta um usu�rio pelo ID.
    /// </summary>
    /// <param name="id">ID do usu�rio a ser deletado.</param>
    /// <returns>Resultado da dele��o.</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _usuarioService.DeleteAsync(id);
        return NoContent();
    }
}

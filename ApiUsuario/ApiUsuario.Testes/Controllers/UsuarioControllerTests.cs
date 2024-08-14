using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ApiUsuario.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class UsuarioControllerTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock;
    private readonly UsuarioController _usuarioController;

    public UsuarioControllerTests()
    {
        _usuarioServiceMock = new Mock<IUsuarioService>();
        _usuarioController = new UsuarioController(_usuarioServiceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithListOfUsuarios()
    {
        // Arrange
        var usuarios = new List<Usuario> { new Usuario { Nome = "Teste", Email = "teste@teste.com", Senha = "123456" } };
        _usuarioServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(usuarios);

        // Act
        var result = await _usuarioController.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(usuarios, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkResult_WithUsuario()
    {
        // Arrange
        var usuario = new Usuario { Nome = "Teste", Email = "teste@teste.com", Senha = "123456" };
        _usuarioServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(usuario);

        // Act
        var result = await _usuarioController.GetById(Guid.NewGuid());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(usuario, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenUsuarioIsNull()
    {
        // Arrange
        _usuarioServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario)null);

        // Act
        var result = await _usuarioController.GetById(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Insert_ShouldReturnCreatedAtActionResult_WhenUsuarioIsValid()
    {
        // Arrange
        var usuario = new Usuario { Id = Guid.NewGuid(), Nome = "Teste", Email = "teste@teste.com", Senha = "123456" };
        _usuarioServiceMock.Setup(service => service.InsertAsync(usuario)).Returns(Task.CompletedTask);

        // Act
        var result = await _usuarioController.Insert(usuario);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(usuario, createdAtActionResult.Value);
    }

    [Fact]
    public async Task Insert_ShouldReturnBadRequest_WhenValidationExceptionIsThrown()
    {
        // Arrange
        var usuario = new Usuario { Nome = "", Email = "teste@teste.com", Senha = "123456" };
        var validationFailures = new List<ValidationFailure> { new ValidationFailure("Nome", "Nome é obrigatório.") };
        _usuarioServiceMock.Setup(service => service.InsertAsync(usuario)).ThrowsAsync(new ValidationException(validationFailures));

        // Act
        var result = await _usuarioController.Insert(usuario);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(validationFailures, badRequestResult.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenUsuarioIsValid()
    {
        // Arrange
        var usuario = new Usuario { Id = Guid.NewGuid(), Nome = "Teste", Email = "teste@teste.com", Senha = "123456" };
        _usuarioServiceMock.Setup(service => service.UpdateAsync(usuario)).Returns(Task.CompletedTask);

        // Act
        var result = await _usuarioController.Update(usuario.Id, usuario);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var usuario = new Usuario { Id = Guid.NewGuid(), Nome = "Teste", Email = "teste@teste.com", Senha = "123456" };

        // Act
        var result = await _usuarioController.Update(Guid.NewGuid(), usuario);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID do usuário não corresponde.", badRequestResult.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenValidationExceptionIsThrown()
    {
        // Arrange
        var usuario = new Usuario { Id = Guid.NewGuid(), Nome = "", Email = "teste@teste.com", Senha = "123456" };
        var validationFailures = new List<ValidationFailure> { new ValidationFailure("Nome", "Nome é obrigatório.") };
        _usuarioServiceMock.Setup(service => service.UpdateAsync(usuario)).ThrowsAsync(new ValidationException(validationFailures));

        // Act
        var result = await _usuarioController.Update(usuario.Id, usuario);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(validationFailures, badRequestResult.Value);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _usuarioServiceMock.Setup(service => service.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _usuarioController.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}

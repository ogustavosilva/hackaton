using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiUsuario.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IValidator<Usuario>> _validatorMock;
    private readonly UsuarioService _usuarioService;

    public UsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _validatorMock = new Mock<IValidator<Usuario>>();
        _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsuarios()
    {
        // Arrange
        var expectedUsuarios = new List<Usuario> { new Usuario { Nome = "Teste", Email = "teste@teste.com", Senha = "123456" } };
        _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedUsuarios);

        // Act
        var result = await _usuarioService.GetAllAsync();

        // Assert
        Assert.Equal(expectedUsuarios, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUsuario_WhenIdIsValid()
    {
        // Arrange
        var expectedUsuario = new Usuario { Nome = "Teste", Email = "teste@teste.com", Senha = "123456" };
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expectedUsuario);

        // Act
        var result = await _usuarioService.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Equal(expectedUsuario, result);
    }

    [Fact]
    public async Task InsertAsync_ShouldInsertUsuario_WhenValidationPasses()
    {
        // Arrange
        var usuario = new Usuario { Nome = "Teste", Email = "teste@teste.com", Senha = "123456" };
        _validatorMock.Setup(v => v.ValidateAsync(usuario, default)).ReturnsAsync(new ValidationResult());
        _usuarioRepositoryMock.Setup(repo => repo.InsertAsync(usuario)).Returns(Task.CompletedTask);

        // Act
        await _usuarioService.InsertAsync(usuario);

        // Assert
        _validatorMock.Verify(v => v.ValidateAsync(usuario, default), Times.Once);
        _usuarioRepositoryMock.Verify(repo => repo.InsertAsync(usuario), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var usuario = new Usuario { Nome = "Teste", Email = "teste@teste.com", Senha = "123456" };
        var validationFailures = new List<ValidationFailure> { new ValidationFailure("Nome", "Nome é obrigatório") };
        _validatorMock.Setup(v => v.ValidateAsync(usuario, default)).ReturnsAsync(new ValidationResult(validationFailures));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _usuarioService.InsertAsync(usuario));
        _usuarioRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUsuario_WhenValidationPasses()
    {
        // Arrange
        var usuario = new Usuario { Nome = "Teste", Email = "teste@teste.com", Senha = "123456" };
        _validatorMock.Setup(v => v.ValidateAsync(usuario, default)).ReturnsAsync(new ValidationResult());
        _usuarioRepositoryMock.Setup(repo => repo.UpdateAsync(usuario)).Returns(Task.CompletedTask);

        // Act
        await _usuarioService.UpdateAsync(usuario);

        // Assert
        _validatorMock.Verify(v => v.ValidateAsync(usuario, default), Times.Once);
        _usuarioRepositoryMock.Verify(repo => repo.UpdateAsync(usuario), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var usuario = new Usuario { Nome = "Teste", Email = "teste@teste.com", Senha = "123456" };
        var validationFailures = new List<ValidationFailure> { new ValidationFailure("Nome", "Nome é obrigatório") };
        _validatorMock.Setup(v => v.ValidateAsync(usuario, default)).ReturnsAsync(new ValidationResult(validationFailures));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _usuarioService.UpdateAsync(usuario));
        _usuarioRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUsuario()
    {
        // Arrange
        var id = Guid.NewGuid();
        _usuarioRepositoryMock.Setup(repo => repo.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        await _usuarioService.DeleteAsync(id);

        // Assert
        _usuarioRepositoryMock.Verify(repo => repo.DeleteAsync(id), Times.Once);
    }
}

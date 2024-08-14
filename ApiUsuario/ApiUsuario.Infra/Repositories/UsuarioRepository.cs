using Dapper;
using System.Data.SqlClient;
using ApiUsuario.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ApiUsuario.Infra.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Usuario>("SELECT * FROM Usuarios");
            }
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM Usuarios WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task InsertAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("INSERT INTO Usuarios (Id, Nome, Email, Senha) VALUES (@Id, @Nome, @Email, @Senha)", usuario);
            }
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("UPDATE Usuarios SET Nome = @Nome, Email = @Email, Senha = @Senha WHERE Id = @Id", usuario);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });
            }
        }
    }

}
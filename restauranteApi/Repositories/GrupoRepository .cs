// File: GrupoRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class GruposRepository : IGruposRepository
    {
        private readonly SqlConnectionFactory _factory;
        public GruposRepository(SqlConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<Grupos>> GetAllAsync()
        {
            const string sql = @"
            SELECT Id AS id, Grupo AS grupo, Descricao AS descricao, IpImpressora AS ipImpressora,
                   Imagem AS imagem, Ativo AS ativo, DataCadastro AS dataCadastro, DataAlteracao AS dataAlteracao
            FROM Grupos;";
            using var conn = _factory.CreateConnection();
            return await conn.QueryAsync<Grupos>(sql);
        }

        public async Task<Grupos?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT Id AS id, Grupo AS grupo, Descricao AS descricao, IpImpressora AS ipImpressora,
                    Imagem AS imagem, Ativo AS ativo, DataCadastro AS dataCadastro, DataAlteracao AS dataAlteracao
            FROM Grupos WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Grupos>(sql, new { id });
        }

        public async Task<int> CreateAsync(Grupos grupo)
        {
            const string sql = @"
            INSERT INTO Grupos (Grupo, Descricao, IpImpressora, Imagem, Ativo, DataCadastro, DataAlteracao)
            VALUES (@grupo, @descricao, @ipImpressora, @imagem, @ativo, GETDATE(), NULL);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, grupo);
        }

        public async Task<Grupos?> UpdateAsync(Grupos grupo)
        {
            const string sql = @"
            UPDATE Grupos SET Grupo=@grupo, Descricao=@descricao, IpImpressora=@ipImpressora,
                Imagem=@imagem, Ativo=@ativo, DataAlteracao=GETDATE()
            WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, grupo);
            if (rows == 0) return null;
            return await GetByIdAsync(grupo.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Grupos WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}
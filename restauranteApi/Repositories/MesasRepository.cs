// File: MesasRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace restauranteApi.Repositories
{
    public class MesasRepository : IMesasRepository
    {
        private readonly SqlConnectionFactory _factory;

        public MesasRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Mesas>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Mesas";
            using var connection = _factory.CreateConnection();
            return await connection.QueryAsync<Mesas>(sql);
        }

        public async Task<Mesas> GetByIdAsync(int numeroMesa)
        {
            const string sql = "SELECT * FROM Mesas WHERE NumeroMesa = @NumeroMesa";
            using var connection = _factory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Mesas>(sql, new { NumeroMesa = numeroMesa });
        }

        public async Task<bool> CreateAsync(Mesas mesa)
        {
            const string sql = @"
                INSERT INTO Mesas
                    (NumeroMesa, QuantidadeCadeiras, Localizacao, StatusMesa, Ativo, DataCadastro, DataAlteracao)
                VALUES
                    (@NumeroMesa, @QuantidadeCadeiras, @Localizacao, @StatusMesa, @Ativo, GETDATE(), NULL);";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, mesa);
            return affected > 0;
        }

        public async Task<bool> UpdateAsync(Mesas mesa)
        {
            const string sql = @"
                UPDATE Mesas SET
                    QuantidadeCadeiras = @QuantidadeCadeiras,
                    Localizacao = @Localizacao,
                    StatusMesa = @StatusMesa,
                    Ativo = @Ativo,
                    DataAlteracao = GETDATE()
                WHERE NumeroMesa = @NumeroMesa";

            using var connection = _factory.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, mesa);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int numeroMesa)
        {
            const string sql = "DELETE FROM Mesas WHERE NumeroMesa = @NumeroMesa";
            using var connection = _factory.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { NumeroMesa = numeroMesa });
            return rowsAffected > 0;
        }
    }
}
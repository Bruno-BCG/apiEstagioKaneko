using Dapper;
using restauranteApi.Models;
using restauranteApi.Repositories.Interfaces;

namespace restauranteApi.Repositories
{
    public class VeiculosRepository : IVeiculosRepository
    {
        private readonly SqlConnectionFactory _factory;
        public VeiculosRepository(SqlConnectionFactory factory) => _factory = factory;

        private const string BaseJoin = @"
        LEFT JOIN Transportadoras t ON t.Id = v.TransportadoraId
        LEFT JOIN Marcas        m ON m.Id = v.IdMarca";

        private const string SelectV = @"
        SELECT
          -- Veiculos
          v.Id               AS id,
          v.TransportadoraId AS transportadorasId,
          v.IdMarca          AS marcasId,
          v.Placa            AS placa,
          v.Modelo           AS modelo,
          v.AnoFabricacao    AS anoFabricacao,
          v.CapacidadeCargaKg AS capacidadeCargaKg,
          v.Ativo            AS ativo,
          v.DataCadastro     AS dataCadastro,
          v.DataAlteracao    AS dataAlteracao,

          -- marcador + Transportadoras
          t.Id               AS transpIdSplit,
          t.Id               AS id,
          t.Transportadora   AS transportadora,

          -- marcador + Marcas
          m.Id               AS marcaIdSplit,
          m.Id               AS id,
          m.Marca            AS marca,
          m.Ativo            AS ativo,
          m.DataCadastro     AS dataCadastro,
          m.DataAlteracao    AS dataAlteracao
        FROM Veiculos v
        ";

        public async Task<IEnumerable<Veiculos>> GetAllAsync()
        {
            var sql = $@"{SelectV} {BaseJoin};";
            using var conn = _factory.CreateConnection();

            var list = await conn.QueryAsync<Veiculos, Transportadoras, Marcas, Veiculos>(
                sql,
                (v, t, m) => { v.transportadora = t; v.marca = m; return v; },
                splitOn: "transpIdSplit,marcaIdSplit"
            );
            return list;
        }

        public async Task<Veiculos?> GetByIdAsync(int id)
        {
            var sql = $@"{SelectV} {BaseJoin} WHERE v.Id = @id;";
            using var conn = _factory.CreateConnection();

            var rows = await conn.QueryAsync<Veiculos, Transportadoras, Marcas, Veiculos>(
                sql,
                (v, t, m) => { v.transportadora = t; v.marca = m; return v; },
                new { id },
                splitOn: "transpIdSplit,marcaIdSplit"
            );
            return rows.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Veiculos v)
        {
            const string sql = @"
            INSERT INTO Veiculos
            (TransportadoraId, IdMarca, Placa, Modelo, AnoFabricacao, CapacidadeCargaKg, Ativo, DataCadastro, DataAlteracao)
            VALUES
            (@transportadorasId, @marcasId, @placa, @modelo, @anoFabricacao, @capacidadeCargaKg, @ativo, GETDATE(), NULL);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using var conn = _factory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, v);
        }

        public async Task<Veiculos?> UpdateAsync(Veiculos v)
        {
            const string sql = @"
            UPDATE Veiculos SET
              TransportadoraId=@transportadorasId, IdMarca=@marcasId, Placa=@placa, Modelo=@modelo,
              AnoFabricacao=@anoFabricacao, CapacidadeCargaKg=@capacidadeCargaKg, Ativo=@ativo, DataAlteracao=GETDATE()
            WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, v);
            if (rows == 0) return null;
            return await GetByIdAsync(v.id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Veiculos WHERE Id=@id;";
            using var conn = _factory.CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { id });
            return rows > 0;
        }
    }
}

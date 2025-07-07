using restauranteApi;
using restauranteApi.Repositories;
using restauranteApi.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SqlConnectionFactory>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICidadesRepository, CidadesRepository>(); // Add this line
builder.Services.AddScoped<IClientesRepository, ClientesRepository>();
builder.Services.AddScoped<IComprasRepository, ComprasRepository>();
builder.Services.AddScoped<ICondicoesPagamentoRepository, CondicoesPagamentoRepository>();
builder.Services.AddScoped<IEstadosRepository, EstadosRepository>();
builder.Services.AddScoped<IFormasPagamentoRepository, FormasPagamentoRepository>();
builder.Services.AddScoped<IFuncionariosRepository, FuncionariosRepository>();
builder.Services.AddScoped<IFornecedoresRepository, FornecedoresRepository>();
builder.Services.AddScoped<IGrupoRepository, GrupoRepository>();
builder.Services.AddScoped<IItensComprasRepository, ItensComprasRepository>();
builder.Services.AddScoped<IItensPedidosRepository, ItensPedidosRepository>();
builder.Services.AddScoped<IMesasRepository, MesasRepository>();
builder.Services.AddScoped<IPaisesRepository, PaisesRepository>();
builder.Services.AddScoped<IParcelamentosRepository, ParcelamentosRepository>();
builder.Services.AddScoped<IPedidosRepository, PedidosRepository>();
builder.Services.AddScoped<IVendasRepository, VendasRepository>();
builder.Services.AddScoped<IProdutosVendidosRepository, ProdutosVendidosRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

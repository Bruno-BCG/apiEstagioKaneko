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

builder.Services.AddScoped<IPaisesRepository, PaisesRepository>();
builder.Services.AddScoped<IEstadosRepository, EstadosRepository>();
builder.Services.AddScoped<ICidadesRepository, CidadesRepository>();

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

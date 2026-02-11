using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DbContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])    
);         

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/rangos", () =>
{
    return "Rota disponivel";
});

app.MapGet("/rangos/{numero}/{nome}", (int numero, string nome) =>
{
    return $"Pedido de número : {numero}, cliente : {nome}.";
});

app.MapGet("/rangos-disponiveis", (RangoDbContext rangoDbContext) =>
{
    return rangoDbContext.Rangos;
});

app.MapGet("/pedido/{id}", (RangoDbContext rangoDbContext, int id) =>
{
    return rangoDbContext.Rangos.FirstOrDefault(x => x.Id == id);
});


app.MapGet("/ingredientes", () =>
{
    return "Rota disponivel";
});

app.Run();

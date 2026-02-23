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

app.MapGet("/rangos-disponiveis", async (RangoDbContext rangoDbContext) =>
{
    return await rangoDbContext.Rangos.ToListAsync();
});

app.MapGet("/pedido/{id}", async (RangoDbContext rangoDbContext, int id) =>
{
    return await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == id);
});

app.MapGet("/ingredientes", () =>
{
    return "Rota disponivel";
});

app.Run();

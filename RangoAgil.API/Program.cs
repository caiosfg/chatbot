using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DbContexts;
using RangoAgil.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])    
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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


app.MapGet("/searchname", async (RangoDbContext rangoDbContext, [FromQuery(Name = "name")] string? rangoNome) =>
{
   var rangosEntity = await rangoDbContext.Rangos
                                .Where(x => x.Nome.Contains(rangoNome))
                                .ToListAsync();

    if (rangosEntity.Count <= 0 || rangosEntity == null)
        return Results.NoContent();
    else
        return Results.Ok(rangosEntity);
});

app.MapGet("/searchall", async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>>
    (RangoDbContext rangoDbContext,
    IMapper mapper,
    [FromQuery(Name = "name")] string? rangoNome) =>
{
    var rangosEntity = await rangoDbContext.Rangos
                                 .Where(x => rangoNome == null || x.Nome.ToLower().Contains(rangoNome.ToLower()))
                                 .ToListAsync();

    if (rangosEntity.Count <= 0 || rangosEntity == null)
        return TypedResults.NoContent();
    else
        return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
});

app.MapGet("/rangos/{rangoId}/ingredientes", async (RangoDbContext rangoDbContext, int rangoId) =>
{
    return await rangoDbContext.Rangos
                                .Include(rango => rango.Ingredientes)
                                .FirstOrDefaultAsync(rango => rango.Id == rangoId);
});


app.MapGet("/rangos/{rangoId}/ingredientess", async (
    RangoDbContext rangoDbContext,
    IMapper mapper,
    int rangoId) =>
{
    return mapper.Map<IEnumerable<IngredienteDTO>>((await rangoDbContext.Rangos
                                .Include(rango => rango.Ingredientes)
                                .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredientes);
});


app.MapGet("/rangosxxx/{id:int}", async (
    RangoDbContext rangoDbContext,
    IMapper mapper,
    int id) =>
{
    return mapper.Map<RangoDTO>(await rangoDbContext.Rangos
                                .FirstOrDefaultAsync(x => x.Id == id));
});


app.Run();

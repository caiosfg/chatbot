using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DbContexts;
using RangoAgil.API.Entities;
using RangoAgil.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])    
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var rangosEndpoints = app.MapGroup("/rangos");

var rangosComIdEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}");

rangosEndpoints.MapGet("", async (RangoDbContext rangoDbContext) =>
{
    return await rangoDbContext.Rangos.ToListAsync();
});

rangosComIdEndpoints.MapGet("", async (
    RangoDbContext rangoDbContext,
    IMapper mapper,
    int rangoId) =>
{
    return mapper.Map<RangoDTO>(await rangoDbContext.Rangos
                                .FirstOrDefaultAsync(x => x.Id == rangoId));
}).WithName("GetRango");

rangosEndpoints.MapPost("", async (
    RangoDbContext rangoDbContext,
    IMapper mapper,
    [FromBody] RangoParaCriacaoDTO rangoParaCriacaoDTO,
    LinkGenerator linkGenerator,
    HttpContext httpContext
    ) =>
{
    var rangoEntity = mapper.Map<Rango>(rangoParaCriacaoDTO);
    rangoDbContext.Add(rangoEntity);
    await rangoDbContext.SaveChangesAsync();

    var rangoToReturn = mapper.Map<RangoDTO>(rangoEntity);

    var linkToReturn = linkGenerator.GetUriByName(
        httpContext,
        "GetRango",
        new { id = rangoToReturn.Id }
        );

    return TypedResults.Created(
        linkToReturn, rangoToReturn
    );
});


rangosComIdEndpoints.MapPut("", async Task<Results<NotFound, Ok>> (
    RangoDbContext rangoDbContext,
    IMapper mapper,
    int rangoId,
    [FromBody] RangoParaAtualizacaoDTO rangoParaAtualizacaoDTO) =>
{
    var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

    if (rangosEntity == null)
        return TypedResults.NotFound();

    mapper.Map(rangoParaAtualizacaoDTO, rangosEntity);

    await rangoDbContext.SaveChangesAsync();

    return TypedResults.Ok();
});

rangosComIdEndpoints.MapDelete("", async Task<Results<NotFound, NoContent>> (
    RangoDbContext rangoDbContext,
    int rangoId,
    [FromBody] RangoParaAtualizacaoDTO rangoParaAtualizacaoDTO) =>
{
    var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);
    if (rangosEntity == null)
        return TypedResults.NotFound();

    rangoDbContext.Rangos.Remove(rangosEntity);

    await rangoDbContext.SaveChangesAsync();

    return TypedResults.NoContent();
});


app.Run();

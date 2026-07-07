using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DbContexts;
using RangoAgil.API.Entities;
using RangoAgil.API.Models;

public static class RangosEndpoints
{
    public static IEndpointRouteBuilder MapRangosEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/rangos")
            .WithTags("Rangos");

        group.MapGet("/", GetAll)
            .WithName("GetRangos")
            .WithSummary("Lista todos os rangos");

        group.MapGet("/{rangoId:int}", GetById)
            .WithName("GetRango")
            .WithSummary("Obtém um rango pelo Id");

        group.MapPost("/", Create)
            .WithSummary("Cria um novo rango");

        group.MapPut("/{rangoId:int}", Update)
            .WithSummary("Atualiza um rango");

        group.MapDelete("/{rangoId:int}", Delete)
            .WithSummary("Remove um rango");

        return app;
    }

    private static async Task<Ok<List<RangoDTO>>> GetAll(
        RangoDbContext context,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var rangos = await context.Rangos
            .AsNoTracking()
            .ProjectTo<RangoDTO>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(rangos);
    }

    private static async Task<Results<NotFound, Ok<RangoDTO>>> GetById(
        RangoDbContext context,
        IMapper mapper,
        int rangoId,
        CancellationToken cancellationToken)
    {
        var rango = await context.Rangos
            .AsNoTracking()
            .Where(r => r.Id == rangoId)
            .ProjectTo<RangoDTO>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return rango is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(rango);
    }

    private static async Task<CreatedAtRoute<RangoDTO>> Create(
        RangoDbContext context,
        IMapper mapper,
        [FromBody] RangoParaCriacaoDTO rangoParaCriacaoDTO,
        CancellationToken cancellationToken)
    {
        var rango = mapper.Map<Rango>(rangoParaCriacaoDTO);

        context.Rangos.Add(rango);

        await context.SaveChangesAsync(cancellationToken);

        var rangoDto = mapper.Map<RangoDTO>(rango);

        return TypedResults.CreatedAtRoute(
            rangoDto,
            "GetRango",
            new { rangoId = rangoDto.Id });
    }

    private static async Task<Results<NotFound, NoContent>> Update(
        RangoDbContext context,
        IMapper mapper,
        int rangoId,
        [FromBody] RangoParaAtualizacaoDTO rangoParaAtualizacaoDTO,
        CancellationToken cancellationToken)
    {
        var rango = await FindRangoAsync(context, rangoId, cancellationToken);

        if (rango is null)
            return TypedResults.NotFound();

        mapper.Map(rangoParaAtualizacaoDTO, rango);

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<Results<NotFound, NoContent>> Delete(
        RangoDbContext context,
        int rangoId,
        CancellationToken cancellationToken)
    {
        var rango = await FindRangoAsync(context, rangoId, cancellationToken);

        if (rango is null)
            return TypedResults.NotFound();

        context.Rangos.Remove(rango);

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }

    private static Task<Rango?> FindRangoAsync(
        RangoDbContext context,
        int id,
        CancellationToken cancellationToken)
    {
        return context.Rangos
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}
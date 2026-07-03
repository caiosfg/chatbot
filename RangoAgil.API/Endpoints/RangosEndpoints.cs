using AutoMapper;
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
        var group = app.MapGroup("/rangos");

        group.MapGet("/", GetAll);
        group.MapGet("/{rangoId:int}", GetById).WithName("GetRango");
        group.MapPost("/", Create);
        group.MapPut("/{rangoId:int}", Update);
        group.MapDelete("/{rangoId:int}", Delete);

        return app;
    }

    private static Task<List<Rango>> GetAll(RangoDbContext context)
        => context.Rangos.ToListAsync();

    private static async Task<Results<NotFound, Ok<RangoDTO>>> GetById(
        RangoDbContext context,
        IMapper mapper,
        int rangoId)
    {
        var rango = await GetRango(context, rangoId);

        if (rango is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(mapper.Map<RangoDTO>(rango));
    }

    private static async Task<Created<RangoDTO>> Create(
        RangoDbContext context,
        IMapper mapper,
        [FromBody] RangoParaCriacaoDTO rangoParaCriacaoDTO,
        LinkGenerator linkGenerator,
        HttpContext httpContext)
    {
        var rango = mapper.Map<Rango>(rangoParaCriacaoDTO);

        context.Rangos.Add(rango);
        await context.SaveChangesAsync();

        var rangoDto = mapper.Map<RangoDTO>(rango);

        var link = linkGenerator.GetUriByName(
            httpContext,
            "GetRango",
            new { rangoId = rangoDto.Id });

        return TypedResults.Created(link!, rangoDto);
    }

    private static async Task<Results<NotFound, Ok>> Update(
        RangoDbContext context,
        IMapper mapper,
        int rangoId,
        [FromBody] RangoParaAtualizacaoDTO rangoParaAtualizacaoDTO)
    {
        var rango = await GetRango(context, rangoId);

        if (rango is null)
            return TypedResults.NotFound();

        mapper.Map(rangoParaAtualizacaoDTO, rango);

        await context.SaveChangesAsync();

        return TypedResults.Ok();
    }

    private static async Task<Results<NotFound, NoContent>> Delete(
        RangoDbContext context,
        int rangoId)
    {
        var rango = await GetRango(context, rangoId);

        if (rango is null)
            return TypedResults.NotFound();

        context.Rangos.Remove(rango);

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    private static ValueTask<Rango?> GetRango(
        RangoDbContext context,
        int id)
        => context.Rangos.FindAsync(id);
}
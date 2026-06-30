using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DbContexts;
using RangoAgil.API.Entities;
using RangoAgil.API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    private static ValueTask<Rango?> GetRango(
        RangoDbContext context,
        int id)
        => context.Rangos.FindAsync(id);

}
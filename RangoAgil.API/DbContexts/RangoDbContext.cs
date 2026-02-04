using Microsoft.EntityFrameworkCore;
using RangoAgil.API.Entities;

namespace RangoAgil.API.DbContexts;

public class RangoDbContext(DbContextOptions<RangoDbContext> options) : DbContext(options)
{
    public DbSet<Rango> Rangos { get; set; } = null!;
    public DbSet<Ingrediente> Ingredientes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Ingrediente>().HasData(
            new { Id = 1, Nome = "Carne de Vaca" },
            new { Id = 2, Nome = "Cebola" },
            new { Id = 3, Nome = "Alho" },
            new { Id = 4, Nome = "Arroz" },
            new { Id = 5, Nome = "Feijão" },
            new { Id = 6, Nome = "Farofa" }
        );

        _ = modelBuilder.Entity<Rango>().HasData(
            new { Id = 1, Nome = "Vaca atolada" },
            new { Id = 2, Nome = "Bife com Fritas" },
            new { Id = 3, Nome = "Strogonoff de carne" },
            new { Id = 4, Nome = "Escondidinho de carne" },
            new { Id = 5, Nome = "Bife a Cavala" }
        );


        _ = modelBuilder
            .Entity<Rango>()
            .HasMany(d => d.Ingredientes)
            .WithMany(p => p.Rangos)
            .UsingEntity(e => e.HasData(
                new { RangosId = 1, IngredientesId = 1 },
                new { RangosId = 1, IngredientesId = 2 },
                new { RangosId = 1, IngredientesId = 3 },
                new { RangosId = 1, IngredientesId = 4 },
                new { RangosId = 1, IngredientesId = 5 },
                new { RangosId = 1, IngredientesId = 6 }
            ));

        base.OnModelCreating(modelBuilder);
    }
}


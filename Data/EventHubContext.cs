using EventHub.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Api.Data;
public class EventHubContext : DbContext
{
    public EventHubContext(DbContextOptions<EventHubContext> options) : base(options) { }
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Asistente> Asistentes => Set<Asistente>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Evento>().HasData(
            new Evento { Id = 1, Nombre = "Dev Tucumán", Fecha = DateTime.UtcNow.AddDays(7), Ubicacion = "San Miguel de Tucumán" },
            new Evento { Id = 2, Nombre = "Data & Mate", Fecha = DateTime.UtcNow.AddDays(14), Ubicacion = "Yerba Buena" }
        );
    }
}

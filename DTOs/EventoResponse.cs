using EventHub.Api.Models;
namespace EventHub.Api.DTOs;
public class EventoResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public DateTime Fecha { get; set; }
    public string Ubicacion { get; set; } = "";
    public static EventoResponse From(Evento e) => new()
    { Id = e.Id, Nombre = e.Nombre, Fecha = e.Fecha, Ubicacion = e.Ubicacion };
}

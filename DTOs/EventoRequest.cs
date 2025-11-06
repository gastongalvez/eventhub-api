namespace EventHub.Api.DTOs;
public class EventoRequest
{
    public string Nombre { get; set; } = "";
    public DateTime? Fecha { get; set; }
    public string Ubicacion { get; set; } = "";
}

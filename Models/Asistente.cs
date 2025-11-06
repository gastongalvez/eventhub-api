namespace EventHub.Api.Models;

public class Asistente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Email { get; set; } = "";
    public int EventoId { get; set; }
}

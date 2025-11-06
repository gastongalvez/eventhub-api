namespace EventHub.Api.Models;

public class Evento
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public DateTime Fecha { get; set; }
    public string Ubicacion { get; set; } = "";
    public List<Asistente> Asistentes { get; set; } = new();
    public List<Comentario> Comentarios { get; set; } = new();
}

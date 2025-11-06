namespace EventHub.Api.Models;
public class Comentario
{
    public int Id { get; set; }
    public string Autor { get; set; } = "";
    public string Texto { get; set; } = "";
    public DateTime Creado { get; set; } = DateTime.UtcNow;
    public int EventoId { get; set; }
}

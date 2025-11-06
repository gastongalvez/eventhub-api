using EventHub.Api.DTOs;
using EventHub.Api.Models;

namespace EventHub.Api.Services;
public interface IEventosService
{
    (IEnumerable<Evento> items, int total) ObtenerEventos(int limit, int offset, string? ubicacion);
    Evento? ObtenerPorId(int id);
    Evento Crear(EventoRequest request);
    Evento? Patch(int id, string? nombre, DateTime? fecha);
    bool Eliminar(int id);

    (IEnumerable<Asistente> items, int total) ObtenerAsistentes(int eventoId, int page, int limit, string? sort);
    Asistente? AgregarAsistente(int eventoId, string nombre, string email);

    (IEnumerable<Comentario> items, int total) ObtenerComentarios(int eventoId, int limit, int offset);
    Comentario? AgregarComentario(int eventoId, string autor, string texto);
}

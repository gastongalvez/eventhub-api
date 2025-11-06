using EventHub.Api.Data;
using EventHub.Api.DTOs;
using EventHub.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Api.Services;
public class EventosService : IEventosService
{
    private readonly EventHubContext _db;
    public EventosService(EventHubContext db) => _db = db;

    public (IEnumerable<Evento> items, int total) ObtenerEventos(int limit, int offset, string? ubicacion)
    {
        var q = _db.Eventos.AsQueryable();
        if (!string.IsNullOrWhiteSpace(ubicacion))
            q = q.Where(e => e.Ubicacion.ToLower().Contains(ubicacion.ToLower()));

        var total = q.Count();
        var items = q.OrderBy(e => e.Fecha).Skip(offset).Take(limit).ToList();
        return (items, total);
    }

    public Evento? ObtenerPorId(int id) =>
        _db.Eventos.Include(e => e.Asistentes).Include(e => e.Comentarios).FirstOrDefault(e => e.Id == id);

    public Evento Crear(EventoRequest request)
    {
        var e = new Evento { Nombre = request.Nombre, Fecha = request.Fecha!.Value, Ubicacion = request.Ubicacion };
        _db.Eventos.Add(e); _db.SaveChanges(); return e;
    }

    public Evento? Patch(int id, string? nombre, DateTime? fecha)
    {
        var e = _db.Eventos.FirstOrDefault(x => x.Id == id);
        if (e is null) return null;
        if (!string.IsNullOrWhiteSpace(nombre)) e.Nombre = nombre;
        if (fecha.HasValue) e.Fecha = fecha.Value;
        _db.SaveChanges(); return e;
    }

    public bool Eliminar(int id)
    {
        var e = _db.Eventos.FirstOrDefault(x => x.Id == id);
        if (e is null) return false;
        _db.Eventos.Remove(e); _db.SaveChanges(); return true;
    }

    public (IEnumerable<Asistente> items, int total) ObtenerAsistentes(int eventoId, int page, int limit, string? sort)
    {
        var q = _db.Asistentes.Where(a => a.EventoId == eventoId);
        q = sort?.ToLower() == "nombre" ? q.OrderBy(a => a.Nombre) : q.OrderBy(a => a.Id);
        var total = q.Count();
        var items = q.Skip((page - 1) * limit).Take(limit).ToList();
        return (items, total);
    }

    public Asistente? AgregarAsistente(int eventoId, string nombre, string email)
    {
        if (_db.Eventos.Any(e => e.Id == eventoId) is false) return null;
        var a = new Asistente { EventoId = eventoId, Nombre = nombre, Email = email };
        _db.Asistentes.Add(a); _db.SaveChanges(); return a;
    }

    public (IEnumerable<Comentario> items, int total) ObtenerComentarios(int eventoId, int limit, int offset)
    {
        var q = _db.Comentarios.Where(c => c.EventoId == eventoId).OrderByDescending(c => c.Creado);
        var total = q.Count();
        var items = q.Skip(offset).Take(limit).ToList();
        return (items, total);
    }

    public Comentario? AgregarComentario(int eventoId, string autor, string texto)
    {
        if (_db.Eventos.Any(e => e.Id == eventoId) is false) return null;
        var c = new Comentario { EventoId = eventoId, Autor = autor, Texto = texto, Creado = DateTime.UtcNow };
        _db.Comentarios.Add(c); _db.SaveChanges(); return c;
    }
}

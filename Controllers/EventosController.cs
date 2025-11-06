using EventHub.Api.DTOs;
using EventHub.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class EventosController : ControllerBase
{
    private readonly IEventosService _srv;
    public EventosController(IEventosService srv) => _srv = srv;

    [HttpGet]
    public IActionResult GetEventos([FromQuery] string? ubicacion, [FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        if (limit <= 0 || limit > 100) return BadRequest(new { error = "limit debe estar entre 1 y 100" });
        if (offset < 0) return BadRequest(new { error = "offset no puede ser negativo" });
        var (items, total) = _srv.ObtenerEventos(limit, offset, ubicacion);
        var resp = new PaginacionResponse<EventoResponse>
        { Meta = new { total, limit, offset }, Data = items.Select(EventoResponse.From) };
        return Ok(resp);
    }

    [HttpPost]
    public IActionResult CrearEvento([FromBody] EventoRequest body)
    {
        if (body.Fecha is null) return BadRequest(new { error = "El campo 'fecha' es obligatorio" });
        var e = _srv.Crear(body);
        return Created($"/v1/eventos/{e.Id}", EventoResponse.From(e));
    }

    [HttpGet("{id:int}")]
    public IActionResult GetEvento(int id)
    {
        var e = _srv.ObtenerPorId(id);
        return e is null ? NotFound(new { error = "Evento no encontrado" }) : Ok(e);
    }

    public class EventoPatch { public string? Nombre { get; set; } public DateTime? Fecha { get; set; } }

    [HttpPatch("{id:int}")]
    public IActionResult PatchEvento(int id, [FromBody] EventoPatch patch)
    {
        var e = _srv.Patch(id, patch.Nombre, patch.Fecha);
        return e is null ? NotFound(new { error = "Evento no encontrado" }) : Ok(EventoResponse.From(e));
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteEvento(int id)
    {
        var ok = _srv.Eliminar(id);
        return ok ? NoContent() : NotFound(new { error = "Evento no encontrado" });
    }

    [HttpGet("{id:int}/asistentes")]
    public IActionResult GetAsistentes(int id, [FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string? sort = null)
    {
        if (page < 1) return BadRequest(new { error = "page debe ser >= 1" });
        if (limit <= 0 || limit > 100) return BadRequest(new { error = "limit debe estar entre 1 y 100" });
        var ev = _srv.ObtenerPorId(id);
        if (ev is null) return NotFound(new { error = "Evento no encontrado" });
        var (items, total) = _srv.ObtenerAsistentes(id, page, limit, sort);
        return Ok(new { meta = new { total, page, limit, sort }, data = items });
    }

    [HttpGet("{id:int}/comentarios")]
    public IActionResult GetComentarios(int id, [FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        var ev = _srv.ObtenerPorId(id);
        if (ev is null) return NotFound(new { error = "Evento no encontrado" });
        var (items, total) = _srv.ObtenerComentarios(id, limit, offset);
        return Ok(new { meta = new { total, limit, offset }, data = items });
    }

    public record AsistentePost(string Nombre, string Email);
    [HttpPost("{id:int}/asistentes")]
    public IActionResult PostAsistente(int id, [FromBody] AsistentePost body)
    {
        var a = _srv.AgregarAsistente(id, body.Nombre, body.Email);
        return a is null ? NotFound(new { error = "Evento no encontrado" }) : Created($"/v1/eventos/{id}/asistentes/{a.Id}", a);
    }

    public record ComentarioPost(string Autor, string Texto);
    [HttpPost("{id:int}/comentarios")]
    public IActionResult PostComentario(int id, [FromBody] ComentarioPost body)
    {
        var c = _srv.AgregarComentario(id, body.Autor, body.Texto);
        return c is null ? NotFound(new { error = "Evento no encontrado" }) : Created($"/v1/eventos/{id}/comentarios/{c.Id}", c);
    }
}

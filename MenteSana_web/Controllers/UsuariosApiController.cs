using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenteSana_web.Models;

namespace MenteSana_web.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasApiController : ControllerBase
    {
        private readonly MenteSanaDbContext _context;

        public PersonasApiController(MenteSanaDbContext context)
        {
            _context = context;
        }

        // GET: api/personasapi
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var personas = await _context.Persona.ToListAsync();
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var persona = await _context.Persona.FindAsync(id);

            if (persona == null)
                return NotFound();

            return Ok(persona);
        }
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Persona persona)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Por si el cliente envía un id_persona distinto de 0
            // y tu PK es identity en BD
            persona.id_persona = 0;

            _context.Persona.Add(persona);
            await _context.SaveChangesAsync();

            // Devuelve 201 + ubicación del recurso creado
            return CreatedAtAction(nameof(GetById),
                                   new { id = persona.id_persona },
                                   persona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Persona persona)
        {
            if (id != persona.id_persona)
                return BadRequest("El id de la URL y el del cuerpo no coinciden.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personaExistente = await _context.Persona.FindAsync(id);
            if (personaExistente == null)
                return NotFound();

            // Actualizas solo los campos que quieras permitir cambiar
            personaExistente.nombres = persona.nombres;
            personaExistente.apellidos = persona.apellidos;
            personaExistente.correo_institucional = persona.correo_institucional;
            personaExistente.contrasena = persona.contrasena;
            personaExistente.id_rol = persona.id_rol;

            _context.Persona.Update(personaExistente);
            await _context.SaveChangesAsync();

            return NoContent(); // 204, actualizado sin contenido
        }

    }
}

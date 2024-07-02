using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using impedimento_salidaAPI.Context;
using impedimento_salidaAPI.Models;
using impedimento_salidaAPI.Custom;


namespace impedimento_salidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;
        private readonly Utilities _utilities;

        public UsuariosController(ImpedimentoSalidaContext context, Utilities utilities)
        {
            _context = context;
            _utilities = utilities;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            // Validar que el ID en la URL coincide con el ID del usuario
            if (id != usuario.Id)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del usuario.");
            }

            // Buscar el usuario existente en la base de datos
            var usuarioExistente = await _context.Usuarios.FindAsync(id);

            if (usuarioExistente == null)
            {
                return NotFound("El usuario no existe.");
            }

            // Actualizar propiedades del usuario existente con los valores proporcionados
            usuarioExistente.Estatusid = usuario.Estatusid;
            usuarioExistente.Rolid = usuario.Rolid;
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.Username = usuario.Username;

            // Solo actualizar la contraseña si se proporciona una nueva
            if (!string.IsNullOrEmpty(usuario.Password))
            {
                usuarioExistente.Password = _utilities.encriptarSHA256(usuario.Password);
            }

            // Marcar la entidad como modificada
            _context.Entry(usuarioExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound("El usuario no existe.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // O puede devolver Ok(usuarioExistente) para devolver el usuario actualizado
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {

            var modeloUsuario = new Usuario
            {
                Estatusid = usuario.Estatusid,
                Rolid = usuario.Rolid,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Username = usuario.Username,
                Password = _utilities.encriptarSHA256(usuario.Password)
            };

            await _context.Usuarios.AddAsync(modeloUsuario);
            await _context.SaveChangesAsync();

            if (modeloUsuario.Id != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}

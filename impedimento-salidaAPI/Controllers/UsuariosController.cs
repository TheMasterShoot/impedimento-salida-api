using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using impedimento_salidaAPI.Context;
using impedimento_salidaAPI.Models;
using impedimento_salidaAPI.Models.DTOs;
using impedimento_salidaAPI.Custom;
using AutoMapper;


namespace impedimento_salidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;
        private readonly Utilities _utilities;
        private readonly IMapper _mapper;

        public UsuariosController(ImpedimentoSalidaContext context, Utilities utilities, IMapper mapper)
        {
            _context = context;
            _utilities = utilities;
            _mapper = mapper;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.
                Include(u => u.Rol)
                .Include(u => u.Estatus)
                .ToListAsync();
            var usuariosDTO = _mapper.Map<List<UsuarioDTO>>(usuarios);
            return Ok(usuariosDTO);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Estatus)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);
            return Ok(usuarioDTO);
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDTO usuarioDTO)
        {
            if (id != usuarioDTO.Id)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del usuario.");
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(id);

            if (usuarioExistente == null)
            {
                return NotFound("El usuario no existe.");
            }

            usuarioExistente.Nombre = usuarioDTO.Nombre;
            usuarioExistente.Apellido = usuarioDTO.Apellido;
            usuarioExistente.Username = usuarioDTO.Username;

            if (!string.IsNullOrEmpty(usuarioDTO.Password))
            {
                usuarioExistente.Password = _utilities.encriptarSHA256(usuarioDTO.Password);
            }

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

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioDTO usuarioDTO)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDTO);
            usuario.Password = _utilities.encriptarSHA256(usuarioDTO.Password);

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            if (usuario.Id != 0)
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

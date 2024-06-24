using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using impedimento_salidaAPI.Context;
using impedimento_salidaAPI.Models;

namespace impedimento_salidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudLevantamientoController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;

        public SolicitudLevantamientoController(ImpedimentoSalidaContext context)
        {
            _context = context;
        }

        // GET: api/SolicitudLevantamiento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudLevantamiento>>> GetSolicitudLevantamientos()
        {
            return await _context.SolicitudLevantamientos.ToListAsync();
        }

        // GET: api/SolicitudLevantamiento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudLevantamiento>> GetSolicitudLevantamiento(int id)
        {
            var solicitudLevantamiento = await _context.SolicitudLevantamientos.FindAsync(id);

            if (solicitudLevantamiento == null)
            {
                return NotFound();
            }

            return solicitudLevantamiento;
        }

        // PUT: api/SolicitudLevantamiento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitudLevantamiento(int id, SolicitudLevantamiento solicitudLevantamiento)
        {
            if (id != solicitudLevantamiento.Id)
            {
                return BadRequest();
            }

            _context.Entry(solicitudLevantamiento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudLevantamientoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SolicitudLevantamiento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SolicitudLevantamiento>> PostSolicitudLevantamiento(SolicitudLevantamiento solicitudLevantamiento)
        {
            _context.SolicitudLevantamientos.Add(solicitudLevantamiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitudLevantamiento", new { id = solicitudLevantamiento.Id }, solicitudLevantamiento);
        }

        // DELETE: api/SolicitudLevantamiento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitudLevantamiento(int id)
        {
            var solicitudLevantamiento = await _context.SolicitudLevantamientos.FindAsync(id);
            if (solicitudLevantamiento == null)
            {
                return NotFound();
            }

            _context.SolicitudLevantamientos.Remove(solicitudLevantamiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolicitudLevantamientoExists(int id)
        {
            return _context.SolicitudLevantamientos.Any(e => e.Id == id);
        }
    }
}

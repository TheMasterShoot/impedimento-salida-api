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
    public class CertificacionExistenciaController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;

        public CertificacionExistenciaController(ImpedimentoSalidaContext context)
        {
            _context = context;
        }

        // GET: api/CertificacionExistencia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CertificacionExistencium>>> GetCertificacionExistencia()
        {
            return await _context.CertificacionExistencia.ToListAsync();
        }

        // GET: api/CertificacionExistencia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CertificacionExistencium>> GetCertificacionExistencium(int id)
        {
            var certificacionExistencium = await _context.CertificacionExistencia.FindAsync(id);

            if (certificacionExistencium == null)
            {
                return NotFound();
            }

            return certificacionExistencium;
        }

        // PUT: api/CertificacionExistencia/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCertificacionExistencium(int id, CertificacionExistencium certificacionExistencium)
        {
            if (id != certificacionExistencium.Id)
            {
                return BadRequest();
            }

            _context.Entry(certificacionExistencium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificacionExistenciumExists(id))
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

        // POST: api/CertificacionExistencia
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CertificacionExistencium>> PostCertificacionExistencium(CertificacionExistencium certificacionExistencium)
        {
            _context.CertificacionExistencia.Add(certificacionExistencium);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCertificacionExistencium", new { id = certificacionExistencium.Id }, certificacionExistencium);
        }

        // DELETE: api/CertificacionExistencia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCertificacionExistencium(int id)
        {
            var certificacionExistencium = await _context.CertificacionExistencia.FindAsync(id);
            if (certificacionExistencium == null)
            {
                return NotFound();
            }

            _context.CertificacionExistencia.Remove(certificacionExistencium);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CertificacionExistenciumExists(int id)
        {
            return _context.CertificacionExistencia.Any(e => e.Id == id);
        }
    }
}

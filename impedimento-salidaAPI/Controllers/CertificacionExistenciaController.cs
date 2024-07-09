using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using impedimento_salidaAPI.Context;
using impedimento_salidaAPI.Models;
using AutoMapper;
using impedimento_salidaAPI.Models.DTOs;

namespace impedimento_salidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificacionExistenciaController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;
        private readonly IMapper _mapper;


        public CertificacionExistenciaController(ImpedimentoSalidaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/CertificacionExistencia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CertificacionExistenciumDTO>>> GetCertificacionExistencia()
        {
            var certificacionesExistencia = await _context.CertificacionExistencia
                .Include(c => c.Estatus)
                .ToListAsync();
            var certificacionesExistenciaDTO = _mapper.Map<List<CertificacionExistenciumDTO>>(certificacionesExistencia);
            return Ok(certificacionesExistenciaDTO);
        }

        // GET: api/CertificacionExistencia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CertificacionExistenciumDTO>> GetCertificacionExistencium(int id)
        {
            var certificacionExistencium = await _context.CertificacionExistencia
               .Include(c => c.Estatus)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (certificacionExistencium == null)
            {
                return NotFound();
            }

            var certificacionesExistenciaDTO = _mapper.Map<CertificacionExistenciumDTO>(certificacionExistencium);
            return Ok(certificacionesExistenciaDTO);
        }

        // PUT: api/CertificacionExistencia/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCertificacionExistencium(int id, CertificacionExistenciumDTO certificacionExistenciumDTO)
        {
            if (id != certificacionExistenciumDTO.Id)
            {
                return BadRequest();
            }

            var certificacionExistencium = await _context.CertificacionExistencia.FindAsync(id);

            if (certificacionExistencium == null)
            {
                return NotFound();
            }

            _mapper.Map(certificacionExistenciumDTO, certificacionExistencium);

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
        public async Task<ActionResult<CertificacionExistencium>> PostCertificacionExistencium(CertificacionExistenciumDTO certificacionExistenciumDTO)
        {
            var certificacionExistencium = _mapper.Map<CertificacionExistencium>(certificacionExistenciumDTO);
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

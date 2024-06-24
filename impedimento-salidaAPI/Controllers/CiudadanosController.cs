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
    public class CiudadanosController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;

        public CiudadanosController(ImpedimentoSalidaContext context)
        {
            _context = context;
        }

        // GET: api/Ciudadanos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ciudadano>>> GetCiudadanos()
        {
            return await _context.Ciudadanos.ToListAsync();
        }

        // GET: api/Ciudadanos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ciudadano>> GetCiudadano(int id)
        {
            var ciudadano = await _context.Ciudadanos.FindAsync(id);

            if (ciudadano == null)
            {
                return NotFound();
            }

            return ciudadano;
        }

        // PUT: api/Ciudadanos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCiudadano(int id, Ciudadano ciudadano)
        {
            if (id != ciudadano.Id)
            {
                return BadRequest();
            }

            _context.Entry(ciudadano).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CiudadanoExists(id))
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

        // POST: api/Ciudadanos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ciudadano>> PostCiudadano(Ciudadano ciudadano)
        {
            _context.Ciudadanos.Add(ciudadano);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCiudadano", new { id = ciudadano.Id }, ciudadano);
        }

        // DELETE: api/Ciudadanos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCiudadano(int id)
        {
            var ciudadano = await _context.Ciudadanos.FindAsync(id);
            if (ciudadano == null)
            {
                return NotFound();
            }

            _context.Ciudadanos.Remove(ciudadano);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CiudadanoExists(int id)
        {
            return _context.Ciudadanos.Any(e => e.Id == id);
        }
    }
}

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
    public class EstatusController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;

        public EstatusController(ImpedimentoSalidaContext context)
        {
            _context = context;
        }

        // GET: api/Estatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estatus>>> GetEstatuses()
        {
            return await _context.Estatuses.ToListAsync();
        }

        // GET: api/Estatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estatus>> GetEstatus(int id)
        {
            var estatus = await _context.Estatuses.FindAsync(id);

            if (estatus == null)
            {
                return NotFound();
            }

            return estatus;
        }

        // PUT: api/Estatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstatus(int id, Estatus estatus)
        {
            if (id != estatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(estatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstatusExists(id))
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

        // POST: api/Estatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estatus>> PostEstatus(Estatus estatus)
        {
            _context.Estatuses.Add(estatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstatus", new { id = estatus.Id }, estatus);
        }

        // DELETE: api/Estatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstatus(int id)
        {
            var estatus = await _context.Estatuses.FindAsync(id);
            if (estatus == null)
            {
                return NotFound();
            }

            _context.Estatuses.Remove(estatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstatusExists(int id)
        {
            return _context.Estatuses.Any(e => e.Id == id);
        }
    }
}

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
    public class CiudadanosController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;
        private readonly IMapper _mapper;

        public CiudadanosController(ImpedimentoSalidaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Ciudadanos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CiudadanoDTO>>> GetCiudadanos()
        {
            var ciudadanos = await _context.Ciudadanos
                .Include(c => c.Rol)
                .ToListAsync();
            var ciudadanosDTO = _mapper.Map<List<CiudadanoDTO>>(ciudadanos);
            return Ok(ciudadanosDTO);
        }

        // GET: api/Ciudadanos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CiudadanoDTO>> GetCiudadano(int id)
        {
            var ciudadano = await _context.Ciudadanos
               .Include(c => c.Rol)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (ciudadano == null)
            {
                return NotFound();
            }

            var ciudadanosDTO = _mapper.Map<List<CiudadanoDTO>>(ciudadano);
            return Ok(ciudadanosDTO);
        }

        // PUT: api/Ciudadanos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCiudadano(int id, CiudadanoDTO ciudadanoDTO)
        {
            if (id != ciudadanoDTO.Id)
            {
                return BadRequest();
            }

            var ciudadano = await _context.Ciudadanos.FindAsync(id);

            if (ciudadano == null)
            {
                return NotFound();
            }

            _mapper.Map(ciudadanoDTO, ciudadano);

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
        public async Task<ActionResult<Ciudadano>> PostCiudadano(CiudadanoDTO ciudadanoDTO)
        {
            var ciudadano = _mapper.Map<Ciudadano>(ciudadanoDTO);
            _context.Ciudadanos.Add(ciudadano);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCiudadano", new { id = ciudadano.Id }, ciudadanoDTO);
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

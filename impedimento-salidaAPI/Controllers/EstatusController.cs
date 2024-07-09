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
using System.Net.NetworkInformation;

namespace impedimento_salidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatusController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;
        private readonly IMapper _mapper;


        public EstatusController(ImpedimentoSalidaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Estatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstatusDTO>>> GetEstatuses()
        {
            var estatuses = await _context.Estatuses
                .Include(c => c.TipoCodigoNavigation)
                .ToListAsync();
            var estatusDTO = _mapper.Map<List<CiudadanoDTO>>(estatuses);
            return Ok(estatusDTO);
        }

        // GET: api/Estatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstatusDTO>> GetEstatus(int id)
        {
            var estatus = await _context.Estatuses
                .Include(c => c.TipoCodigoNavigation)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (estatus == null)
            {
                return NotFound();
            }

            var estatusDTO = _mapper.Map<List<EstatusDTO>>(estatus);
            return Ok(estatusDTO);
        }

        // PUT: api/Estatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstatus(int id, EstatusDTO estatusDTO)
        {
            if (id != estatusDTO.Id)
            {
                return BadRequest();
            }

            var estatus = await _context.Estatuses.FindAsync(id);

            if (estatus == null)
            {
                return NotFound();
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
        public async Task<ActionResult<Estatus>> PostEstatus(EstatusDTO estatusDTO)
        {
            var estatus = _mapper.Map<Estatus>(estatusDTO);
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

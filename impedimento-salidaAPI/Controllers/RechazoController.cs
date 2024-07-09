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
    public class RechazoController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;
        private readonly IMapper _mapper;


        public RechazoController(ImpedimentoSalidaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Rechazo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RechazoDTO>>> GetRechazos()
        {
            var rechazos = await _context.Rechazos
                .ToListAsync();
            var rechazosDTO = _mapper.Map<List<RechazoDTO>>(rechazos);
            return Ok(rechazosDTO);
        }

        // GET: api/Rechazo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RechazoDTO>> GetRechazo(int id)
        {
            var rechazo = await _context.Rechazos
                .FirstOrDefaultAsync(c => c.Id == id);

            if (rechazo == null)
            {
                return NotFound();
            }

            var rechazosDTO = _mapper.Map<List<RechazoDTO>>(rechazo);
            return Ok(rechazosDTO);
        }

        // PUT: api/Rechazo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRechazo(int id, RechazoDTO rechazoDTO)
        {
            if (id != rechazoDTO.Id)
            {
                return BadRequest();
            }

            var rechazo = await _context.Rechazos.FindAsync(id);

            if (rechazo == null)
            {
                return NotFound();
            }

            _mapper.Map(rechazoDTO, rechazo);

            _context.Entry(rechazo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RechazoExists(id))
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

        // POST: api/Rechazo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rechazo>> PostRechazo(RechazoDTO rechazoDTO)
        {
            var rechazo = _mapper.Map<Rechazo>(rechazoDTO);
            _context.Rechazos.Add(rechazo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRechazo", new { id = rechazo.Id }, rechazo);
        }

        // DELETE: api/Rechazo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRechazo(int id)
        {
            var rechazo = await _context.Rechazos.FindAsync(id);
            if (rechazo == null)
            {
                return NotFound();
            }

            _context.Rechazos.Remove(rechazo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RechazoExists(int id)
        {
            return _context.Rechazos.Any(e => e.Id == id);
        }
    }
}

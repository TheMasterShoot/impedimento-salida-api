using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using impedimento_salidaAPI.Context;
using impedimento_salidaAPI.Models;
using AutoMapper;
using impedimento_salidaAPI.Models.DTOs;
using impedimento_salidaAPI.Custom;

namespace impedimento_salidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudLevantamientoController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;
        private readonly IMapper _mapper;


        public SolicitudLevantamientoController(ImpedimentoSalidaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/SolicitudLevantamiento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudLevantamientoDTO>>> GetSolicitudLevantamientos()
        {
            var solicitudesLevantamiento = await _context.SolicitudLevantamientos
                            .Include(s => s.Estatus)
                            .ToListAsync();
            var solicitudesLevantamientoDTO = _mapper.Map<List<SolicitudLevantamientoDTO>>(solicitudesLevantamiento);
            return Ok(solicitudesLevantamientoDTO);
        }

        // GET: api/SolicitudLevantamiento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudLevantamientoDTO>> GetSolicitudLevantamiento(int id)
        {
            var solicitudLevantamiento = await _context.SolicitudLevantamientos
                .Include(s => s.Estatus)
                .FirstOrDefaultAsync(s => s.Id == id);


            if (solicitudLevantamiento == null)
            {
                return NotFound();
            }

            var solicitudLevantamientoDTO = _mapper.Map<SolicitudLevantamientoDTO>(solicitudLevantamiento);
            return Ok(solicitudLevantamientoDTO);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<SolicitudLevantamiento>> GetSolicitudLevantamiento(int id)
        //{
        //    var solicitudLevantamiento = await _context.SolicitudLevantamientos.FindAsync(id);

        //    if (solicitudLevantamiento == null)
        //    {
        //        return NotFound();
        //    }

        //    return solicitudLevantamiento;
        //}

        // PUT: api/SolicitudLevantamiento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //public async Task<IActionResult> PutSolicitudLevantamiento(int id, SolicitudLevantamientoDTO solicitudLevantamientoDTO)
        //{
        //    if (id != solicitudLevantamientoDTO.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var solicitudLevantamiento = await _context.SolicitudLevantamientos.FindAsync(id);

        //    if (solicitudLevantamiento == null)
        //    {
        //        return NotFound();
        //    }

        //    _mapper.Map(solicitudLevantamientoDTO, solicitudLevantamiento);

        //    _context.Entry(solicitudLevantamientoDTO).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SolicitudLevantamientoExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        public async Task<IActionResult> PutSolicitudLevantamiento(int id, [FromForm] SolicitudLevantamientoDTO solicitudLevantamientoDTO)
        {
            if (id != solicitudLevantamientoDTO.Id)
            {
                return BadRequest();
            }

            var solicitudLevantamiento = await _context.SolicitudLevantamientos.FindAsync(id);
            if (solicitudLevantamiento == null)
            {
                return NotFound();
            }

            // Directorio donde se guardarán los archivos
            var uploadPath = "D:\\Tesis\\impedimento-salidaAPI\\impedimento-salidaAPI\\Documentos\\";

            // Guardar archivo de carta
            if (solicitudLevantamientoDTO.Carta != null && solicitudLevantamientoDTO.Carta.Length > 0)
            {
                var cartaFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.Carta.FileName);
                using (var stream = new FileStream(cartaFileName, FileMode.Create))
                {
                    await solicitudLevantamientoDTO.Carta.CopyToAsync(stream);
                }
                solicitudLevantamiento.Carta = cartaFileName;
            }

            // Guardar archivo de sentencia
            if (solicitudLevantamientoDTO.Sentencia != null && solicitudLevantamientoDTO.Sentencia.Length > 0)
            {
                var sentenciaFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.Sentencia.FileName);
                using (var stream = new FileStream(sentenciaFileName, FileMode.Create))
                {
                    await solicitudLevantamientoDTO.Sentencia.CopyToAsync(stream);
                }
                solicitudLevantamiento.Sentencia = sentenciaFileName;
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


        // PATCH: api/SolicitudLevantamiento/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSolicitudLevantamiento(int id, [FromBody] JsonPatchDocument<SolicitudLevantamiento> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var solicitudLevantamiento = await _context.SolicitudLevantamientos.FindAsync(id);
            if (solicitudLevantamiento == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(solicitudLevantamiento, ModelState);

            if (!TryValidateModel(solicitudLevantamiento))
            {
                return ValidationProblem(ModelState);
            }

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
        //public async Task<ActionResult<SolicitudLevantamiento>> PostSolicitudLevantamiento(SolicitudLevantamientoDTO solicitudLevantamientoDTO)
        //{
        //    var solicitudLevantamiento = _mapper.Map<SolicitudLevantamiento>(solicitudLevantamientoDTO);
        //    _context.SolicitudLevantamientos.Add(solicitudLevantamiento);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetSolicitudLevantamiento", new { id = solicitudLevantamiento.Id }, solicitudLevantamiento);
        //}


        public async Task<ActionResult<SolicitudLevantamiento>> PostSolicitudLevantamiento([FromForm] SolicitudLevantamientoDTO solicitudLevantamientoDTO)
        {
            var solicitudLevantamiento = _mapper.Map<SolicitudLevantamiento>(solicitudLevantamientoDTO);

            // Directorio donde se guardarán los archivos
            var uploadPath = "D:\\Tesis\\impedimento-salidaAPI\\impedimento-salidaAPI\\Documentos\\";

            // Guardar archivo de carta
            if (solicitudLevantamientoDTO.Carta != null && solicitudLevantamientoDTO.Carta.Length > 0)
            {
                var cartaFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.Carta.FileName);
                using (var stream = new FileStream(cartaFileName, FileMode.Create))
                {
                    await solicitudLevantamientoDTO.Carta.CopyToAsync(stream);
                }
                solicitudLevantamiento.Carta = cartaFileName;
            }

            // Guardar archivo de sentencia
            if (solicitudLevantamientoDTO.Sentencia != null && solicitudLevantamientoDTO.Sentencia.Length > 0)
            {
                var sentenciaFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.Sentencia.FileName);
                using (var stream = new FileStream(sentenciaFileName, FileMode.Create))
                {
                    await solicitudLevantamientoDTO.Sentencia.CopyToAsync(stream);
                }
                solicitudLevantamiento.Sentencia = sentenciaFileName;
            }

            // Guardar archivo de noRecurso
            if (solicitudLevantamientoDTO.NoRecurso != null && solicitudLevantamientoDTO.NoRecurso.Length > 0)
            {
                var noRecursoFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.NoRecurso.FileName);
                using (var stream = new FileStream(noRecursoFileName, FileMode.Create))
                {
                    await solicitudLevantamientoDTO.NoRecurso.CopyToAsync(stream);
                }
                solicitudLevantamiento.NoRecurso = noRecursoFileName;
            }

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

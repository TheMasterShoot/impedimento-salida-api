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
using Microsoft.AspNetCore.Hosting;
using Azure.Storage.Blobs;

namespace impedimento_salidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudLevantamientoController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;
        private readonly IMapper _mapper;
        private readonly BlobServiceClient _blobServiceClient;


        public SolicitudLevantamientoController(ImpedimentoSalidaContext context, IMapper mapper, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _mapper = mapper;
            _blobServiceClient = blobServiceClient;
        }

        private async Task<string> UploadFileToBlobAsync(IFormFile file, string containerName, string virtualFolderName)
        {
            // Obtener el contenedor de blobs (crearlo si no existe)
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            // Crear un blob en el contenedor y subir el archivo
            var blobName = $"{virtualFolderName}/{file.FileName}";
            var blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            // Retornar la URL del blob subido
            return blobClient.Uri.ToString();
        }

        // Ejemplo de método para descargar un archivo de Azure Blob Storage
        private async Task<IActionResult> DownloadFileFromBlobAsync(string blobName, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            await blobClient.DownloadToAsync(memory);
            memory.Position = 0;

            return File(memory, GetContentType(blobName), Path.GetFileName(blobName));
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
        public async Task<ActionResult<SolicitudLevantamiento>> GetSolicitudLevantamiento(int id)
        {
            var solicitudLevantamiento = await _context.SolicitudLevantamientos
                .Include(s => s.Estatus)
                .FirstOrDefaultAsync(s => s.Id == id);


            if (solicitudLevantamiento == null)
            {
                return NotFound();
            }

            var solicitudLevantamientoDTO = _mapper.Map<SolicitudLevantamiento>(solicitudLevantamiento);
            return Ok(solicitudLevantamientoDTO);
        }

        [HttpGet("download/{cedula}/{filename}")]
        //public IActionResult DownloadFile(string cedula, string filename)
        public async Task<IActionResult> DownloadFile(string cedula, string filename)
        {
            var containerName = "documentos"; //contenedor
            var virtualFolderName =cedula;

            var blobClient = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient($"{virtualFolderName}/{filename}");

            //var folderPath = "D:\\Tesis\\impedimento-salidaAPI\\impedimento-salidaAPI\\Documentos\\";
            //var filePath = Path.Combine(folderPath, cedula, filename);

            //if (!System.IO.File.Exists(filePath))
            //{
            //    return NotFound(new { Message = "File not found." });
            //}

            //var memory = new MemoryStream();
            //using (var stream = new FileStream(filePath, FileMode.Open))
            //{
            //    stream.CopyTo(memory);
            //}
            //memory.Position = 0;

            //return File(memory, GetContentType(filePath), Path.GetFileName(filePath));

            if (await blobClient.ExistsAsync())
            {
                var memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);
                memoryStream.Position = 0;

                return File(memoryStream, GetContentType(filename), filename);
            }

            return NotFound(new { Message = "File not found." });
        }

        //    private string GetContentType(string path)
        //    {
        //        var types = GetMimeTypes();
        //        var ext = Path.GetExtension(path).ToLowerInvariant();
        //        return types[ext];
        //    }

        //    private Dictionary<string, string> GetMimeTypes()
        //    {
        //        return new Dictionary<string, string>
        //{
        //    {".txt", "text/plain"},
        //    {".pdf", "application/pdf"},
        //    {".doc", "application/vnd.ms-word"},
        //    {".docx", "application/vnd.ms-word"},
        //    {".xls", "application/vnd.ms-excel"},
        //    {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        //    {".png", "image/png"},
        //    {".jpg", "image/jpeg"},
        //    {".jpeg", "image/jpeg"},
        //    {".gif", "image/gif"},
        //    {".csv", "text/csv"}
        //};
        //}
        private string GetContentType(string fileName)
        {
            var types = new Dictionary<string, string>
    {
        {".txt", "text/plain"},
        {".pdf", "application/pdf"},
        {".doc", "application/vnd.ms-word"},
        {".docx", "application/vnd.ms-word"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".csv", "text/csv"}
    };

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }


        // PUT: api/SolicitudLevantamiento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
            solicitudLevantamiento.Email = solicitudLevantamientoDTO.Email;
            solicitudLevantamiento.Estatusid = solicitudLevantamientoDTO.Estatusid;

            //preparacion para almacenamiento en la nube
            var containerName = "documentos"; //contenedor
            var virtualFolderName = solicitudLevantamientoDTO.Cedula;

           
            //var folderName = solicitudLevantamientoDTO.Cedula;  //nombre de nueva carpeta           
            //var folderPath = "D:\\Tesis\\impedimento-salidaAPI\\impedimento-salidaAPI\\Documentos\\"; // Directorio donde se guardarán los archivos
            //var uploadPath = Path.Combine(folderPath, folderName);

            //creacion de la nueva carpeta dentro del directorio
            //if (!Directory.Exists(uploadPath))
            //{
            //    Directory.CreateDirectory(uploadPath);
            //}

            // Guardar archivo de carta
            if (solicitudLevantamientoDTO.Carta != null && solicitudLevantamientoDTO.Carta.Length > 0)
            {
                //var cartaFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.Carta.FileName);
                //using (var stream = new FileStream(cartaFileName, FileMode.Create))
                //{
                //    await solicitudLevantamientoDTO.Carta.CopyToAsync(stream);
                //}
                //solicitudLevantamiento.Carta = cartaFileName;
                if (solicitudLevantamientoDTO.Carta != null && solicitudLevantamientoDTO.Carta.Length > 0);
            }

            // Guardar archivo de sentencia
            if (solicitudLevantamientoDTO.Sentencia != null && solicitudLevantamientoDTO.Sentencia.Length > 0)
            {
                //var sentenciaFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.Sentencia.FileName);
                //using (var stream = new FileStream(sentenciaFileName, FileMode.Create))
                //{
                //    await solicitudLevantamientoDTO.Sentencia.CopyToAsync(stream);
                //}
                //solicitudLevantamiento.Sentencia = sentenciaFileName;
                if (solicitudLevantamientoDTO.Sentencia != null && solicitudLevantamientoDTO.Sentencia.Length > 0);
            }

            // Guardar archivo de noRecurso
            if (solicitudLevantamientoDTO.NoRecurso != null && solicitudLevantamientoDTO.NoRecurso.Length > 0)
            {
                //var noRecursoFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.NoRecurso.FileName);
                //using (var stream = new FileStream(noRecursoFileName, FileMode.Create))
                //{
                //    await solicitudLevantamientoDTO.NoRecurso.CopyToAsync(stream);
                //}
                //solicitudLevantamiento.NoRecurso = noRecursoFileName;
                if (solicitudLevantamientoDTO.NoRecurso != null && solicitudLevantamientoDTO.NoRecurso.Length > 0);
            }


            //continuacion de logica de PUT
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


        //PATCH: api/SolicitudLevantamiento/5
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

            if (!ModelState.IsValid)
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
        public async Task<ActionResult<SolicitudLevantamiento>> PostSolicitudLevantamiento([FromForm] SolicitudLevantamientoDTO solicitudLevantamientoDTO)
        {
            var solicitudLevantamiento = _mapper.Map<SolicitudLevantamiento>(solicitudLevantamientoDTO);

            //preparacion para almacenamiento en la nube
            var containerName = "documentos"; //contenedor
            var virtualFolderName = solicitudLevantamientoDTO.Cedula;

            // preparacion para el guardado de archivo en ruta local
            //var folderName = solicitudLevantamientoDTO.Cedula; //nombre de nueva carpeta           
            //var folderPath = "D:\\Tesis\\impedimento-salidaAPI\\impedimento-salidaAPI\\Documentos\\"; // Directorio donde se guardarán los archivos
            //var uploadPath = Path.Combine(folderPath, folderName);

            //creacion de la nueva carpeta dentro del directorio
            //if (!Directory.Exists(uploadPath))
            //{
            //    Directory.CreateDirectory(uploadPath);
            //}

            // Guardar archivo de carta
            if (solicitudLevantamientoDTO.Carta != null && solicitudLevantamientoDTO.Carta.Length > 0)
            {
                //var cartaFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.Carta.FileName);
                //using (var stream = new FileStream(cartaFileName, FileMode.Create))
                //{
                //    await solicitudLevantamientoDTO.Carta.CopyToAsync(stream);
                //}
                //solicitudLevantamiento.Carta = cartaFileName;
                solicitudLevantamiento.Carta = await UploadFileToBlobAsync(solicitudLevantamientoDTO.Carta, containerName, virtualFolderName);
            }

            // Guardar archivo de sentencia
            if (solicitudLevantamientoDTO.Sentencia != null && solicitudLevantamientoDTO.Sentencia.Length > 0)
            {
                //var sentenciaFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.Sentencia.FileName);
                //using (var stream = new FileStream(sentenciaFileName, FileMode.Create))
                //{
                //    await solicitudLevantamientoDTO.Sentencia.CopyToAsync(stream);
                //}
                //solicitudLevantamiento.Sentencia = sentenciaFileName;
                solicitudLevantamiento.Sentencia = await UploadFileToBlobAsync(solicitudLevantamientoDTO.Sentencia, containerName, virtualFolderName);
            }

            // Guardar archivo de noRecurso
            if (solicitudLevantamientoDTO.NoRecurso != null && solicitudLevantamientoDTO.NoRecurso.Length > 0)
            {
                //var noRecursoFileName = Path.Combine(uploadPath, solicitudLevantamientoDTO.NoRecurso.FileName);
                //using (var stream = new FileStream(noRecursoFileName, FileMode.Create))
                //{
                //    await solicitudLevantamientoDTO.NoRecurso.CopyToAsync(stream);
                //}
                //solicitudLevantamiento.NoRecurso = noRecursoFileName;
                solicitudLevantamiento.NoRecurso = await UploadFileToBlobAsync(solicitudLevantamientoDTO.NoRecurso, containerName, virtualFolderName);
            }

            // continuacion de logica POST
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

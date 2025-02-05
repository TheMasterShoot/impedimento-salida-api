﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using impedimento_salidaAPI.Custom;
using impedimento_salidaAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using impedimento_salidaAPI.Context;
using impedimento_salidaAPI.Models;

namespace impedimento_salidaAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ImpedimentoSalidaContext _context;
        private readonly Utilities _utilities;

        public AuthController(ImpedimentoSalidaContext context, Utilities utilities)
        {
            _context = context;
            _utilities = utilities;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            var usuarioEncontrado = await _context.Usuarios
                                                    .Where(u =>
                                                        u.Username == objeto.Username &&
                                                        u.Password == _utilities.encriptarSHA256(objeto.Password!) &&
                                                        u.Estatusid == 6
                                                      ).FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilities.generarJWT(usuarioEncontrado), user = objeto.Username });
        }

        [HttpGet]
        [Route("ValidarToken")]
        public ActionResult ValidarToken([FromQuery]string token)
        {
            bool respuesta = _utilities.validarToken(token);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}

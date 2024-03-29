﻿using Alura.ListaLeitura.Seguranca;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.Services
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<Usuario> _signInManager;

        public LoginController(SignInManager<Usuario> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Token(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);

                if (result.Succeeded)
                {
                    var direitos = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, model.Login),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                    var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("alura-webapi-authentication-valid"));
                    var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                            issuer: "Alura.WebApp",
                            audience: "Postman",
                            claims: direitos,
                            signingCredentials: credenciais,
                            expires: DateTime.Now.AddMinutes(30)
                        );

                    var tokenRepresentation = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(tokenRepresentation);
                }

                return Unauthorized();
            }

            return BadRequest();
        }
    }
}

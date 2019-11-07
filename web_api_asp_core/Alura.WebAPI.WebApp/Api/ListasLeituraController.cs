using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.WebAPI.WebApp.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListasLeituraController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public ListasLeituraController(IRepository<Livro> repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult TodasListas()
        {
            var paraLer = CriaListaLeitura(TipoListaLeitura.ParaLer);
            var lendo = CriaListaLeitura(TipoListaLeitura.Lendo);
            var lidos = CriaListaLeitura(TipoListaLeitura.Lidos);
            var colecaoLeituras = new List<Lista> { paraLer, lendo, lidos };

            return Ok(colecaoLeituras);
        }

        [HttpGet("{tipo}")]
        public IActionResult Recuperar(TipoListaLeitura tipo)
        {
            var lista = CriaListaLeitura(tipo);

            return Ok(lista);
        }

        public Lista CriaListaLeitura(TipoListaLeitura tipo)
        {
            return new Lista
            {
                Tipo = tipo.ParaString(),
                Livros = _repo.All
                              .Where(l => l.Lista == tipo)
                              .Select(l => l.ToApi())
                              .ToList()
            };
        }
    }
}

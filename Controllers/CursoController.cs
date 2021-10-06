using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using Dio.BackendComNETCore.Models.Cursos;
using Dio.BackendComNETCore.Business.Repositories;
using Dio.BackendComNETCore.Business.Entities;

namespace Dio.BackendComNETCore.Controllers
{
    [Route("api/v1/cursos")]
    [ApiController]
    [Authorize]
    public class CursoController : ControllerBase
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly ILogger<UsuarioController> _logger;

        public CursoController(ICursoRepository cursoRepository, ILogger<UsuarioController> logger)
        {
            _cursoRepository = cursoRepository;
            _logger = logger;
        }

        /// <summary>
        /// Este serviço permite cadastrar curso para o usuário autenticado.
        /// </summary>
        /// <returns>Retorna status 201 e dados do curso do usuário</returns>
        [SwaggerResponse(statusCode: 201, description: "Sucesso ao Cadastrar um curso", Type = typeof(CursoViewModelOutput))]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(CursoViewModelInput cursoViewModelInput)
        {
            try
            {
                Curso curso = new Curso
                {
                    Nome = cursoViewModelInput.Nome,
                    Descricao = cursoViewModelInput.Descricao
                };

                var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                curso.CodigoUsuario = codigoUsuario;
                _cursoRepository.Adicionar(curso);
                _cursoRepository.Commit();

                var cursoViewModelOutput = new CursoViewModelOutput
                {
                    Nome = curso.Nome,
                    Descricao = curso.Descricao,
                };

                return Created("", cursoViewModelOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Este serviço permite obter todos os cursos ativos do usuário.
        /// </summary>
        /// <returns>Retorna status ok e dados do curso do usuário</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao obter os cursos", Type = typeof(CursoViewModelOutput))]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                var cursos = _cursoRepository.ObterPorUsuario(codigoUsuario)
                    .Select(s => new CursoViewModelOutput()
                    {
                        Nome = s.Nome,
                        Descricao = s.Descricao
                    });

                return Ok(cursos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }
        }
    }
}

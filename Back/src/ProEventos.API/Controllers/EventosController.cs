using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController: ControllerBase
    {
        private readonly IEventoServices eventoServices;
        public EventosController(IEventoServices eventoServices)
        {
            this.eventoServices = eventoServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEventos([FromQuery] bool incluirPalestrantes)
        {
            try
            {
                var eventos = await eventoServices.GetAllEventosAsync(incluirPalestrantes);

                if (eventos == null) return NotFound("Nenhum evento cadastrado.");

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar os eventos. Erro: {ex.Message}");
            }
        }

        
        [HttpGet("{eventoId}")]
        public async Task<IActionResult> GetEventoById(int eventoId, [FromQuery] bool incluirPalestrantes)
        {
            try
            {
                var evento = await eventoServices.GetEventoByIdAsync(eventoId, incluirPalestrantes);

                if (evento == null) return NotFound("Evento não cadastrado.");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar o evento. Erro: {ex.Message}");
            }
        }

        [HttpGet("tema/{tema}")]
        public async Task<IActionResult> GetAllEventosByTema(string tema, [FromQuery] bool incluirPalestrantes)
        {
            try
            {
                var eventos = await eventoServices.GetAllEventosByTemaAsync(tema, incluirPalestrantes);

                if (eventos == null) return NotFound("Não foram encontrados eventos para o tema informado");

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar os eventos por tema. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEventos(Evento model)
        {
            try
            {
                var evento = await eventoServices.AddEventos(model);

                if (evento == null) return BadRequest("Não foi possível cadastrar o evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar cadastrar o evento. Erro: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await eventoServices.UpdateEvento(eventoId, model);

                if (evento == null) return BadRequest("Não foi possível atualizar o evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar o evento. Erro: {ex.Message}");
            }
        }

         [HttpDelete]
        public async Task<IActionResult> DeleteEvento(int eventoId)
        {
            try
            {
                if(await eventoServices.DeleteEvento(eventoId))
                    return Ok("Evento excluído com sucesso");
                else 
                    return BadRequest("Não foi possível deletar o evento.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar o evento. Erro: {ex.Message}");
            }
        }
    }
}

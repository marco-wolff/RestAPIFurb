using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPIFurb.Dtos;
using RestAPIFurb.Models;
using RestAPIFurb.Services;

namespace RestAPIFurb.Controllers
{
    [ApiController]
    [Route("api/equipamentos")]
    public class EquipamentosController : ControllerBase
    {
        private readonly IEquipamentoService _service;

        public EquipamentosController(IEquipamentoService service)
        {
            _service = service;
        }

        // GET api/equipamentos -> 200
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var lista = await _service.ListarAsync();
            return Ok(new { equipamentos = lista });
        }

        // GET api/equipamentos/{id} -> 200 ou 404
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resultado = await _service.ObterPorIdAsync(id);
            if (resultado.Tipo == ResultadoTipo.NaoEncontrado)
                return NotFound(new { erro = "Equipamento não encontrado" });

            return Ok(resultado.Dado);
        }

        // POST api/equipamentos -> 201 ou 400
        [HttpPost]
        [Authorize] // rota protegida por JWT, conforme item 3 do enunciado
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] Equipamento equipamento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _service.CriarAsync(equipamento);
            if (resultado.Tipo == ResultadoTipo.ReferenciaInvalida)
                return BadRequest(new { erro = resultado.Mensagem });

            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Dado!.Id }, resultado.Dado);
        }

        // PUT api/equipamentos/{id} -> 200, 400 ou 404 (aceita atualização parcial)
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] EquipamentoPatchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _service.AtualizarParcialAsync(id, dto);

            return resultado.Tipo switch
            {
                ResultadoTipo.NaoEncontrado => NotFound(new { erro = "Equipamento não encontrado" }),
                ResultadoTipo.ReferenciaInvalida => BadRequest(new { erro = resultado.Mensagem }),
                _ => Ok(resultado.Dado)
            };
        }

        // DELETE api/equipamentos/{id} -> 200 ou 404
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(int id)
        {
            var resultado = await _service.RemoverAsync(id);
            if (resultado.Tipo == ResultadoTipo.NaoEncontrado)
                return NotFound(new { erro = "Equipamento não encontrado" });

            return Ok(new { success = new { text = "equipamento removido" } });
        }
    }
}

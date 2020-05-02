using BexsAPI.Models;
using BexsAPI.Repository;
using BexsAPI.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BexsAPI.Controllers
{
    [Route("api/v1/pergunta")]
    [ApiController]
    public class PerguntaController : ControllerBase
    {

        private readonly Repository.Repository _repositorio;


        public PerguntaController(Repository.Repository repositorio)
        {
            _repositorio = repositorio;
        }


        /// <summary>
        /// retorna um lista de perguntas.
        /// </summary>
        /// <response code="200">Lista retornada com sucesso - objeto respostas nunca é retornado</response>
        /// <response code="404">não há nenhum dado para ser retornado pela API</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pergunta>>> GetPerguntas()
        {
            List<Pergunta> list = await _repositorio.PerguntaDB.ToListAsync();

            list.ForEach(pergunta =>
            {
                pergunta.QtdRespostas = _repositorio.RespotasDB.Where(resposta => resposta.PerguntaId == pergunta.PerguntaId).Count();
            });

            if (list.Count == 0)
            {
                return NotFound();
            }
            return list;
        }


        /// <summary>
        /// retorna uma pergunta com base no identificador da pergunta.
        /// </summary>
        /// <param name="perguntaId"> valor retornado na lista de perguntas</param>   
        /// <response code="200">retornado com sucesso - objeto respostas nunca é retornado</response>
        /// <response code="404">nenhum dado localizado com base no parâmetro recebido</response>
        [HttpGet("{perguntaId}")]

        public async Task<ActionResult<Pergunta>> GetPergunta(long perguntaId)
        {
            var pergunta = await _repositorio.PerguntaDB.FindAsync(perguntaId);

            if (pergunta == null)
            {
                return NotFound();
            }

            return pergunta;
        }

        /// <summary>
        /// insere uma pergunta nova no sistema.
        /// </summary> 
        /// <response code="201">Retorna a pergunta criada - objeto respostas nunca é retornado</response>
        /// <response code="400">Dados obrigatórios não localizados na requisição - (autor e texto)</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pergunta>> PostPergunta(PerguntaInput Pergunta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Pergunta perguntaInsert = new Pergunta(Pergunta);
            _repositorio.PerguntaDB.Add(perguntaInsert);

            await _repositorio.SaveChangesAsync();
            return CreatedAtAction("GetPergunta", new { perguntaId = perguntaInsert.PerguntaId }, perguntaInsert);
        }


        /// <summary>
        /// adiciona um voto a pergunta 
        /// </summary> 
        /// <param name="perguntaId"> valor retornado na lista de perguntas</param>  
        /// <response code="204">Atualização realizada com sucesso</response>
        /// <response code="404">perguntaId não localizada no banco de dados</response>
        [HttpPut("{perguntaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPergunta(long perguntaId)
        {
           
            
            Pergunta perguntaSistema = _repositorio.PerguntaDB.FirstOrDefault(pergunta => pergunta.PerguntaId == perguntaId);
           
            if (perguntaSistema == null)
            {
                return NotFound();
            }


            perguntaSistema.QtdVotos++;        
             _repositorio.Entry(perguntaSistema).State = EntityState.Modified;
         
            try
            {
                await _repositorio.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
                
            }

            return NoContent();
        }

        /// <summary>
        /// deleta a pergunta e todas suas respostas.
        /// </summary> 
        /// <returns>sem objeto retornado</returns>
        /// <param name="perguntaId"> valor retornado na lista de perguntas</param>
        /// <response code="204">pergunta deleta com sucesso</response>
        /// <response code="404">perguntaId não localizada no banco de dados</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{perguntaId}")]
        public async Task<IActionResult> DeletePergunta(long perguntaId)
        {
            var pergunta = await _repositorio.PerguntaDB.FindAsync(perguntaId);
            if (pergunta == null)
            {
                return NotFound();
            }

            _repositorio.PerguntaDB.Remove(pergunta);
            await _repositorio.SaveChangesAsync();

            return NoContent();
        }

    }
}
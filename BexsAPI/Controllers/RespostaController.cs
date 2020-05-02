using BexsAPI.Inputs;
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
    [Route("api/v1/{perguntaId}/resposta")]
    [ApiController]
    public class RespostaController : ControllerBase
    {

        private readonly Repository.Repository _repositorio;


        public RespostaController(Repository.Repository repositorio)
        {
            _repositorio = repositorio;
        }

        /// <summary>
        /// retorna uma resposta com base no identificador da pergunta.
        /// </summary>
        /// <param name="perguntaId"> valor retornado na lista de perguntas</param>   
        /// <response code="200">retornado com sucesso</response>
        /// <response code="404">nenhum dado localizado com base no parâmetro recebido</response>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Resposta>>> GetResposta(long perguntaId)
        {
            List<Resposta> resposta = await _repositorio.RespotasDB.Where(resposta => resposta.PerguntaId == perguntaId).ToListAsync();

            if (resposta == null || resposta.Count == 0)
            {
                return NotFound();
            }

            return resposta;
        }


        /// <summary>
        /// insere uma resposta a uma pergunta.
        /// </summary>
        /// <param name="perguntaId"> valor retornado na lista de perguntas</param>
        /// <param name="resposta">valor da nova pergunta a ser inserida</param>   
        /// <response code="201">criado com sucesso</response>
        /// <response code="400">dados obrigatorios nao localiados (autor, pergunta e PerguntaId) ou perguntaId localiado na url diferente no body</response>
        /// <response code="404">pergunta nao localizada no banco de dados</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Resposta>> PostPergunta(long perguntaId, RespostaInput resposta)
        {
            // tipo primitivo não retorna null, logo preciso dessa validação se pergunta igual a 0
            if (!ModelState.IsValid || perguntaId != resposta.PerguntaId || resposta.PerguntaId == 0)
            {
                return BadRequest();
            }

            
            if (!_repositorio.PerguntaDB.Any(pergunta => pergunta.PerguntaId == resposta.PerguntaId))
            {
                return NotFound();
            }

            Resposta respostaInsert = new Resposta(resposta);
            _repositorio.RespotasDB.Add(respostaInsert);

            await _repositorio.SaveChangesAsync();
            return CreatedAtAction("GetResposta", new { perguntaId = respostaInsert.PerguntaId }, respostaInsert);
        }


        /// <summary>
        /// adiciona um voto a uma resposta.
        /// </summary>
        /// <param name="perguntaId"> número de identificação da pergunta</param>   
        /// <param name="respostaId"> número de identificação da resposta</param>   
        /// <response code="204">voto computado com sucesso</response>
        /// <response code="404">respostaId ou perguntaId não localizada no banco</response>
        /// <response code="400">perguntaId não vinculado a essa resposta solicitada</response>
        [HttpPut("{respostaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutResposta(long perguntaId, long respostaId)
        {
           
            
            Resposta respostSistema = _repositorio.RespotasDB.FirstOrDefault(resposta => resposta.RespostaId == respostaId);
            Pergunta perguntaSistema = _repositorio.PerguntaDB.FirstOrDefault(pergunta => pergunta.PerguntaId == perguntaId);

            if (respostSistema == null)
            {
                return NotFound();
            }

            if (perguntaSistema.PerguntaId != respostSistema.PerguntaId)
            {
                return BadRequest();
            }

            respostSistema.QtdVotos++;
            _repositorio.Entry(respostSistema).State = EntityState.Modified;
            
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
        /// deleta uma resposta.
        /// </summary>
        /// <param name="perguntaId"> número de identificação da pergunta</param>   
        /// <param name="respostaId"> número de identificação da resposta</param>   
        /// <response code="204">resposta deleta com sucesso</response>
        /// <response code="404">respostaId ou perguntaId não localizada no banco</response>
        /// <response code="400">perguntaId não vinculado a essa resposta solicitada</response>
        [HttpDelete("{respostaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteResposta(long perguntaId, long respostaId)
        {


            Resposta respostSistema = _repositorio.RespotasDB.FirstOrDefault(resposta => resposta.RespostaId == respostaId);
            Pergunta perguntaSistema = _repositorio.PerguntaDB.FirstOrDefault(pergunta => pergunta.PerguntaId == perguntaId);

            if (respostSistema == null || perguntaSistema == null)
            {
                return NotFound();
            }

            if (perguntaSistema.PerguntaId != respostSistema.PerguntaId)
            {
                return BadRequest();
            }


            _repositorio.RespotasDB.Remove(respostSistema);
            await _repositorio.SaveChangesAsync();

            return NoContent();
        }

       
    }
}
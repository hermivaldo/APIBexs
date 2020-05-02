using BexsAPI.Inputs;
using BexsAPI.Models;
using BexsAPITeste.Controllers.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BexsAPITeste.Controllers.V1
{
    public class RespostaControllerTeste : EstruturaTestes
    {

        PerguntaControllerTeste perguntaControllerTeste = new PerguntaControllerTeste();

        #region OBJETOS COMUNS

        public async Task<IEnumerable<Pergunta>> devolveListaPerguntas()
        {
            await perguntaControllerTeste.PostDadosInformadosCompletoHtmlOK();

            // GARANTIR QUE UTILIZE UM DADO EXISTENTE
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/v1/pergunta");
            var response = await _client.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();
            IEnumerable<Pergunta> retorno = JsonConvert.DeserializeObject<IEnumerable<Pergunta>>(responseBody);

            return retorno;
        }

        public async Task<Resposta> devolveResposta()
        {

            IEnumerable<Pergunta> retorno = await devolveListaPerguntas();

            RespostaInput ObjetoResposta = new RespostaInput
            {
                Autor = "Hermivaldo Braga",
                Texto = "Esse cara precisa ser inserido no banco de dados?",
                PerguntaId = retorno.First().PerguntaId
            };

            var response = await EstruturaBasicaPost(ObjetoResposta);
            string responseBody = await response.Content.ReadAsStringAsync();

            Resposta retornoResposta = JsonConvert.DeserializeObject<Resposta>(responseBody);

            return retornoResposta;
        }
        #endregion

        #region TESTES GET

        [Fact]
        public async Task GetRespostaComIdPerguntaInexistente()
        {

            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/v1/-1/resposta");
            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion

        #region TESTES POST


        private Task<HttpResponseMessage> EstruturaBasicaPost(RespostaInput resposta)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/api/v1/{resposta.PerguntaId}/resposta")
            {
                Content = CreateHttpContent(resposta)
            };
            var response = _client.SendAsync(request);
            return response;
        }

        [Fact]
        public async Task PostRespostaSemDadoObrigatorioAutorBadRequest()
        {
            
            RespostaInput ObjetoResposta = new RespostaInput
            {
                Texto = "posso fazer uma pergunta anônima?",
                PerguntaId = 9
            };
            var response = await EstruturaBasicaPost(ObjetoResposta);
          
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostRespostaSemDadoObrigatorioTextoRespostaBadRequest()
        {

            RespostaInput ObjetoResposta = new RespostaInput
            {
                Autor = "Hermivaldo Braga",
                PerguntaId = 9
            };
            var response = await EstruturaBasicaPost(ObjetoResposta);
           
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostRespostaSemDadoObrigatorioIdPerguntaBadRequest()
        {

            RespostaInput ObjetoResposta = new RespostaInput
            {
                Autor = "Hermivaldo Braga",
                Texto = "posso fazer uma pergunta sem o ID?"
            };
            var response = await EstruturaBasicaPost(ObjetoResposta);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task PostRespostaIdPerguntaInexistenteNotFound()
        {

            RespostaInput ObjetoResposta = new RespostaInput
            {
                Autor = "Hermivaldo Braga",
                Texto = "posso fazer uma pergunta sem o ID?",
                PerguntaId = -1
            };
            var response = await EstruturaBasicaPost(ObjetoResposta);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostRespostaIdPerguntaDiferenteDaUrlBadReques()
        {

            RespostaInput ObjetoResposta = new RespostaInput
            {
                Autor = "Hermivaldo Braga",
                Texto = "posso fazer uma pergunta sem o ID?",
                PerguntaId = -1
            };

            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/api/v1/2/resposta")
            {
                Content = CreateHttpContent(ObjetoResposta)
            };
            var response = await _client.SendAsync(request);
            

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostRespostaIdPerguntaExistenteCreated()
        {


            IEnumerable<Pergunta> retorno = await devolveListaPerguntas();

            RespostaInput ObjetoResposta = new RespostaInput
            {
                Autor = "Hermivaldo Braga",
                Texto = "Esse cara precisa ser inserido no banco de dados?",
                PerguntaId = retorno.First().PerguntaId
            };
            
            var response = await EstruturaBasicaPost(ObjetoResposta);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        #endregion

        #region TESTE PUT

        [Fact]
        public async Task PutInserirVotoPerguntaInexistente()
        {

            // esse teste costuma dar problema de inválido, tentar entender sua estrutura depois.
            Resposta retorno = await this.devolveResposta();

            var request = new HttpRequestMessage(new HttpMethod("PUT"), $"/api/v1/{retorno.PerguntaId}/resposta/{retorno.RespostaId}");
            
            var response = await _client.SendAsync(request);


            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region TESTES DELETE 


        [Fact]
        public async Task DeleteRespostaInexistenteNotFound()
        {


            Resposta retorno = await this.devolveResposta();

            var request = new HttpRequestMessage(new HttpMethod("delete"), $"/api/v1/{retorno.PerguntaId}/resposta/-1");

            var response = await _client.SendAsync(request);


            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteRespostaPerguntaInexistenteNotFound()
        {


            Resposta retorno = await this.devolveResposta();

            var request = new HttpRequestMessage(new HttpMethod("delete"), $"/api/v1/-1/resposta/{retorno.RespostaId}");

            var response = await _client.SendAsync(request);


            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteRespostaPerguntaDiferenteBadRequest()
        {
            // caso não haja duas perguntas no sistema esse teste pode retornar NotFound - tentar melhorar cenário

            Resposta retorno = await this.devolveResposta();

            var request = new HttpRequestMessage(new HttpMethod("delete"), $"/api/v1/{retorno.PerguntaId + 1}/resposta/{retorno.RespostaId}");

            var response = await _client.SendAsync(request);


            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task DeleteRespostaNoContent()
        {
            
            Resposta retorno = await this.devolveResposta();

            var request = new HttpRequestMessage(new HttpMethod("delete"), $"/api/v1/{retorno.PerguntaId}/resposta/{retorno.RespostaId}");

            var response = await _client.SendAsync(request);


            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        #endregion
    }
}

using BexsAPI.Models;
using BexsAPITeste.Controllers.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BexsAPITeste.Controllers.V1
{
    public class PerguntaControllerTeste : EstruturaTestes
    {

        #region OBJETOS COMUNS
        public async Task<IEnumerable<Pergunta>> devolveListaPerguntas()
        {
            await this.PostDadosInformadosCompletoHtmlOK();

            // GARANTIR QUE UTILIZE UM DADO EXISTENTE
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/v1/pergunta");
            var response = await _client.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();
            IEnumerable<Pergunta> retorno = JsonConvert.DeserializeObject<IEnumerable<Pergunta>>(responseBody);

            return retorno;
        }
        #endregion

        #region TESTES POST

        private Task<HttpResponseMessage> EstruturaBasicaPost(Pergunta pergunta)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/pergunta")
            {
                Content = CreateHttpContent(pergunta)
            };
            var response = _client.SendAsync(request);
            return response;
        }
        [Fact]
        public async Task PostDadosEntradaVazioHtmlBadRequest()
        {
            var response = await EstruturaBasicaPost(new Pergunta());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task PostDadosInformandoApenasPerguntaHtmlBadRequest()
        {
            Pergunta pergunta = new Pergunta
            {
                Texto = "PRECISAMOS DE UM BAD REQUEST?"
            };
            var response = await EstruturaBasicaPost(pergunta);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostDadosInformandoApenasAutorHtmlBadRequest()
        {
            Pergunta pergunta = new Pergunta
            {
                Autor = "PRECISAMOS DE UM BAD REQUEST?"
            };
            var response = await EstruturaBasicaPost(pergunta);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostDadosInformadosCompletoHtmlOK()
        {
            Pergunta pergunta = new Pergunta
            {
                Autor = "Hermivaldo Braga",
                Texto = "Teremos um Status ok?"
            };
            var response = await EstruturaBasicaPost(pergunta);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        #endregion

        #region TESTES GET
       /* DEIXAR ESSE MÉTODO COMENTADO POR ENQUANTO, NÃO VEJO A NECESSIDADE DA EXISTÊNCIA DELE
        * [Fact]
        public async Task PegarListaPerguntas()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/v1/pergunta");

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }*/

        [Fact]
        public async Task PegarPerguntaPorId()
        {

            IEnumerable<Pergunta> retorno = await devolveListaPerguntas();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/v1/pergunta/" + retorno.First().PerguntaId);
            var response = await _client.SendAsync(request);

            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PerguntaPorIdInexistente()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/v1/pergunta/100");

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion

        #region TESTES PUT
        private Task<HttpResponseMessage> EstruturaBasicaPut(long Id)
        {
            var request = new HttpRequestMessage(new HttpMethod("PUT"), "/api/v1/pergunta/" + Id);
            var response = _client.SendAsync(request);
            return response;
        }
        

        [Fact]
        public async Task PutIdDadosNaoExistentesDaPerguntaHtmlBadRequest()
        {
            
            var response = await EstruturaBasicaPut(-1);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutIdDadosInsereVotoPerguntaBadRequest()
        {
           
            IEnumerable<Pergunta> retorno = await this.devolveListaPerguntas();

            var response = await EstruturaBasicaPut(retorno.First().PerguntaId);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region TESTES DELETE

        private Task<HttpResponseMessage> EstruturaBasicaDelete(long Id)
        {
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), "/api/v1/pergunta/" + Id);
            
            var response = _client.SendAsync(request);
            return response;
        }

        [Fact]
        public async Task DeletedPerguntaInexistenteNotfound()
        {
            var response = await EstruturaBasicaDelete(-1);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeletedPerguntaInexistente()
        {
            // GARANTIR QUE EXISTA UM DADO NO BANCO DE DADOS PARA ESSE TESTE
            // SÓ PARA GARANTIR CASO O TESTE SEJA CHAMADO ANTES DO POST ** [Category("1st")] NÃO FUNCIONOU.
            await this.PostDadosInformadosCompletoHtmlOK();

            var response = await EstruturaBasicaDelete(1);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        #endregion
    }
}

using BexsAPI.Views;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;


namespace BexsAPI.Models
{
    [Serializable]
    public class Pergunta : PerguntaInput
    {
        public Pergunta()
        {

        }

        public Pergunta(PerguntaInput input)
        {
            this.Autor = input.Autor;
            this.Texto = input.Texto;
            this.DtCriacao = DateTime.Now;
        }

        [BindNever]
        public long PerguntaId { get; set; }

        public DateTime DtCriacao { get; set; }

        public long QtdRespostas { get; set; }
        public long QtdVotos { get; set; }

       
        public ICollection<Resposta> Respostas { get; set; }

    }
}

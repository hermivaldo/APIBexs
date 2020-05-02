using BexsAPI.Inputs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BexsAPI.Models
{
    public class Resposta : RespostaInput
    {

        public Resposta()
        {

        }
        public Resposta(RespostaInput input)
        {
            this.Autor = input.Autor;
            this.Texto = input.Texto;
            this.PerguntaId = input.PerguntaId;
            this.DtCriacao = DateTime.Now;
        }
        [BindNever]
        public long RespostaId { get; set; }

        public DateTime DtCriacao { get; set; }

        public long QtdVotos { get; set; }
        
    }
}

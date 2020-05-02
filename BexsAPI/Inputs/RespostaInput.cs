using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BexsAPI.Inputs
{
    public class RespostaInput
    {
        [Required]
        public String Texto { get; set; }
        [Required]
        public String Autor { get; set; }
        
        [Required]
        public long PerguntaId { get; set; }
    }
}

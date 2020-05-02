using System;
using System.ComponentModel.DataAnnotations;

namespace BexsAPI.Views
{
    public class PerguntaInput
    {

        [Required]
        public String Texto { get; set; }
        [Required]
        public String Autor { get; set; }

    }
}

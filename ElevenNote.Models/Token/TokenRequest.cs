using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNote.Models.Token
{
    public class TokenRequest
    {
        [Required]
        public string UserName {get; set;} = string.Empty;
        [Required]
        public string Password {get; set;} = string.Empty;
    }
}
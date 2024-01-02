using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNote.Models.Note
{
    public class NoteUpdate
    {
        [Required]
        public int Id {get; set;}
        [Required]
        [MinLength(1, ErrorMessage = "{0} must be at least {1} characters long")]
        [MaxLength(100, ErrorMessage = "{0} must be no more than {1} characters long")]
        public string Title {get; set;} = string.Empty;
        [Required]
        [MaxLength(8000, ErrorMessage ="{0} must contain no more than {1} characters")]
        public string Content {get; set;} = string.Empty;
    }
}
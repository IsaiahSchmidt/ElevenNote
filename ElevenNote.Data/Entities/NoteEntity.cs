using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNote.Data.Entities
{
    public class NoteEntity
    {
        [Key]
        public int Id {get; set;}
        [Required]
        [ForeignKey(nameof(Owner))]
        public int OwnerId {get; set;}
        public UserEntity Owner {get; set;} = null!;
        [Required, MinLength(1), MaxLength(100)]
        public string Title {get; set;} = string.Empty;
        [Required, MinLength(1), MaxLength(8000)]
        public string Content {get; set;} = string.Empty;
        [Required]
        public DateTimeOffset CreatedUtc {get; set;}
        public DateTimeOffset? ModifiedUtc {get; set;}
    }
}
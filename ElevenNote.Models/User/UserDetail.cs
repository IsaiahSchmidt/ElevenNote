using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNote.Models.User
{
    public class UserDetail
    {
        public int Id {get; set;}
        public string Email {get; set;}= null!;
        public string UserName {get; set;}= null!;
        public string? FirstName {get; set;} 
        public string? LastName {get; set;} 
        public DateTime DateCreated {get; set;}
    }
}
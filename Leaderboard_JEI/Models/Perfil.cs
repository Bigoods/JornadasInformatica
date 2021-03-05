using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leaderboard_JEI.Models
{
    public class Perfil
    {
        public int Id { get; set; }
        [Display(Name = "Pontos")]
        public int Pontos { get; set; } = 0;
        //[Display(Name = "Rifa1")]
        //public Rifa Rifa1 { get; set; } 
        //[Display(Name = "Rifa2")]
        //public Rifa Rifa2 { get; set; } 
        //[Display(Name = "Rifa3")]
        //public Rifa Rifa3 { get; set; } 
        //[Display(Name = "Rifa4")]
        //public Rifa Rifa4 { get; set; } 
        public string Username { get; set; }
    }
}

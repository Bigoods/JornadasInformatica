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
        [Display(Name = "Rifa1")]
        public int Rifa1 { get; set; } = 0;
        [Display(Name = "Rifa2")]
        public int Rifa2 { get; set; } = 0;
        [Display(Name = "Rifa3")]
        public int Rifa3 { get; set; } = 0;
        [Display(Name = "Rifa4")]
        public int Rifa4 { get; set; } = 0;
        public string Username { get; set; }
    }
}

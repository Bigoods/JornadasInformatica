using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leaderboard_JEI.Models
{
    public class Participante
    {
        public int Id { get; set; }
        [Required]
        public int Num { get; set; }
        [Required]
        public int Pontuacao { get; set; }
    }
}

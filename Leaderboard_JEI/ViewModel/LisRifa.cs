using Leaderboard_JEI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leaderboard_JEI.ViewModel
{
    public class LisRifa
    {
        [Display(Name = "Rifa1")]
        public int NumRifa1 { get; set; }
        [Display(Name = "Rifa2")]
        public int NumRifa2 { get; set; }
        [Display(Name = "Rifa3")]
        public int NumRifa3 { get; set; }
        [Display(Name = "Rifa4")]
        public int NumRifa4 { get; set; }

        public string Username { get; set; }



    }
}

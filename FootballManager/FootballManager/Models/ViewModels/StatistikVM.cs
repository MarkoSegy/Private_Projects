using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager.Models.ViewModels
{
    public class StatistikVM
    {
        public Team Team { get; set; }
        public int TeamAnzahl { get; set; }
        public Person Person { get; set; }
        public int PersonAnzahl { get; set; } 
        public int PersonsPlayInTeams { get; set; }
        public Game Game { get; set; }
        public int GamesAnzahl { get; set; } 

    }
}

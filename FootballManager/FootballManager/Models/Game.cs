using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager.Models
{
    public class Game
    {
        public int Id { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public DateTime GameDate { get; set; }
        public int GoalsTeam1 { get; set; }
        public int GoalsTeam2 { get; set; }
        public bool IsActive { get; set; } = true;

        

    }
}

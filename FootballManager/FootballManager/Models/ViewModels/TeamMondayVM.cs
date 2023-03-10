using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager.Models.ViewModels
{
    public class TeamMondayVM
    {
        public Team team { get; set; }
        public bool isMonday { get; set; } = false;

    }
}

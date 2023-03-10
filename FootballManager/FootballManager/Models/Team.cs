using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public List<Person> TeamPlayerList { get; set; }
        public bool IsActive { get; set; } = true;

    }
}

using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Player : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public string Rank { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Player() { }

        public Player(int id, string name, int level, int score, string rank, DateTime time)
        {
            Id = id;
            Name = name;
            Level = level;
            Score = score;
            Rank = rank;
            RegistrationDate = time;
        }
    }
}

using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLogic
{
    public class Logic
    {
        private List<Player> players = new List<Player>();

        public bool Validate(int id, string name, int level, int score, string rank, DateTime time)
        {
            if (string.IsNullOrWhiteSpace(name) || level <= 0 || score < 0 ||  string.IsNullOrWhiteSpace(rank) || time == DateTime.MinValue)
            {
               return false;
            }
            return true;
        }

        public void Create(int id, string name, int level, int score, string rank, DateTime time) 
        {     
            if (Validate(id, name, level, score, rank, time) == true)
            {
                Player newPlayer = new Player(id, name, level, score, rank, time);
                players.Add(newPlayer);
            }
            else
            {
                throw new ArgumentException($"Игрок не создан");
            }

        }
        public List<List<string>> Read()
        {
            List<List<string>> allPlayers = new List<List<string>>();
            foreach (Player player in players) 
            {
                List<string> listPlayers = new List<string>
                {
                    player.Id.ToString(),
                    player.Name,
                    player.Level.ToString(),
                    player.Score.ToString(),
                    player.Rank,
                    player.RegistrationDate.ToString("dd.MM.yyyy"),

                };
                allPlayers.Add(listPlayers);
            }
            return allPlayers;
        }

        public void Update(int id, string name, int level, int score, string rank, DateTime time)
        {

            var player = players.FirstOrDefault(p => p.Id == id);

            if (player == null)
            {
                throw new ArgumentException($"Игрок с Id={id} не найден");
            }


            if (Validate(id, name, level, score, rank, time) == false)
            {
                throw new ArgumentException($"Игрок не создан");
            }
            player.Name = name;
            player.Level = level;
            player.Score = score;
            player.Rank = rank;
            player.RegistrationDate = time;

        }
        public void Delete(int id)
        {
            var player = players.FirstOrDefault(p => p.Id == id);
            if (player != null)
            {
                players.Remove(player);
            }
            else
            {
                throw new ArgumentException($"Игрок с Id={id} не найден");
            }
        }

        public Dictionary<string, List<Player>> RankGroup()
        {
           
            var grouped = players
                .GroupBy(p => p.Rank)
                .ToDictionary(g => g.Key, g => g.ToList());

            return grouped;
        }

        public List<Player> DateGroup(DateTime startDate, DateTime endDate)
        {
            var filteredPlayers = players
                .Where(p => p.RegistrationDate >= startDate && p.RegistrationDate <= endDate)
                .ToList();

            return filteredPlayers;
        }


    }
}

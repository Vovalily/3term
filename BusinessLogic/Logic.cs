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
        /// <summary>
        /// Валидация игрока
        /// </summary>
        /// <param name="id">Номер игрока</param>
        /// <param name="name">Имя игрока</param>
        /// <param name="level">Уровень игрока</param>
        /// <param name="score">Кол-во очков</param>
        /// <param name="rank">Название ранга</param>
        /// <param name="time">Время регистрации</param>
        /// <returns>True или false</returns>
        public bool Validate(int id, string name, int level, int score, string rank, DateTime time)
        {
            if (string.IsNullOrWhiteSpace(name) || level <= 0 || score < 0 ||  string.IsNullOrWhiteSpace(rank) || time == DateTime.MinValue)
            {
               return false;
            }
            return true;
        }
        /// <summary>
        /// Создание ирока
        /// </summary>
        /// <param name="id">Номер игрока</param>
        /// <param name="name">Имя игрока</param>
        /// <param name="level">Уровень игрока</param>
        /// <param name="score">Кол-во очков</param>
        /// <param name="rank">Название ранга</param>
        /// <param name="time">Время регистрации</param>
        /// <exception cref="ArgumentException">Выводит сообщение при ошибке</exception>
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
        /// <summary>
        /// Показывает всех игроков
        /// </summary>
        /// <returns>Список игроков</returns>
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
        /// <summary>
        /// Изменяет выбранного игрока
        /// </summary>
        /// <param name="id">Номер игрока</param>
        /// <param name="name">Имя игрока</param>
        /// <param name="level">Уровень игрока</param>
        /// <param name="score">Кол-во очков</param>
        /// <param name="rank">Название ранга</param>
        /// <param name="time">Время регистрации</param>
        /// <exception cref="ArgumentException">Выводит сообщение при ошибке</exception>
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
        /// <summary>
        /// Удаляет выбранного игрока
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentException">Выводит сообщение при ошибке</exception>
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
        /// <summary>
        /// Группирует игроков по рангу
        /// </summary>
        /// <returns>Список игроков с одинаковым рангом</returns>
        public Dictionary<string, List<Player>> RankGroup()
        {
           
            var grouped = players
                .GroupBy(p => p.Rank)
                .ToDictionary(g => g.Key, g => g.ToList());

            return grouped;
        }
        /// <summary>
        /// Фильтрует игроков по дата 
        /// </summary>
        /// <param name="startDate">Первая дата</param>
        /// <param name="endDate">Вторая дата</param>
        /// <returns>Список игроков зарегистрированных в выбранный промежуток</returns>
        public List<Player> DateGroup(DateTime startDate, DateTime endDate)
        {
            var filteredPlayers = players
                .Where(p => p.RegistrationDate >= startDate && p.RegistrationDate <= endDate)
                .ToList();

            return filteredPlayers;
        }


    }
}

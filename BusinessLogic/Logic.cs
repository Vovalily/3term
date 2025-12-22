using DataAccessLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly IRepository<Player> Repository;
        private readonly BattleService BattleService;
        public Logic(IRepository<Player> repository, BattleService battleService)
        {
            Repository = repository;
            BattleService = battleService;
        }



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
        public bool Validate( string name, int level, int score, string rank, DateTime time)
        {
            if (string.IsNullOrWhiteSpace(name) || level <= 0 || score < 0 ||  string.IsNullOrWhiteSpace(rank) || time == DateTime.MinValue)
            {
               return false;
            }
            return true;
        }
        /// <summary>
        /// Создание игрока
        /// </summary>
        /// <param name="id">Номер игрока</param>
        /// <param name="name">Имя игрока</param>
        /// <param name="level">Уровень игрока</param>
        /// <param name="score">Кол-во очков</param>
        /// <param name="rank">Название ранга</param>
        /// <param name="time">Время регистрации</param>
        /// <exception cref="ArgumentException">Выводит сообщение при ошибке</exception>
        public void Create(string name, int level, int score, string rank, DateTime time) 
        {     
            if (Validate( name, level, score, rank, time) == true)
            {
                Player newPlayer = new Player(name, level, score, rank, time);
                Repository.Create(newPlayer);
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
        public List<List<string>> ReadAll()
        {
            List<List<string>> allPlayers = new List<List<string>>();
            foreach (Player player in Repository.ReadAll()) 
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

            var player = Repository.ReadById(id);

            if (player == null)
            {
                throw new ArgumentException($"Игрок с Id={id} не найден");
            }


            if (Validate( name, level, score, rank, time) == false)
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
            var player = Repository.ReadById(id);
            if (player != null)
            {
                Repository.Delete(player);
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
           
            var players = Repository.ReadAll();
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
            var players = Repository.ReadAll();
            var filteredPlayers = players
                .Where(p => p.RegistrationDate >= startDate && p.RegistrationDate <= endDate)
                .ToList();

            return filteredPlayers;
        }

        public string Battle(int idFirst, int idSecond)
        {
            var playerOne = Repository.ReadById(idFirst);
            var playerTwo = Repository.ReadById(idSecond);

            if (playerOne == null || playerTwo == null)
                throw new ArgumentException("Один из игроков не найден");

            var (winner, loser, score) = BattleService.Execute(playerOne, playerTwo);

            winner.Score += score;
            loser.Score -= score;

            Repository.Update(winner);
            Repository.Update(loser);

            return $"Победил {winner.Name}! +{score} очков. " +
                   $"Проиграл {loser.Name}! -{score} очков.";
        }

    }
}

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
        private IRepository<Player> _repository;



        public Logic(bool useDapper)
        {
            if (useDapper)
            {
                
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                    throw new Exception("Connection string 'DefaultConnection' not found");

                _repository = new DapperRepository(connectionString);
            }
            else
            {
                var context = new DataContext();
                _repository = new EntityRepository(context);
            }
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
                _repository.Create(newPlayer);
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
            foreach (Player player in _repository.ReadAll()) 
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

            var player = _repository.ReadById(id);

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
            var player = _repository.ReadById(id);
            if (player != null)
            {
                _repository.Delete(player);
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
           
            var players = _repository.ReadAll();
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
            var players = _repository.ReadAll();
            var filteredPlayers = players
                .Where(p => p.RegistrationDate >= startDate && p.RegistrationDate <= endDate)
                .ToList();

            return filteredPlayers;
        }

        public string Battle(int idFirst, int idTwo)
        {
            var playerOne =  _repository.ReadById(idFirst);
            var playerTwo = _repository.ReadById(idTwo);

            if (playerOne == null || playerTwo == null)
            {
                throw new ArgumentException($"Неправильные id игроков");
            }
            
            var rand = new Random();
            int score = rand.Next(0, 100);
            int result = rand.Next(0,2);
            if(result == 0)
            {
                playerOne.Score += score;
                playerTwo.Score -= score;
                return $"Выиграл первый + {score} очков второй проиграл - {score}";
            }
            playerOne.Score -= score;
            playerTwo.Score += score;
            return $"$Выиграл второй +  {score} очков первый проиграл - {score}"
;        }

    }
}

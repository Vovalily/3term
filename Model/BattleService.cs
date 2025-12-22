using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BattleService
    {
        /// <summary>
        /// Проводит бой между двумя игроками и определяет победителя
        /// </summary>
        /// <param name="player1">Первый игрок</param>
        /// <param name="player2">Второй игрок</param>
        /// <returns>Кортеж: (победитель, проигравший, количество очков)</returns>
        public (Player winner, Player loser, int score) Execute(Player player1, Player player2)
        {
            if (player1 == null || player2 == null)
                throw new ArgumentNullException("Игроки не могут быть null");

            var random = new Random();
            int scoreGain = random.Next(1, 101); 
            bool player1Wins = random.Next(2) == 0;

            if (player1Wins)
                return (player1, player2, scoreGain);
            else
                return (player2, player1, scoreGain);
        }
    }
}

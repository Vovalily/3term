using BusinessLogic;
using Model;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
namespace ConsoleApp1
{
    public class Program
    {

        private static Logic logic;
        private static IKernel ninjectKernel;

        static void Main(string[] args)
        {
            ninjectKernel = new StandardKernel(new SimpleConfigModule());
            logic = ninjectKernel.Get<Logic>();

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Создать игрока");
                Console.WriteLine("2. Показать всех игроков");
                Console.WriteLine("3. Обновить игрока");
                Console.WriteLine("4. Удалить игрока");
                Console.WriteLine("5. Группировка по рангу");
                Console.WriteLine("6. Фильтр по дате");
                Console.WriteLine("7. Сражение");
                Console.WriteLine("8. Сменить Dapper или Entity");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        CreatePlayer();
                        break;
                    case "2":
                        ShowAllPlayers();
                        break;
                    case "3":
                        UpdatePlayer();
                        break;
                    case "4":
                        DeletePlayer();
                        break;
                    case "5":
                        ShowRankGroups();
                        break;
                    case "6":
                        ShowPlayersByDate();
                        break;
                    case "7":
                        Battle();
                        break;
                    case "8":
                        Choose();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static void Choose()
        {
            try
            {
                Console.WriteLine("Выберите:");
                Console.WriteLine("1 Dapper");
                Console.WriteLine("2 Entity");
                int choose = int.Parse(Console.ReadLine());
                if (choose == 1)
                {
                    ninjectKernel = new StandardKernel(new SimpleConfigModule(true));
                    logic = ninjectKernel.Get<Logic>();
                    Console.WriteLine("Включен Dapper");

                }

                else
                {
                    ninjectKernel = new StandardKernel(new SimpleConfigModule(false));
                    logic = ninjectKernel.Get<Logic>();
                    Console.WriteLine("Включен Entity");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        static void CreatePlayer()
        {
            try
            {
                Console.Write("Имя: ");
                string name = Console.ReadLine();

                Console.Write("Уровень: ");
                int level = int.Parse(Console.ReadLine());

                Console.Write("Очки: ");
                int score = int.Parse(Console.ReadLine());

                Console.Write("Ранг: ");
                string rank = Console.ReadLine();

                Console.Write("Дата регистрации (дд.мм.гггг): ");
                DateTime date = DateTime.Parse(Console.ReadLine());

                logic.Create(name, level, score, rank, date);
                Console.WriteLine("Игрок создан успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ShowAllPlayers()
        {
            var players = logic.ReadAll();
            if (players.Count == 0)
            {
                Console.WriteLine("Игроков нет.");
                return;
            }

            Console.WriteLine("Список игроков:");
            foreach (var p in players)
            {
                Console.WriteLine(string.Join(" | ", p));
            }
        }

        static void UpdatePlayer()
        {
            try
            {
                Console.Write("Введите id игрока");
                int id = int.Parse(Console.ReadLine());


                Console.Write("Новое имя: ");
                string name = Console.ReadLine();

                Console.Write("Новый уровень: ");
                int level = int.Parse(Console.ReadLine());

                Console.Write("Новые очки: ");
                int score = int.Parse(Console.ReadLine());

                Console.Write("Новый ранг: ");
                string rank = Console.ReadLine();

                Console.Write("Новая дата регистрации (дд.мм.гггг): ");
                DateTime date = DateTime.Parse(Console.ReadLine());

                logic.Update(id, name, level, score, rank, date);
                Console.WriteLine("Игрок обновлён успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void DeletePlayer()
        {
            try
            {
                Console.Write("Id игрока для удаления: ");
                int id = int.Parse(Console.ReadLine());

                logic.Delete(id);
                Console.WriteLine("Игрок удалён успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ShowRankGroups()
        {
            var groups = logic.RankGroup();
            if (groups.Count == 0)
            {
                Console.WriteLine("Игроков нет.");
                return;
            }

            foreach (var group in groups)
            {
                Console.WriteLine($"\nРанг: {group.Key}");
                foreach (var player in group.Value)
                {
                    Console.WriteLine($"  {player.Name} | Уровень: {player.Level} | Очки: {player.Score} | Дата: {player.RegistrationDate:dd.MM.yyyy}");
                }
            }

        }

        static void ShowPlayersByDate()
        {
            try
            {
                Console.Write("Начальная дата (дд.мм.гггг): ");
                DateTime start = DateTime.Parse(Console.ReadLine());

                Console.Write("Конечная дата (дд.мм.гггг): ");
                DateTime end = DateTime.Parse(Console.ReadLine());

                var filtered = logic.DateGroup(start, end);
                if (filtered.Count == 0)
                {
                    Console.WriteLine("Игроков за этот период нет.");
                    return;
                }

                Console.WriteLine("Игроки за указанный период:");
                foreach (var player in filtered)
                {
                    Console.WriteLine($"{player.Name} | Ранг: {player.Rank} | Уровень: {player.Level} | Очки: {player.Score} | Дата: {player.RegistrationDate:dd.MM.yyyy}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void Battle()
        {
            try
            {
                Console.Write("Введите id первого игрока ");
                int idOne = int.Parse(Console.ReadLine());

                Console.Write("Введите id второго игрока ");
                int idTwo = int.Parse(Console.ReadLine());
                
                Console.WriteLine(logic.Battle(idOne, idTwo));

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}

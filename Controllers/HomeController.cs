using GameZone.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace GameZone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /* JAVASCRIPT METHODS */

        public IActionResult GameUpdateButton(string game_genre, string new_sorting_type, bool i_or_d)
        {
            List<string> game_genre_list = ConvertStringToList(game_genre);
            DataBase _database = new DataBase();
            SortingType sorting_type;

            switch (new_sorting_type.ToLower())
            {
                case "namegame":
                    sorting_type = SortingType.NAME;
                    break;
                case "date":
                    sorting_type = SortingType.DATE_OF_CREATION;
                    break;
                case "cost":
                    sorting_type = SortingType.COST;
                    break;
                case "rate":
                    sorting_type = SortingType.RATE;
                    break;
                default:
                    return BadRequest("Invalid sorting type.");
            }

            Game[] games;
            List<List<string>> sortedResult = new List<List<string>>();
            if (i_or_d)
            {
                games = SortedDatabase(sorting_type, SortingMethod.DECREASING, game_genre_list);
            }
            else
            {
                games = SortedDatabase(sorting_type, SortingMethod.INCREASING, game_genre_list);
            }

            foreach (var f in games)
            {
                sortedResult.Add(Form(f));
            }
            return Ok(sortedResult);

        }
        public IActionResult GraphsUpdateButton(int id, string dates)
        {
            List<string> when;
            List<string> result = new();
            DataBase _database = new DataBase();

            when = ConvertStringToList(dates);
            foreach (string date in when)
            {
                result.Add(_database.GetProfit(id, ConvertDateFormat(date)));
            }
            result.Add(_database.GetGameCreationDate(id));
            return Ok(result);
        }

        public IActionResult DeleteSlotMachineButton(int id)
        {
            DataBase _database = new();
            _database.DeleteGameById(id);
            return Ok(0);
        }

        public IActionResult ChangeSlotMachineButton(int id, string name, string type, string description, int cost, string date, double rate)
        {
            date = ConvertDateFormat(date);
            description = DescriptioString(description);
            Game xxx = new Game(name, type, description, cost, rate, date, id);
            DataBase _database = new();
            bool luck;
            luck = _database.UpdateGame(xxx);
            if (luck == false)
                return Ok(-1);
            else
                return Ok(0);
        }

        public IActionResult GetInfoSlotMachine(int id)
        {
            DataBase _database = new();
            Game game = _database.GetGameById(id);
            List<string> game_l = new List<string>();
            game_l.Add(game.Name);
            game_l.Add(game.Type);
            game_l.Add(game.Description);
            game_l.Add(Convert.ToString(game.Cost));
            game_l.Add(ConvertDateFormat(game.CreateDate));
            game_l.Add(Convert.ToString(game.Rate));

            return Ok(game_l);
        }

        public IActionResult GetInfoDate(string date, int id)
        {
            DataBase _database = new();
            Dates d = _database.GetDate(id, ConvertDateFormat(date));
            if(d == null)
                return Ok(0);
            return Ok(d.Count);
        }

        public IActionResult EditDate(string date, int id, int newValue)
        {
            DataBase _database = new();
            Dates f = _database.GetDate(id, ConvertDateFormat(date));
            if (f == null)
            { 
                Dates ddd = new Dates(ConvertDateFormat(date), newValue, id);   
                _database.AddDate(ddd);
                _database.ChangeDate(ddd.Id, ConvertDateFormat(ddd.Date), newValue);
                return Ok(0);
            }
            _database.ChangeDate(f.Id, ConvertDateFormat(date), newValue);
            return Ok(0);
        }

        public IActionResult ResetAll()
        {
            DataBase d = new DataBase();
            d.CreateTables();
            d.FillTables();

            return Ok(0);
        }

        public IActionResult AddSlotMachineButton(string name, string type, string description, int cost, string date, double rate)
        {
            DataBase _database = new();
            bool Artem;
            date = ConvertDateFormat(date);
            description = DescriptioString(description);
            Game gg = new Game(name, type, description, cost, rate, date);
            Artem = _database.AddGame(gg);
            if (Artem == true)
            {
                List<string> chto_to = Form(gg);
                return Ok(chto_to);
            }
            return Ok(-1);
        }

        public void AddDateIfNotExists(Dates dates)
        {
            DataBase _database = new();
            if (_database.DateExistsForGame(dates.Date, dates.Game_Id))
            {
                Console.WriteLine("Date already exists for this game.");
            }
            else
            {
                _database.AddDate(dates);
                Console.WriteLine("Date added successfully.");
            }
        }

        public IActionResult ChartUpdateButton(string date_start, string date_end)
        {
            DataBase _database = new DataBase();
            List<List<List<string>>> result = new List<List<List<string>>>();

            string[] gameTypes = new[] { "Sport", "Race", "Shoot", "Automat" };
            date_start = ConvertDateFormat(date_start);
            date_end = ConvertDateFormat(date_end);

            foreach (var gameType in gameTypes)
            {
                //date_start = ConvertDateFormat(date_start);
                //date_end = ConvertDateFormat(date_end);
                List<List<string>> games = _database.GetGameCountsByType(date_start, date_end, gameType);
                games.Sort((list1, list2) =>
                {
                    int minLength = Math.Min(list1.Count, list2.Count);
                    for (int i = 0; i < minLength; i++)
                    {
                        int comparison = string.Compare(list1[i], list2[i], StringComparison.OrdinalIgnoreCase);
                        if (comparison != 0)
                        {
                            return comparison;
                        }
                    }
                    return list1.Count.CompareTo(list2.Count);
                });
                games.Reverse();
                result.Add(games);
            }

            List<string> smallList = new List<string>() { ConvertDateFormat(_database.GetEarliestCreationDate()) };
            List<List<string>> bigList = new List<List<string>>() { smallList };
            result.Add(bigList);

            return Ok(result);
        }

        /* OTHER METHODS */

        public List<string> Form(Game game)
        {
            if (game.Type == "Shoot")
            {
                game.Type = "Стрельба";
            }
            if (game.Type == "Sport")
            {
                game.Type = "Спорт";
            }
            if (game.Type == "Race")
            {
                game.Type = "Гонки";
            }
            if (game.Type == "Automat")
            {
                game.Type = "Автомат";
            }
            DataBase _database = new();
            List<string> result = new List<string>();
            result.Add(game.Name);
            result.Add($"Тип: {game.Type}\nОписание: {game.Description}\nСтоимость сеанса: {game.Cost}$\nОценка: {game.Rate}\nДата создания: {game.CreateDate}");
            result.Add(_database.GetGameIdByName(game.Name).ToString());
            return result;
        }

        public enum SortingType
        {
            NAME,
            COST,
            DATE_OF_CREATION,
            RATE
        }
        public enum SortingMethod
        {
            INCREASING,
            DECREASING
        }
        public Game[] SortedDatabase(SortingType type, SortingMethod method, List<string> gameTypes)
        {
            DataBase _database = new();
            Game[] allGames = _database.ReadAllGames(); // Чтение базы данных

            // Фильтрация игр по заданным типам
            List<Game> filteredGames = new List<Game>();

            if (gameTypes.Contains("sport"))
            {
                filteredGames.AddRange(allGames.Where(game => game.Type.Equals("Sport", StringComparison.OrdinalIgnoreCase)));
            }
            if (gameTypes.Contains("race"))
            {
                filteredGames.AddRange(allGames.Where(game => game.Type.Equals("Race", StringComparison.OrdinalIgnoreCase)));
            }
            if (gameTypes.Contains("shoot"))
            {
                filteredGames.AddRange(allGames.Where(game => game.Type.Equals("Shoot", StringComparison.OrdinalIgnoreCase)));
            }
            if (gameTypes.Contains("automat"))
            {
                filteredGames.AddRange(allGames.Where(game => game.Type.Equals("Automat", StringComparison.OrdinalIgnoreCase)));
            }

            Game[] result = filteredGames.ToArray();
            switch (type)
            {
                case SortingType.NAME:
                    result = method == SortingMethod.INCREASING
                    ? result.OrderBy(x => x.Name).ToArray()
                    : result.OrderByDescending(x => x.Name).ToArray();
                    break;
                case SortingType.COST:
                    result = method == SortingMethod.INCREASING
                    ? result.OrderBy(x => x.Cost).ToArray()
                    : result.OrderByDescending(x => x.Cost).ToArray();
                    break;
                case SortingType.RATE:
                    result = method == SortingMethod.INCREASING
                    ? result.OrderBy(x => x.Rate).ToArray()
                    : result.OrderByDescending(x => x.Rate).ToArray();
                    break;
                case SortingType.DATE_OF_CREATION:
                    {
                        result = method == SortingMethod.INCREASING
                        ? result.OrderBy(x => ConvertCreateDateNotPount(x.CreateDate)).ToArray()
                        : result.OrderByDescending(x => ConvertCreateDateNotPount(x.CreateDate)).ToArray();
                        break;
                    }
            }
            return result;
        }

        public List<string> ConvertStringToList(string inputString)
        {
            List<string> resultList = new List<string>();
            string[] elements = inputString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string element in elements)
            {
                resultList.Add(element);
            }

            return resultList;
        }

        private string ConvertCreateDateNotPount(string date)
        {
            if (DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate.ToString("yyyyMMdd");
            }
            else if (DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDatee))
            {
                return parsedDatee.ToString("yyyyMMdd");
            }
            else
            {
                throw new ArgumentException("Invalid date format. Date should be in yyyy-MM-dd format.");
            }
        }

        public string ConvertDateFormat(string date)
        {
            if (DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate.ToString("dd.MM.yyyy");
            }
            else if (DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDatee))
            {
                return parsedDatee.ToString("yyyy-MM-dd");
            }
            else if (date == null)
            {
                return "06.03.1995";
            }
            else
            {
                throw new ArgumentException("Invalid date format. Date should be in yyyy-MM-dd format.");
            }
        }

        public string DescriptioString(string description)
        {
            int a, k = 1, um = 0;
            for (int i = 0; i < description.Length; i++)
            {
                if (i == 0)
                    a = 33;
                else
                    a = 40;
                for (int j = 0; j < a && (j < description.Length) && (i < description.Length) && (description[j] != ' ' || description[j] != '\n'); j++)
                {
                    if (j == (a - 1))
                    {
                        um = 37 == a ? 37 : a * k;
                        um = um < description.Length ? um : description.Length;
                        description = description.Insert(um, "\n");
                        i = i + j;
                        k++;
                    }
                }
            }
            return description;
        }

        #region Заготовки

        // public IActionResult ChartUpdateButton(string start_date, string end_date){
        //     List<List<List<string>>> diagram_data;
        //     // �����, �����, ��������, ��������
        //     List<string> giner = new List<string>(1) {"Sport"};// ('Sport'),('Shoot'),('Race'),('Automat');
        //     List<List<string>> game_genre = DataBase.FindGame(giner);
        //     return diagram_data;
        // }

        //[HttpPost("game-update-button")]
        // public IActionResult GameUpdateButton(List<string> gameTypes, DataBase.SortingType sortingType, bool isIncreasing)
        // {
        //     if (gameTypes == null || !gameTypes.Any() || gameTypes.Count > 4)
        //     {
        //         return BadRequest("Invalid game types list.");
        //     }

        //     DataBase _database = new DataBase();
        //     var sortingMethod = isIncreasing ? DataBase.SortingMethod.INCREASING : DataBase.SortingMethod.DECREASING;

        //     if (!gameTypes.All(type => new List<string> { "sport", "race", "shoot", "automat" }.FirstOrDefault().Equals(type, StringComparison.OrdinalIgnoreCase)))
        //     {
        //         return BadRequest("Invalid game type names.");
        //     }

        //     try
        //     {
        //         var sortedGames = _database.FindGame(gameTypes).Select(gamesOfType => _database.SortedDatabase(sortingType, sortingMethod)).ToList();
        //         return Ok(sortedGames);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }

        #endregion
    }
}

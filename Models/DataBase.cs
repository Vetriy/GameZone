using Npgsql;
using Npgsql.Internal.Postgres;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace GameZone.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public double Rate { get; set; }
        public string CreateDate { get; set; }

        public Game(string name_, string type_, string description_, int cost_, double rate_, string createdate_, int id_ = 0)
        {
            Name = name_;
            Type = type_;
            Description = description_;
            Cost = cost_;
            Rate = rate_;
            CreateDate = createdate_;
            Id = id_;
        }
    }

    public class Dates
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Count { get; set; }
        public int Game_Id { get; set; }

        public Dates(string date, int count, int game_id, int id = 0)
        {
            Date = date;
            Count = count;
            Game_Id = game_id;
            Id = id;
        }
    }

    public class DataBase
    {
        private string connectionString = @"
            Host=localhost;
            Username=postgres;
            Password=1234;
            Database=GameZone;";

        public void CreateTables()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Types (
                            Id SERIAL PRIMARY KEY,
                            Type VARCHAR(100) NOT NULL UNIQUE
                        );

                        CREATE TABLE IF NOT EXISTS Descriptions (
                            Id SERIAL PRIMARY KEY,
                            Description TEXT NOT NULL
                        );

                        CREATE TABLE IF NOT EXISTS Costs (
                            Id SERIAL PRIMARY KEY,
                            Cost INT NOT NULL
                        );

                        CREATE TABLE IF NOT EXISTS Rates (
                            Id SERIAL PRIMARY KEY,
                            Rate DOUBLE PRECISION NOT NULL
                        );

                        CREATE TABLE IF NOT EXISTS Names (
                            Id SERIAL PRIMARY KEY,
                            Name VARCHAR(100) NOT NULL UNIQUE,
                            Type_Id INT NOT NULL REFERENCES Types(Id),
                            Description_Id INT NOT NULL REFERENCES Descriptions(Id),
                            Cost_Id INT NOT NULL REFERENCES Costs(Id),
                            Rate_Id INT NOT NULL REFERENCES Rates(Id)
                        );

                        CREATE TABLE IF NOT EXISTS CreateDates (
                            Id SERIAL PRIMARY KEY,
                            Date VARCHAR(100) NOT NULL,
                            Game_Id INT NOT NULL REFERENCES Names(Id)
                        );

                        CREATE TABLE IF NOT EXISTS Dates (
                            Id SERIAL PRIMARY KEY,
                            Date VARCHAR(100) NOT NULL,
                            Game_Id INT NOT NULL REFERENCES Names(Id)
                        );

                        CREATE TABLE IF NOT EXISTS Counts (
                            Id SERIAL PRIMARY KEY,
                            Count INT NOT NULL,
                            Date_Id INT NOT NULL REFERENCES Dates(Id)
                        );
                    ";

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"
                        INSERT INTO Types (Type) VALUES ('Shoot')
                        ON CONFLICT (Type) DO NOTHING;
                        
                        INSERT INTO Types (Type) VALUES ('Race')
                        ON CONFLICT (Type) DO NOTHING;
                        
                        INSERT INTO Types (Type) VALUES ('Sport')
                        ON CONFLICT (Type) DO NOTHING;
                        
                        INSERT INTO Types (Type) VALUES ('Automat')
                        ON CONFLICT (Type) DO NOTHING;
                    ";

                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Tables created and default types inserted successfully");
            }
        }

        public void FillTables()
        {
            DataBase b = new DataBase();

            Game newGame_1 = new Game("Лазерный Штурм", "Shoot", "Возьмите лазерный бластер и отправляйтесь в космическое сражение против инопланетян! Защищайте свою базу, уничтожайте врагов и собирайте бонусы для улучшения оружия.", 860, 7.2, "23.07.2023");
            Game newGame_2 = new Game("Космический Захватчик", "Shoot", "Станьте пилотом звездного истребителя и отправляйтесь на миссию по защите галактики! Маневрируйте среди астероидов, сбивайте вражеские корабли и получайте награды за смелость.", 410, 6.4, "15.04.2023");
            Game newGame_3 = new Game("Формула Фантазий", "Race", "Вступайте в волшебные гонки по сказочным трассам! Обгоняйте соперников, собирайте магические артефакты и используйте волшебные силы для победы.", 1190, 9.8, "30.12.2023");
            Game newGame_4 = new Game("Дрифт Кинг", "Race", "Превратите каждую гонку в шоу дрифта! Выполняйте крутые маневры, зарабатывайте очки за стиль и точность, и станьте королем дрифта на захватывающих городских трассах.", 320, 5.0, "04.02.2023");
            Game newGame_5 = new Game("Счастливая Лапка", "Automat", "Управляйте механической лапкой и ловите забавные игрушки! Проявите ловкость и точность, чтобы поймать самых редких и ценных призов.", 850, 5.9, "11.10.2023");
            Game newGame_6 = new Game("Коготь Удачи", "Automat", "Испытайте свою удачу с механической лапкой! Захватывайте ценные призы, нацеливайтесь на самые желанные и станьте мастером точного захвата.", 740, 2.3, "28.05.2023");
            Game newGame_7 = new Game("Виртуальный Волейбол", "Sport", "Примите участие в захватывающем волейбольном матче! Отбивайте мячи, делайте впечатляющие подачи и приведите свою команду к победе.", 970, 6.1, "25.03.2023");
            Game newGame_8 = new Game("Мега-Гол", "Sport", "Испытайте себя в футбольном симуляторе! Бейте пенальти, забивайте мячи в ворота, демонстрируйте мастерство и зарабатывайте очки за точные удары.", 530, 9.1, "16.04.2023");

            b.AddGame(newGame_1);
            b.AddGame(newGame_2);
            b.AddGame(newGame_3);
            b.AddGame(newGame_4);
            b.AddGame(newGame_5);
            b.AddGame(newGame_6);
            b.AddGame(newGame_7);
            b.AddGame(newGame_8);

            Dates d_1 = new Dates("07.11.2018", 3, b.GetGameIdByName("Лазерный Штурм"));
            Dates d_2 = new Dates("20.04.2019", 2, b.GetGameIdByName("Счастливая Лапка"));
            Dates d_3 = new Dates("09.08.2018", 4, b.GetGameIdByName("Формула Фантазий"));
            Dates d_4 = new Dates("30.12.2020", 5, b.GetGameIdByName("Лазерный Штурм"));
            Dates d_5 = new Dates("14.06.2019", 6, b.GetGameIdByName("Лазерный Штурм"));
            Dates d_6 = new Dates("03.10.2021", 3, b.GetGameIdByName("Мега-Гол"));
            Dates d_7 = new Dates("25.03.2019", 2, b.GetGameIdByName("Счастливая Лапка"));
            Dates d_8 = new Dates("18.09.2018", 5, b.GetGameIdByName("Лазерный Штурм"));
            Dates d_9 = new Dates("07.07.2020", 2, b.GetGameIdByName("Космический Захватчик"));
            Dates d_10 = new Dates("12.11.2019", 8, b.GetGameIdByName("Коготь Удачи"));
            Dates d_11 = new Dates("29.05.2021", 4, b.GetGameIdByName("Коготь Удачи"));
            Dates d_12 = new Dates("16.01.2020", 7, b.GetGameIdByName("Космический Захватчик"));
            Dates d_13 = new Dates("05.08.2018", 3, b.GetGameIdByName("Виртуальный Волейбол"));


            b.CreateBulkDates(b.GetGameIdByName("Лазерный Штурм"));
            b.CreateBulkDates(b.GetGameIdByName("Космический Захватчик"));
            b.CreateBulkDates(b.GetGameIdByName("Формула Фантазий"));
            b.CreateBulkDates(b.GetGameIdByName("Дрифт Кинг"));
            b.CreateBulkDates(b.GetGameIdByName("Счастливая Лапка"));
            b.CreateBulkDates(b.GetGameIdByName("Коготь Удачи"));
            b.CreateBulkDates(b.GetGameIdByName("Виртуальный Волейбол"));
            b.CreateBulkDates(b.GetGameIdByName("Мега-Гол"));

        }

        public bool AddGame(Game game)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Ensure the type is valid
                    cmd.CommandText = "SELECT Id FROM Types WHERE Type = @Type";
                    cmd.Parameters.AddWithValue("Type", game.Type);
                    var typeIdObj = cmd.ExecuteScalar();

                    if (typeIdObj == null)
                    {
                        throw new ArgumentException("Invalid game type.");
                    }

                    int typeId = (int)typeIdObj;

                    // Insert into Descriptions table
                    cmd.CommandText = "INSERT INTO Descriptions (Description) VALUES (@Description) RETURNING Id";
                    cmd.Parameters.AddWithValue("Description", game.Description);
                    int descriptionId = (int)cmd.ExecuteScalar();

                    // Insert into Costs table
                    cmd.CommandText = "INSERT INTO Costs (Cost) VALUES (@Cost) RETURNING Id";
                    cmd.Parameters.AddWithValue("Cost", game.Cost);
                    int costId = (int)cmd.ExecuteScalar();

                    // Insert into Rates table
                    cmd.CommandText = "INSERT INTO Rates (Rate) VALUES (@Rate) RETURNING Id";
                    cmd.Parameters.AddWithValue("Rate", game.Rate);
                    int rateId = (int)cmd.ExecuteScalar();

                    // Insert into Names table
                    try
                    {
                        cmd.CommandText = @"
                            INSERT INTO Names (Name, Type_Id, Description_Id, Cost_Id, Rate_Id) 
                            VALUES (@Name, @Type_Id, @Description_Id, @Cost_Id, @Rate_Id) RETURNING Id";
                        cmd.Parameters.AddWithValue("Name", game.Name);
                        cmd.Parameters.AddWithValue("Type_Id", typeId);
                        cmd.Parameters.AddWithValue("Description_Id", descriptionId);
                        cmd.Parameters.AddWithValue("Cost_Id", costId);
                        cmd.Parameters.AddWithValue("Rate_Id", rateId);
                        int nameId = (int)cmd.ExecuteScalar();

                        // Insert into CreateDates table
                        cmd.CommandText = @"
                            INSERT INTO CreateDates (Date, Game_Id) 
                            VALUES (@Date, @Game_Id) RETURNING Id";
                        cmd.Parameters.AddWithValue("Date", game.CreateDate);
                        cmd.Parameters.AddWithValue("Game_Id", nameId);
                        int dateId = (int)cmd.ExecuteScalar();

                        Console.WriteLine("Game added successfully");
                    }
                    catch (PostgresException ex) when (ex.SqlState == "23505") // Unique violation
                    {
                        Console.WriteLine("A game with this name already exists.");
                        return false;
                    }
                }
                return true;
            }
        }


        public void AddDateIfNotExists(Dates date)
        {
            if (DateExistsForGame(date.Date, date.Game_Id))
            {
                Console.WriteLine("Date already exists for this game.");
            }
            else
            {
                AddDate(date);
                Console.WriteLine("Date added successfully.");
            }
        }

        public bool DateExistsForGame(string date, int gameId)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = @"
                SELECT COUNT(*) 
                FROM Dates 
                WHERE Date = @Date AND Game_Id = @GameId";
                    cmd.Parameters.AddWithValue("Date", date);
                    cmd.Parameters.AddWithValue("GameId", gameId);

                    // Приведение типа данных
                    long count = (long)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }
        public void AddDate(Dates date)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Ensure the game ID is valid
                    cmd.CommandText = "SELECT Id FROM Names WHERE Id = @Game_Id";
                    cmd.Parameters.AddWithValue("Game_Id", date.Game_Id);
                    var gameIdObj = cmd.ExecuteScalar();

                    if (gameIdObj == null)
                    {
                        throw new ArgumentException("Invalid game ID.");
                    }

                    // Insert into Dates table
                    cmd.CommandText = "INSERT INTO Dates (Date, Game_Id) VALUES (@Date, @Game_Id) RETURNING Id";
                    cmd.Parameters.AddWithValue("Date", date.Date);
                    int dateId = (int)cmd.ExecuteScalar();

                    // Insert into Counts table
                    cmd.CommandText = "INSERT INTO Counts (Count, Date_Id) VALUES (@Count, @Date_Id)";
                    cmd.Parameters.AddWithValue("Count", date.Count);
                    cmd.Parameters.AddWithValue("Date_Id", dateId);
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Date and count added successfully");
                }
            }
        }

        public void CreateBulkDates(int gameId)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Получаем дату создания игры
                    string createDateStr = GetGameCreationDate(gameId);
                    if (!DateTime.TryParseExact(createDateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime createDate))
                    {
                        throw new ArgumentException("Invalid creation date format in the database.");
                    }

                    DateTime today = DateTime.Today;
                    Random random = new Random();

                    for (DateTime date = createDate; date <= today; date = date.AddDays(1))
                    {
                        string formattedDate = date.ToString("dd.MM.yyyy");
                        int count = random.Next(1, 101); // Генерация случайного count от 1 до 100

                        Dates newDate = new Dates(formattedDate, count, gameId);
                        AddDateIfNotExists(newDate);
                    }
                }
            }

            Console.WriteLine("Bulk dates created successfully.");
        }


        public int GetGameIdByName(string name)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT Id FROM Names WHERE Name = @Name";
                    cmd.Parameters.AddWithValue("Name", name);
                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        throw new ArgumentException("Game not found.");
                    }

                    return (int)result;
                }
            }
        }

        public Game GetGameById(int id)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        SELECT 
                            n.Id, n.Name, t.Type, d.Description, c.Cost, r.Rate, cd.Date
                        FROM 
                            Names n
                        JOIN 
                            Types t ON n.Type_Id = t.Id
                        JOIN 
                            Descriptions d ON n.Description_Id = d.Id
                        JOIN 
                            Costs c ON n.Cost_Id = c.Id
                        JOIN 
                            Rates r ON n.Rate_Id = r.Id
                        JOIN 
                            CreateDates cd ON n.Id = cd.Game_Id
                        WHERE 
                            n.Id = @Id";
                    cmd.Parameters.AddWithValue("Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Game(
                                reader["Name"].ToString(),
                                reader["Type"].ToString(),
                                reader["Description"].ToString(),
                                (int)reader["Cost"],
                                (double)reader["Rate"],
                                reader["Date"].ToString(),
                                (int)reader["Id"]
                            );
                        }
                        else
                        {
                            throw new ArgumentException("Game not found.");
                        }
                    }
                }
            }
        }

        public Game[] ReadAllGames()
        {
            List<Game> games = new List<Game>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT id FROM Names;", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            games.Add(GetGameById(id));
                        }
                    }
                }
            }
            return games.ToArray();
        }

        public string GetProfit(int gameId, string date)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Получаем количество count по указанной дате и ID игры
                    cmd.CommandText = @"
                    SELECT co.Count
                    FROM Dates d    
                    JOIN Counts co ON d.Id = co.Date_Id
                    WHERE d.Game_Id = @GameId AND d.Date = @Date";
                    cmd.Parameters.AddWithValue("GameId", gameId);
                    cmd.Parameters.AddWithValue("Date", date);
                    var count = (int?)cmd.ExecuteScalar();

                    if (count == null)
                    {
                        return "0";
                    }

                    // Получаем стоимость cost для игры по указанному ID
                    cmd.CommandText = @"
                    SELECT c.Cost
                    FROM Costs c
                    JOIN Names n ON c.Id = n.Cost_Id
                    WHERE n.Id = @GameId";
                    var cost = (int?)cmd.ExecuteScalar();

                    if (cost == null)
                    {
                        return "Cost not found";
                    }

                    // Рассчитываем и возвращаем произведение count и cost
                    int profit = count.Value * cost.Value;
                    return profit.ToString();
                }
            }
        }

        public string GetGameCreationDate(int gameId)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT Date FROM CreateDates WHERE Game_Id = @Game_Id";
                    cmd.Parameters.AddWithValue("Game_Id", gameId);
                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        public void DeleteGameById(int game_id)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Удаление из Counts
                            cmd.CommandText = @"
                    DELETE FROM Counts
                    WHERE Date_Id IN (
                        SELECT Id FROM Dates WHERE Game_Id = @GameId
                    )";
                            cmd.Parameters.AddWithValue("GameId", game_id);
                            cmd.ExecuteNonQuery();

                            // Удаление из Dates
                            cmd.CommandText = @"
                    DELETE FROM Dates
                    WHERE Game_Id = @GameId";
                            cmd.ExecuteNonQuery();

                            // Удаление из CreateDates
                            cmd.CommandText = @"
                    DELETE FROM CreateDates
                    WHERE Game_Id = @GameId";
                            cmd.ExecuteNonQuery();

                            // Удаление из Names (основная запись игры)
                            cmd.CommandText = @"
                    DELETE FROM Names
                    WHERE Id = @GameId";
                            cmd.ExecuteNonQuery();

                            // Удаление из Rates
                            cmd.CommandText = @"
                    DELETE FROM Rates
                    WHERE Id = @GameId";
                            cmd.ExecuteNonQuery();

                            // Удаление из Costs
                            cmd.CommandText = @"
                    DELETE FROM Costs
                    WHERE Id = @GameId";
                            cmd.ExecuteNonQuery();

                            // Удаление из Descriptions
                            cmd.CommandText = @"
                    DELETE FROM Descriptions
                    WHERE Id = @GameId";
                            cmd.ExecuteNonQuery();

                            // Подтверждение транзакции
                            transaction.Commit();
                            Console.WriteLine("Game and related records deleted successfully");
                        }
                        catch (Exception ex)
                        {
                            // Откат транзакции в случае ошибки
                            transaction.Rollback();
                            Console.WriteLine("Error occurred while deleting the game: " + ex.Message);
                        }
                    }
                }
            }
        }

        public List<List<string>> GetGameCountsByType(string startDate, string endDate, string typeValue)
        {
            Dictionary<int, List<int>> values = new Dictionary<int, List<int>>();
            int thisIndex = 0;

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand($"SELECT * FROM types", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(1) == typeValue)
                            {
                                thisIndex = reader.GetInt32(0);
                                break;
                            }
                        }
                    }
                }

                using (var cmd = new NpgsqlCommand("SELECT * FROM names", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetInt32(2) == thisIndex)
                            {
                                values.Add(reader.GetInt32(0), new List<int>() { });
                            }
                        }
                    }
                }

                using (var cmd = new NpgsqlCommand("SELECT * FROM dates", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (values.ContainsKey(reader.GetInt32(2)) &&
                                DateComparison(reader.GetString(1), startDate) >= 0 &&
                                DateComparison(reader.GetString(1), endDate) <= 0
                                )
                            {
                                values[reader.GetInt32(2)].Add(reader.GetInt32(0));
                            }
                        }
                    }
                }

                foreach (var value in values)
                {
                    for (int i = 0; i < value.Value.Count; i++)
                    {
                        using (var cmd = new NpgsqlCommand($"SELECT count FROM counts WHERE date_id = {value.Value[i]}", conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    value.Value[i] = reader.GetInt32(0);
                                }
                            }
                        }
                    }
                }

                //foreach (var value in values)
                //{
                //    using (var cmd = new NpgsqlCommand($"SELECT cost FROM costs WHERE id = {value.Key}", conn))
                //    {
                //        using (var reader = cmd.ExecuteReader())
                //        {
                //            if (reader.Read())
                //            {
                //                for (int i = 0; i < value.Value.Count; i++)
                //                {
                //                    value.Value[i] *= reader.GetInt32(0);
                //                }
                //            }
                //        }
                //    }
                //}
            }

            List<List<string>> result = new List<List<string>>();

            foreach (var value in values)
            {
                int sum = 0;

                for (int i = 0; i < value.Value.Count; i++)
                {
                    sum += value.Value[i];
                }

                if (sum != 0)
                {
                    result.Add(new List<string>() { value.Key.ToString(), sum.ToString() });
                }
            }

            return result;
        }

        public int DateComparison(string date1, string date2)
        {
            string[] date1Array = date1.Split('.');
            string[] date2Array = date2.Split('.');

            if (date1Array[2] != date2Array[2])
            {
                return int.Parse(date1Array[2]) - int.Parse(date2Array[2]);
            }
            else if (date1Array[1] != date2Array[1])
            {
                return int.Parse(date1Array[1]) - int.Parse(date2Array[1]);
            }
            else
            {
                return int.Parse(date1Array[0]) - int.Parse(date2Array[0]);
            }
        }

        //public List<List<string>> GetGameCountsByType(string startDate, string endDate, string type)
        //{
        //    var result = new List<List<string>>();

        //    using (var conn = new NpgsqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand())
        //        {
        //            cmd.Connection = conn;

        //            cmd.CommandText = @"
        //            SELECT n.Id, COALESCE(SUM(c.Count), 0) AS TotalCount
        //            FROM Names n
        //            JOIN Types t ON n.Type_Id = t.Id
        //            LEFT JOIN Dates d ON n.Id = d.Game_Id
        //            LEFT JOIN Counts c ON d.Id = c.Date_Id
        //            WHERE t.Type = @Type AND d.Date >= @StartDate AND d.Date <= @EndDate
        //            GROUP BY n.Id
        //            HAVING COALESCE(SUM(c.Count), 0) > 0
        //            ORDER BY n.Id";

        //            cmd.Parameters.AddWithValue("StartDate", startDate);
        //            cmd.Parameters.AddWithValue("EndDate", endDate);
        //            cmd.Parameters.AddWithValue("Type", type);

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    int gameId = reader.GetInt32(0);
        //                    int totalCount = reader.GetInt32(1);

        //                    var gameInfo = new List<string> { gameId.ToString(), totalCount.ToString() };
        //                    result.Add(gameInfo);
        //                }
        //            }
        //        }
        //    }

        //    return result;
        //}

        public string GetEarliestCreationDate()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "SELECT MIN(TO_DATE(Date, 'DD.MM.YYYY')) FROM CreateDates";

                    var result = cmd.ExecuteScalar();
                    return result != null && result != DBNull.Value ? Convert.ToDateTime(result).ToString("dd.MM.yyyy") : null;
                }
            }
        }

        public string GetLatestDate()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "SELECT MAX(Date) FROM Dates";

                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        public void ChangeDate(int dateId, string newDate, int count)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        // Начало транзакции
                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                // Обновление даты в таблице Dates
                                cmd.CommandText = @"
                                UPDATE Dates
                                SET Date = @NewDate
                                WHERE Id = @DateId;
                                ";
                                cmd.Parameters.AddWithValue("NewDate", newDate);
                                cmd.Parameters.AddWithValue("DateId", dateId);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();

                                // Обновление Count в таблице Counts
                                cmd.CommandText = @"
                                UPDATE Counts
                                SET Count = @Count
                                WHERE Date_Id = @DateId;
                                ";
                                cmd.Parameters.AddWithValue("Count", count);
                                cmd.Parameters.AddWithValue("DateId", dateId);
                                cmd.ExecuteNonQuery();

                                // Подтверждение транзакции
                                transaction.Commit();
                                Console.WriteLine("Date and count updated successfully");
                            }
                            catch (Exception ex)
                            {
                                // Откат транзакции в случае ошибки
                                transaction.Rollback();
                                Console.WriteLine("Error occurred while updating the date and count: " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                throw;
            }
        }

        public Dates GetDate(int gameId, string date)
        {
            Dates dateInfo = null;

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // SQL-запрос для получения данных о дате и счётчике по gameId и date
                    cmd.CommandText = @"
                    SELECT d.Id, d.Date, c.Count, d.Game_Id
                    FROM Dates d
                    LEFT JOIN Counts c ON d.Id = c.Date_Id
                    WHERE d.Game_Id = @GameId AND d.Date = @Date
                    LIMIT 1";

                    cmd.Parameters.AddWithValue("GameId", gameId);
                    cmd.Parameters.AddWithValue("Date", date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dateInfo = new Dates(
                            date: reader.GetString(reader.GetOrdinal("Date")),
                            count: reader.IsDBNull(reader.GetOrdinal("Count")) ? 0 : reader.GetInt32(reader.GetOrdinal("Count")),
                            game_id: reader.GetInt32(reader.GetOrdinal("Game_Id")),
                            id: reader.GetInt32(reader.GetOrdinal("Id"))
                            );
                        }
                    }
                }
            }

            return dateInfo;
        }

        public bool UpdateGame(Game updatedGame)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        // Получение идентификаторов
                        cmd.CommandText = @"
                SELECT
                    (SELECT Id FROM Types WHERE Type = @Type LIMIT 1) AS TypeId,
                    (SELECT Id FROM Descriptions WHERE Description = @Description LIMIT 1) AS DescriptionId,
                    (SELECT Id FROM Costs WHERE Cost = @Cost LIMIT 1) AS CostId,
                    (SELECT Id FROM Rates WHERE Rate = @Rate LIMIT 1) AS RateId;
                ";

                        cmd.Parameters.AddWithValue("Type", updatedGame.Type);
                        cmd.Parameters.AddWithValue("Description", updatedGame.Description);
                        cmd.Parameters.AddWithValue("Cost", updatedGame.Cost);
                        cmd.Parameters.AddWithValue("Rate", updatedGame.Rate);

                        int? typeId = null;
                        int? descriptionId = null;
                        int? costId = null;
                        int? rateId = null;

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                typeId = reader["TypeId"] as int?;
                                descriptionId = reader["DescriptionId"] as int?;
                                costId = reader["CostId"] as int?;
                                rateId = reader["RateId"] as int?;
                            }
                        }

                        // Очистка параметров перед следующим использованием команды
                        cmd.Parameters.Clear();

                        // Обновление или добавление записей в зависимые таблицы
                        if (descriptionId == null)
                        {
                            cmd.CommandText = "INSERT INTO Descriptions (Description) VALUES (@Description) RETURNING Id";
                            cmd.Parameters.AddWithValue("Description", updatedGame.Description);
                            descriptionId = (int)cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                        }

                        if (costId == null)
                        {
                            cmd.CommandText = "INSERT INTO Costs (Cost) VALUES (@Cost) RETURNING Id";
                            cmd.Parameters.AddWithValue("Cost", updatedGame.Cost);
                            costId = (int)cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                        }

                        if (rateId == null)
                        {
                            cmd.CommandText = "INSERT INTO Rates (Rate) VALUES (@Rate) RETURNING Id";
                            cmd.Parameters.AddWithValue("Rate", updatedGame.Rate);
                            rateId = (int)cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                        }

                        // Обновление основной записи в таблице Names
                        cmd.CommandText = @"
                UPDATE Names
                SET
                    Name = @Name,
                    Type_Id = @TypeId,
                    Description_Id = @DescriptionId,
                    Cost_Id = @CostId,
                    Rate_Id = @RateId
                WHERE Id = @Id";
                        cmd.Parameters.AddWithValue("Name", updatedGame.Name);
                        cmd.Parameters.AddWithValue("TypeId", typeId);
                        cmd.Parameters.AddWithValue("DescriptionId", descriptionId);
                        cmd.Parameters.AddWithValue("CostId", costId);
                        cmd.Parameters.AddWithValue("RateId", rateId);
                        cmd.Parameters.AddWithValue("Id", updatedGame.Id);

                        cmd.ExecuteNonQuery();

                        // Обновление записи в таблице CreateDates
                        cmd.CommandText = @"
                UPDATE CreateDates
                SET Date = @CreateDate
                WHERE Game_Id = @Id";
                        cmd.Parameters.AddWithValue("CreateDate", updatedGame.CreateDate);

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Game updated successfully");
                        return true;
                    }
                }
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23505")
            {
                // Unique constraint violation
                return false;
            }
            catch (Exception ex)
            {
                // Handle other exceptions if necessary
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
using rem;
using helper;
using print;
using Npgsql;

namespace commands {
    public class Reminder {
        public int reminder_id { get; set; }
        public string title { get; set; } = string.Empty;
        public DateTime date { get; set; }
        public bool completed { get; set; }

        public Reminder (int reminder_id, string title, DateTime date, bool completed) {
            this.reminder_id = reminder_id;
            this.title = title;
            this.date = date;
            this.completed = completed;
        }

    }

    public static class Commands {
        public static void Init() {
            
        }

        public static async void Add() {
            DateTime dt = DateTime.MinValue;
            string title = "";
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Remeber: ");
            Console.ResetColor();

            string? reminder = Console.ReadLine();
            if (reminder == "") return;


            if (reminder!.Contains("on ")) {
                var rem_split = reminder.Split("on ");
                
                title = rem_split[0];
                dt = add.Add.FindDate(rem_split[1]);

                if (dt == DateTime.MinValue) {
                    C.Error("Invalid Date");
                    return;
                }
            } else {
                title = reminder;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("When: ");
                Console.ResetColor();

                string? date = Console.ReadLine();
                if (date == "") {
                    C.Error("You must enter a date (Press enter again to cancel add).");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("When: ");
                    Console.ResetColor();

                    date = Console.ReadLine();
                    if (date == "") return;
                }

                dt = add.Add.FindDate(date!);

                if (dt == DateTime.MinValue) {
                    C.Error("Invalid Date");
                    return;
                }
            }
            
            try {
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                // Insert data
                cmd.CommandText = $"INSERT INTO reminders (user_id, title, date) VALUES (@user_id, @title, @date)";
                cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Program.user_id;
                cmd.Parameters.Add("@title", NpgsqlTypes.NpgsqlDbType.Varchar).Value = title;
                cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
                await cmd.ExecuteNonQueryAsync();

                con.Close();

                C.Success("Successfully added reminder to list.");
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

        public static async void Complete(string arg1, int arg2) {
            Dictionary<string, string> day_lookup = new Dictionary<string, string>(){
                {"past", "0"},
                {"sunday", "1"},
                {"monday", "2"},
                {"tuesday", "3"},
                {"wednesday", "4"},
                {"thrusday", "5"},
                {"friday", "6"},
                {"saturday", "7"},
                {"future", "8"},
            };

            arg1 = arg1.ToLower();
    
            if (arg1 == "today" || arg1 == "sunday" || arg1 == "monday"
                || arg1 == "tuesday" || arg1 == "wednesday" || arg1 == "thursday"
                || arg1 == "friday" || arg1 == "saturday") {
                    if (arg1 == "today") {
                        arg1 = day_lookup[DateTime.Today.DayOfWeek.ToString().ToLower()];
                    } else {
                        arg1 = day_lookup[arg1];
                    }
                }

            int beginning = Helper.FindBeginningOfWeek();

            // get date from arg1
            DateTime dt = DateTime.Today.AddDays(beginning + (int.Parse(arg1) - 1));
            List<Reminder> reminders = [];

            var con = new NpgsqlConnection(
            connectionString: Program.ConnectionString);
            con.Open();
            
            using (var cmd = new NpgsqlCommand()) {
                try {
                    cmd.Connection = con;
                
                    if (arg1 == "0") {
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date <= @date";
                    } else if (arg1 == "8") {
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date >= @date";
                    } else {
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date = @date";
                    }

                    cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Program.user_id;
                    cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    
                    while (await reader.ReadAsync()) {
                        var r = new Reminder(
                            (int)reader[0],
                            (string)reader[2] ?? string.Empty,
                            (DateTime)reader[3],
                            (bool)reader[4]
                        );

                        reminders.Add(r);
                    }

                    } catch (Exception e) {
                        C.Error(e.Message);
                    }
            }

            con.Close();

            reminders.Sort((x, y) => DateTime.Compare(x.date, y.date));
            reminders = reminders.OrderBy(x => x.completed).ToList();

            con = new NpgsqlConnection(
            connectionString: Program.ConnectionString);
            con.Open();
            using (var cmd = new NpgsqlCommand()) {
                try {
                    cmd.Connection = con;
                    
                    cmd.CommandText = $"UPDATE reminders SET completed = NOT completed WHERE user_id = @user_id AND reminder_id = @reminder_id";
                    cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Program.user_id;
                    cmd.Parameters.Add("@reminder_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = reminders[arg2-1].reminder_id;
                    await cmd.ExecuteNonQueryAsync();

                } catch (Exception e) {
                    C.Error(e.Message);
                }
            }

            string title = reminders[arg2-1].title.Trim();
            C.Success($"Reminder: \"{title}\" was marked complete!");
            con.Close();
        }
    }
}
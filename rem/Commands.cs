using rem;
using print;
using Npgsql;

namespace commands {
    public class Reminder {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool Completed { get; set; }
        public int Work_on_count { get; set; }
        public bool Work_on { get; set; }
        public int Work_on_reminder { get; set; }

        public Reminder (int id, string title, DateTime date, bool completed, int work_on_count, bool work_on, int work_on_reminder) {
            Id = id;
            Title = title;
            Date = date;
            Completed = completed;
            Work_on_count = work_on_count;
            Work_on = work_on;
            Work_on_reminder = work_on_reminder;
        }

        // Sort by date
        public static int CompareDates(Reminder x, Reminder y) {
            return DateTime.Compare(x.Date, y.Date);
        }

        // Sort false then true
        public static int CompareCompleted(Reminder x, Reminder y) {
            return x.Completed.CompareTo(y.Completed);
        }
    }

    public static class Commands {
        public static void Init() {
            List<string> lines = new List<string> {};
            string? res = null;
            
            while (res == null) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Do you already have an account? [Y/N]: ");
                Console.ResetColor();
                res = Console.ReadLine();
            }

            C.WriteBlue(res);
            C.WriteBlue(res.ToUpper());
            
            if (res.ToUpper() == "Y") {
                // login
                lines.Add("login");
            } else if (res.ToUpper() == "N") {
                // sign up
                lines.Add("Sign up");
            } else {
                C.Error("Invalid Character Entered");
                return;
            }

            using (StreamWriter outputFile = new StreamWriter("./.env.local")) {
                foreach (string line in lines)
                outputFile.WriteLine(line);
            }
        }

        public static async Task Add(String[] args) {
            DateTime dt;
            string title;
            string? reminder = "";
            bool flag = false;
            List<DateTime> work_on_dates = [];
            string? dates = "empty";
            int? work_on_id;
            
            if (args.Length == 1) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Remember: ");
                Console.ResetColor();
                
                reminder = Console.ReadLine();
                if (reminder == "") return;
            } else if (args.Length == 2) {
                if (args[1] == "-w") {
                    flag = true;

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("Remember: ");
                    Console.ResetColor();
                
                    reminder = Console.ReadLine();
                    if (reminder == "") return;
                } else reminder = args[1];
            } else if (args.Length == 3) {
                reminder = args[2];
                if (args[1] == "-w") flag = true;
                else {
                    C.Error("Unknown flag.");
                    return;
                }
            } else {
                C.Error("Usage: rem add [flag] [\"reminder\"]");
                return;
            }

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

            if (flag == true) {
                dates = add.Add.PrintWorkOnDates();
                if (dates == "") return;
            }

            var dates_split = dates?.Split(",");
            if (dates_split?.Length > 3) {
                C.Error("Too many days entered (only 3 days may be used). Please try again.");
                
                dates = add.Add.PrintWorkOnDates();
                if (dates == "") return;
            }

            for (int i = 0; i < dates_split?.Length; i++) {
                work_on_dates.Add(add.Add.FindDate(dates_split[i].Trim()));
            }
            
            try {
                using (var con = new NpgsqlConnection(connectionString: Program.ConnectionString)) {
                    con.Open();

                    if (flag == true) {
                        // add to work on days table
                        using (var cmd = new NpgsqlCommand()) {
                            cmd.Connection = con;

                            // Insert data
                            if (work_on_dates.Count == 1) {
                                cmd.CommandText = $"INSERT INTO work_on_dates (reminder_id, date1) VALUES (@reminder_id, @date1) RETURNING work_on_id";
                                cmd.Parameters.Add("@date1", NpgsqlTypes.NpgsqlDbType.Date).Value = work_on_dates[0];

                            } else if (work_on_dates.Count == 2) {
                                cmd.CommandText = $"INSERT INTO work_on_dates (reminder_id, date1, date2) VALUES (@reminder_id, @date1, @date2) RETURNING work_on_id";
                                cmd.Parameters.Add("@date1", NpgsqlTypes.NpgsqlDbType.Date).Value = work_on_dates[0];
                                cmd.Parameters.Add("@date2", NpgsqlTypes.NpgsqlDbType.Date).Value = work_on_dates[1];

                            } else if (work_on_dates.Count == 3) {
                                cmd.CommandText = $"INSERT INTO work_on_dates (reminder_id, date1, date2, date3) VALUES (@reminder_id, @date1, @date2, @date3) RETURNING work_on_id";
                                cmd.Parameters.Add("@date1", NpgsqlTypes.NpgsqlDbType.Date).Value = work_on_dates[0];
                                cmd.Parameters.Add("@date2", NpgsqlTypes.NpgsqlDbType.Date).Value = work_on_dates[1];
                                cmd.Parameters.Add("@date3", NpgsqlTypes.NpgsqlDbType.Date).Value = work_on_dates[2];

                            } else C.Error("Error");

                            var result = await cmd.ExecuteScalarAsync();
                            work_on_id = int.Parse(result?.ToString()!);
                            
                        }
                    }

                    // add to reminders table
                    using (var cmd = new NpgsqlCommand()) {
                        cmd.Connection = con;

                        // Insert data
                        cmd.CommandText = $"INSERT INTO reminders (user_id, title, date, work_on_id) VALUES (@user_id, @title, @date, @work_on_id)";
                        cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Program.user_id;
                        cmd.Parameters.Add("@title", NpgsqlTypes.NpgsqlDbType.Varchar).Value = title.Trim();
                        cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
                        cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
                        await cmd.ExecuteNonQueryAsync();
                    }                    
                }

                // check if adding to past list
                if (DateTime.Compare(DateTime.Today.AddDays(-1*(int)DateTime.Today.DayOfWeek), dt) > 0) C.Success($"Successfully added \"{title.Trim()}\".");
                else {
                    string day = "";
                    if (DateTime.Compare(DateTime.Today, dt) == 0) {
                        day = "today";
                    } else {
                        day = dt.DayOfWeek.ToString();
                    }

                    C.Success($"Successfully added \"{title.Trim()}\" to {day}'s list.");
                }
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

        public static async Task Complete(string arg1, int arg2) {
            Dictionary<string, string> day_lookup = new Dictionary<string, string>(){
                {"past", "0"},
                {"sunday", "1"},
                {"monday", "2"},
                {"tuesday", "3"},
                {"wednesday", "4"},
                {"thursday", "5"},
                {"friday", "6"},
                {"saturday", "7"},
                {"future", "8"},
            };

            arg1 = arg1.ToLower();
    
            if (arg1 == "today" || arg1 == "past" || arg1 == "future" || arg1 == "sunday" || arg1 == "monday"
                || arg1 == "tuesday" || arg1 == "wednesday" || arg1 == "thursday"
                || arg1 == "friday" || arg1 == "saturday") {
                    if (arg1 == "today") {
                        arg1 = day_lookup[DateTime.Today.DayOfWeek.ToString().ToLower()];
                    } else {
                        arg1 = day_lookup[arg1];
                    }
                }

            DateTime beginning = DateTime.Today.AddDays(-1*(int)DateTime.Today.DayOfWeek);

            // get date from arg1
            DateTime dt;
            if (arg1 == "0") dt = beginning;
            else dt = beginning.AddDays(int.Parse(arg1) - 1);
            
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
                            (bool)reader[4],
                            (int)reader[5],
                            (bool)reader[6],
                            (int)reader[7]
                        );

                        reminders.Add(r);
                    }

                } catch (Exception e) {
                    C.Error(e.Message);
                }
            }

            con.Close();

            // reminders.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            // reminders = reminders.OrderBy(x => x.Completed).ToList();
            reminders.Sort(Reminder.CompareDates);
            reminders = reminders.OrderBy(x => x.Completed).ToList();

            con = new NpgsqlConnection(
            connectionString: Program.ConnectionString);
            con.Open();
            using (var cmd = new NpgsqlCommand()) {
                try {
                    cmd.Connection = con;
                    
                    cmd.CommandText = $"UPDATE reminders SET completed = NOT completed WHERE user_id = @user_id AND reminder_id = @reminder_id";
                    cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Program.user_id;
                    cmd.Parameters.Add("@reminder_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = reminders[arg2-1].Id;
                    await cmd.ExecuteNonQueryAsync();

                } catch (Exception e) {
                    C.Error(e.Message);
                }
            }

            string title = reminders[arg2-1].Title.Trim();
            C.Success($"Reminder \"{title}\" was marked complete!");
            con.Close();
        }
    }
}
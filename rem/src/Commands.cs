using rem;
using print;
using Npgsql;
using types;

namespace commands {
    public static class Commands {
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
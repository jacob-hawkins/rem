using rem;
using print;
using Npgsql;
using database;

namespace commands {
    public class View {
        public static async Task SeeReminders() {
            List<Reminder> reminders;
            List<DateTime> dates;
            DateTime today = DateTime.Today;
            
            dates = GetDates();

            Console.WriteLine("\n");
            C.WriteYellowLine($"1 {DateTime.Today.DayOfWeek} {DateTime.Now}");
            C.WriteYellowLine("----------------------------------------------------------");
            
            // print categories
            for (int i = 0; i < dates.Count; i++) {
                int k = 1;
                if (today.AddDays(i) == DateTime.Today) {
                    // print past
                    List<Reminder> past_reminders = await GetList(today, "past");
                    if (past_reminders.Count != 0) {
                        for (int j = 0; j < past_reminders.Count; j++) {
                            if (past_reminders[j].Completed == true) {
                                Database.Delete(past_reminders[j].Id);
                                continue;
                            }
                            CheckColor(past_reminders[j], k, past_reminders[j].Date);
                            k++;
                        }
                    }
                } else {
                    C.WriteBlueLine($"{i+1} {today.AddDays(i).DayOfWeek} {today.AddDays(i).Month}/{today.AddDays(i).Day}");
                }
                
                reminders = await GetList(dates[i], "");
                PrintReminders(dates[i], k, reminders);
            }
            
            // print future
            reminders = await GetList(dates[dates.Count-1], "future");
            if (reminders.Count != 0) {
                int j = 1;
                C.WriteBlueLine("8 Future");
                for (int i = 0; i < reminders.Count; i++) {
                    CheckColor(reminders[i], j, dates[dates.Count-1]);
                    j++;
                }
                
                Console.Write("\n");
            }
        }

        private static List<DateTime> GetDates() {
            List<DateTime> dates = new List<DateTime>();
            DateTime date;
            
            DateTime today = DateTime.Today;
            for (int i = 0; i < 7; i++) {
                date = today.AddDays(i);
                dates.Add(date);
            }
            
            return dates;
        }

        private static async Task<List<Reminder>> GetList(DateTime dt, string s) {
            List<Reminder> reminders = new List<Reminder>();
            
            using (var con = new NpgsqlConnection(connectionString: Program.ConnectionString)) {
                con.Open();
                
                using (var cmd = new NpgsqlCommand()) {
                    cmd.Connection = con;
                    if (s == "past") 
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date < @date";
                    else if (s == "future")
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date > @date";
                    else
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date = @date";
                    
                    cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Program.user_id;
                    cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
                    
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            var r = new Reminder(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("title")),
                                reader.GetDateTime(reader.GetOrdinal("date")),
                                reader.GetBoolean(reader.GetOrdinal("completed")),
                                reader.GetInt32(reader.GetOrdinal("work_on_count")),
                                reader.GetBoolean(reader.GetOrdinal("work_on")),
                                reader.GetInt32(reader.GetOrdinal("work_on_reminder"))
                            );

                            reminders.Add(r);
                        }
                    }
                }
            }
        
            return reminders;
        }

        private static void CheckColor(Reminder r, int j, DateTime dt) {
            string completed = "[ ]";
            const string format = "{0} {1,-32} {2} {3, 6}";
            string title = r.Title;

            if (r.Work_on) {
                title = $"WORK ON: {r.Title}";
            } else if (r.Work_on_count != 0) {
                title = $"{r.Title} ({r.Work_on_count} subtasks)";
            }

            if (r.Completed == true) {
                completed = "[X]";
                
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(format, j, title, r.Date.ToString("d"), completed);
                Console.ResetColor();
            } else if (DateTime.Compare(DateTime.Today, dt) > 0) {
                // overdue
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(format, j, title, r.Date.ToString("d"), completed);
                Console.ResetColor();

            } else {
                Console.ResetColor();
                Console.WriteLine(format, j, title, r.Date.ToString("d"), completed);
            }
        }

        private static void PrintReminders(DateTime dt, int x, List<Reminder> reminders) {
            if (reminders.Count == 0) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                if (dt == DateTime.Today) {
                    Console.WriteLine($"Nothing going on today!\n");
                } else {
                    Console.WriteLine($"Nothing going on {dt.DayOfWeek}!\n");
                }
                Console.ResetColor();
                
            } else {
                int j = x;
                reminders.Sort(Reminder.CompareDates);
                reminders = reminders.OrderBy(x => x.Completed).ToList();
                
                for (int i = 0; i < reminders.Count; i++) { 
                    CheckColor(reminders[i], j, reminders[i].Date);
                    j++;
                }
                
                Console.Write("\n");
            }
        }
    }
}
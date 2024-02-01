using rem;
using commands;
using helper;
using print;
using Npgsql;

namespace view {
    public class View {
        public static async Task SeeReminders() {
            List<Reminder> reminders;
            List<DateTime> dates;
            List<string> categories = new List<string>(){
                "1 Sunday",
                "2 Monday",
                "3 Tuesday",
                "4 Wednesday",
                "5 Thursday",
                "6 Friday",
                "7 Saturday"
            };
            DateTime beginning = DateTime.Today.AddDays(-1*(int)DateTime.Today.DayOfWeek);
            
            
            dates = GetDates();

            Console.WriteLine("\n");
            C.WriteYellow($"{DateTime.Today.DayOfWeek} {DateTime.Today}");
            C.WriteYellow("----------------------------------------------------------");
            
            // print past
            reminders = await GetList(dates[0], "past");
            if (reminders.Count != 0) {
                int j = 1;
                C.WriteBlue("0 Past");
                for (int i = 0; i < reminders.Count; i++) {
                    CheckColor(reminders[i], j, dates[0]);
                    j++;
                }
                
                Console.Write("\n");
            }


            // print categories
            for (int i = 0; i < dates.Count; i++) {
                if (beginning.AddDays(i) == DateTime.Today) {
                    C.WriteYellow($"{categories[i]} {beginning.AddDays(i).Month}/{beginning.AddDays(i).Day}");
                } else {
                    C.WriteBlue($"{categories[i]} {beginning.AddDays(i).Month}/{beginning.AddDays(i).Day}");
                }
                
                reminders = await GetList(dates[i], "");
                PrintReminders(dates[i], reminders);
            }
            
            // print future
            reminders = await GetList(dates[dates.Count-1], "future");
        }

        private static List<DateTime> GetDates() {
            List<DateTime> dates = new List<DateTime>();
            DateTime date;
            
            DateTime beginning = DateTime.Today.AddDays(-1*(int)DateTime.Today.DayOfWeek);
            for (int i = 0; i < 7; i++) {
                date = beginning.AddDays(i);
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
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date <= @date AND completed = false";
                    else if (s == "future")
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date >= @date AND completed = false";
                    else
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date = @date AND completed = false";
                    
                    cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Program.user_id;
                    cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
                    
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            var r = new Reminder(
                                (int)reader[0],
                                (string)reader[2] ?? string.Empty,
                                (DateTime)reader[3],
                                (bool)reader[4]
                            );

                            reminders.Add(r);
                        }
                    }
                }

                using (var cmd = new NpgsqlCommand()) {
                    cmd.Connection = con;
                    if (s == "past") 
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date <= @date AND completed = true";
                    else if (s == "future")
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date >= @date AND completed = true";
                    else
                        cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = @user_id AND date = @date AND completed = true";
                    cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Program.user_id;
                    cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
                    
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            if (s == "past") {
                                Helper.DeleteFromDB((int)reader[0]);
                                continue;
                            }
                            
                            var r = new Reminder(
                                (int)reader[0],
                                (string)reader[2] ?? string.Empty,
                                (DateTime)reader[3],
                                (bool)reader[4]
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

                    if (r.completed == true) {
                        completed = "[X]";
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(format, j, r.title, r.date.ToString("d"), completed);
                        Console.ResetColor();
                    } else if (DateTime.Compare(DateTime.Today, dt) > 0) {
                        // overdue
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(format, j, r.title, r.date.ToString("d"), completed);
                        Console.ResetColor();

                    } else {
                        Console.ResetColor();
                        Console.WriteLine(format, j, r.title, r.date.ToString("d"), completed);
                    }
        }

        private static void PrintReminders(DateTime dt, List<Reminder> reminders) {
            if (reminders.Count == 0) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                if (dt == DateTime.Today) {
                    Console.WriteLine($"Nothing going on today!\n");
                } else {
                    Console.WriteLine($"Nothing going on {dt.DayOfWeek}!\n");
                }
                Console.ResetColor();
                
            } else {
                int j = 1;
                
                for (int i = 0; i < reminders.Count; i++) { 
                    CheckColor(reminders[i], j, reminders[i].date);
                    j++;
                }
                
                Console.Write("\n");
            }
        }
    }
}
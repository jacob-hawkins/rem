using rem;
using helper;
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

    public class Commands {
        public static void Init() {
            
        }

        public static async void Add() {
            try {
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                DateTime dt = DateTime.Now;

                // Insert data
                cmd.CommandText = $"INSERT INTO reminders (user_id, title, date) VALUES (@user_id, @title, @date)";
                cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = 1;
                cmd.Parameters.Add("@title", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "Reminder from program";
                cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
                await cmd.ExecuteNonQueryAsync();

                con.Close();

                C.Success("Successfully added reminder to list.");
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

        public static async void View() {
            const string format = "{0} {1,-32} {2} {3, 6}";
            int j = 1;
            List<Reminder> reminders = new List<Reminder>();
            DateTime dt = DateTime.Today;

            try {
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                
                cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = {Program.user_id}";
                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync()) {
                    var r = new Reminder(
                        (int)reader[1],
                        (string)reader[2] ?? String.Empty,
                        (DateTime)reader[3],
                        (bool)reader[4]
                    );

                    reminders.Add(r);
                }

                con.Close();
                
                reminders = SortReminders(reminders);

                Console.WriteLine("\n");
                C.WriteBlue(dt.ToString("d") + " - " + DateTime.Today.DayOfWeek);
                C.WriteBlue("----------------------------------------------------------");

                for (int i = 0; i < reminders.Count; i++) { 
                    string completed = "[ ]";
                    // Console.Write(DateTime.Compare(dt, reminders[i].date) + " ");

                    if (reminders[i].completed == true) {
                        completed = "[X]";
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(format, j, reminders[i].title, reminders[i].date.ToString("d"), completed);
                        Console.ResetColor();
                    } else if (DateTime.Compare(dt, reminders[i].date) > 0) {
                        // overdue
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(format, j, reminders[i].title, reminders[i].date.ToString("d"), completed);
                        Console.ResetColor();

                    } else {
                        Console.ResetColor();
                        Console.WriteLine(format, j, reminders[i].title, reminders[i].date.ToString("d"), completed);
                    }
                    
                    j++;
                }
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

        private static List<Reminder> SortReminders(List<Reminder> reminders) {
            reminders.Sort((x, y) => DateTime.Compare(x.date, y.date));
            reminders = reminders.OrderBy(x => x.completed).ToList();

            int beginning = FindBeginningOfWeek();
        
            for (int i = 0; i < reminders.Count; i++) {
                // completed reminders from earlier than the beginning of this week
                if (DateTime.Compare(DateTime.Today.AddDays(beginning), reminders[i].date) > 0 && reminders[i].completed == true) {
                    Helper.DeleteFromDB(reminders[i].reminder_id);
                }

                Console.WriteLine("");
                
                if (DateTime.Compare(DateTime.Today.AddDays(beginning), reminders[i].date) > 0 && reminders[i].completed != true) {
                    Console.WriteLine(reminders[i].date);
                }
            }
            
            return reminders;
        }

        private static int FindBeginningOfWeek() {
            int beginning;

            switch (DateTime.Today.DayOfWeek.ToString()) {
                case "Sunday":
                    beginning = 0;
                    break;
                case "Monday":
                    beginning = -1;
                    break;
                case "Tuesday":
                    beginning = -2;
                    break;
                case "Wednesday":
                    beginning = -3;
                    break;
                case "Thursday":
                    beginning = -4;
                    break;
                case "Friday":
                    beginning = -5;
                    break;
                case "Saturday":
                    beginning = -6;
                    break;
                default:
                    Helper.Error("invalid date");
                    return -1;
            }
        
            return beginning;
        }

    }
}
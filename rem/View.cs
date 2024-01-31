using rem;
using commands;
using helper;
using Npgsql;

namespace view {
    public class View {
        private static List<Reminder> OverDue { get; set; } = [];
        private static List<Reminder> Sunday { get; set; } = [];
        private static List<Reminder> Monday { get; set; } = [];
        private static List<Reminder> Tuesday { get; set; } = [];
        private static List<Reminder> Wednesday { get; set; } = [];
        private static List<Reminder> Thursday { get; set; } = [];
        private static List<Reminder> Friday { get; set; } = [];
        private static List<Reminder> Saturday { get; set; } = [];
        private static List<Reminder> Future { get; set; } = [];

        public static async void SeeReminders() {
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
                        (int)reader[0],
                        (string)reader[2] ?? String.Empty,
                        (DateTime)reader[3],
                        (bool)reader[4]
                    );

                    reminders.Add(r);
                }

                con.Close();
                
                } catch (Exception e) {
                    C.Error(e.Message);
                }
                
                SortReminders(reminders);
                int beginning = FindBeginningOfWeek();

                Console.WriteLine("\n");
                C.WriteYellow($"{DateTime.Today.DayOfWeek} {dt.ToString("d")}");
                C.WriteYellow("----------------------------------------------------------");

                if (OverDue.Count > 0) {
                    C.WriteBlue("0 Past");
                    PrintReminders(OverDue);
                }

                CheckColorOfDay($"1 Sunday {DateTime.Today.AddDays(beginning).Month}/{DateTime.Today.AddDays(beginning).Day}");
                if (Sunday.Count > 0) PrintReminders(Sunday);
                else NoRemindersToday(beginning);

                CheckColorOfDay($"2 Monday {DateTime.Today.AddDays(beginning+1).Month}/{DateTime.Today.AddDays(beginning+1).Day}");
                if (Monday.Count > 0) PrintReminders(Monday);
                else NoRemindersToday(beginning+1);

                CheckColorOfDay($"3 Tuesday {DateTime.Today.AddDays(beginning+2).Month}/{DateTime.Today.AddDays(beginning+2).Day}");
                if (Tuesday.Count > 0) PrintReminders(Tuesday);
                else NoRemindersToday(beginning+2);

                CheckColorOfDay($"4 Wednesday {DateTime.Today.AddDays(beginning+3).Month}/{DateTime.Today.AddDays(beginning+3).Day}");
                if (Wednesday.Count > 0) PrintReminders(Wednesday);
                else NoRemindersToday(beginning+3);

                CheckColorOfDay($"5 Thursday {DateTime.Today.AddDays(beginning+4).Month}/{DateTime.Today.AddDays(beginning+4).Day}");
                if (Thursday.Count > 0) PrintReminders(Thursday);
                else NoRemindersToday(beginning+4);

                CheckColorOfDay($"6 Friday {DateTime.Today.AddDays(beginning+5).Month}/{DateTime.Today.AddDays(beginning+5).Day}");
                if (Friday.Count > 0) PrintReminders(Friday);
                else NoRemindersToday(beginning+5);

                CheckColorOfDay($"7 Saturday {DateTime.Today.AddDays(beginning+6).Month}/{DateTime.Today.AddDays(beginning+6).Day}");
                if (Saturday.Count > 0) PrintReminders(Saturday);
                else NoRemindersToday(beginning+6);

                if (Future.Count > 0) {
                    C.WriteBlue("8 Future");
                    PrintReminders(Future);
                }

        }


    
        private static void SortReminders(List<Reminder> reminders) {            
            reminders.Sort((x, y) => DateTime.Compare(x.date, y.date));
            reminders = reminders.OrderBy(x => x.completed).ToList();

            int beginning = FindBeginningOfWeek();
        
            for (int i = 0; i < reminders.Count; i++) {
                // completed reminders from earlier than the beginning of this week
                if (DateTime.Compare(DateTime.Today.AddDays(beginning), reminders[i].date) > 0 && reminders[i].completed == true) {
                    Console.WriteLine(reminders[i].reminder_id);
                    Helper.DeleteFromDB(reminders[i].reminder_id);
                    continue;
                }
                
                // overdue
                if (DateTime.Compare(DateTime.Today.AddDays(beginning), reminders[i].date) > 0) {
                    OverDue.Add(reminders[i]);
                    continue;
                }

                // due sunday
                if (DateTime.Compare(DateTime.Today.AddDays(beginning), reminders[i].date) == 0) {
                    Sunday.Add(reminders[i]);
                    continue;
                }

                // due monday
                if (DateTime.Compare(DateTime.Today.AddDays(beginning+1), reminders[i].date) == 0) {
                    Monday.Add(reminders[i]);
                    continue;
                }

                // due tuesday
                if (DateTime.Compare(DateTime.Today.AddDays(beginning+2), reminders[i].date) == 0) {
                    Tuesday.Add(reminders[i]);
                    continue;
                }

                // due wednesday
                if (DateTime.Compare(DateTime.Today.AddDays(beginning+3), reminders[i].date) == 0) {
                    Wednesday.Add(reminders[i]);
                    continue;
                }

                // due thursday
                if (DateTime.Compare(DateTime.Today.AddDays(beginning+4), reminders[i].date) == 0) {
                    Thursday.Add(reminders[i]);
                    continue;
                }

                // due friday
                if (DateTime.Compare(DateTime.Today.AddDays(beginning+5), reminders[i].date) == 0) {
                    Friday.Add(reminders[i]);
                    continue;
                }

                // due saturday
                if (DateTime.Compare(DateTime.Today.AddDays(beginning+6), reminders[i].date) == 0) {
                    Saturday.Add(reminders[i]);
                    continue;
                }

                // future
                if (DateTime.Compare(DateTime.Today.AddDays(beginning+7), reminders[i].date) < 0) {
                    Future.Add(reminders[i]);
                    continue;
                }
            }
        }

        private static void PrintReminders(List<Reminder> reminders) {
            int j = 1;
            const string format = "{0} {1,-32} {2} {3, 6}";
            DateTime dt = DateTime.Today;
            
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

            Console.Write("\n");
        }

        private static void NoRemindersToday(int i) {
            if (i == 0) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Nothing going on today!\n");
                Console.ResetColor();
            } else {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Nothing going on {DateTime.Today.AddDays(i).DayOfWeek}!\n");
                Console.ResetColor();
            }

        }

        private static void CheckColorOfDay(string s) {
            if (s.Split(' ')[1] == DateTime.Today.DayOfWeek.ToString()) {
                C.WriteYellow(s);
            } else C.WriteBlue(s);
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
                    C.Error("Invalid date.");
                    return -1;
            }
        
            return beginning;
        }
    }
}
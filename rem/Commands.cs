using rem;
using helper;
using Npgsql;
using add;

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

        public static void Add() {
            DateTime dt;
            string title = "";
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Remeber: ");
            Console.ResetColor();

            string? reminder = Console.ReadLine();
            if (reminder == null) return;


            if (reminder.Contains("on ")) {
                var rem_split = reminder.Split("on ");
                
                title = rem_split[0];
                dt = add.Add.FindDate(rem_split[1]);

                if (dt == DateTime.MinValue) {
                    C.Error("Invalid Date");
                    return;
                }
                Console.WriteLine(title);
                Console.WriteLine(dt);
            } else {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("When: ");
                Console.ResetColor();

                string? date = Console.ReadLine();
                if (reminder == null) return;
            }

            // try {
            //     var con = new NpgsqlConnection(
            //     connectionString: Program.ConnectionString);
            //     con.Open();
            //     using var cmd = new NpgsqlCommand();
            //     cmd.Connection = con;

            //     // Insert data
            //     cmd.CommandText = $"INSERT INTO reminders (user_id, title, date) VALUES (@user_id, @title, @date)";
            //     cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = 1;
            //     cmd.Parameters.Add("@title", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "Reminder from program";
            //     cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
            //     await cmd.ExecuteNonQueryAsync();

            //     con.Close();

            //     C.Success("Successfully added reminder to list.");
            // } catch (Exception e) {
            //     C.Error(e.Message);
            // }
        }
    }
}
using Npgsql;
using rem;
using helper;
using System.Reflection;
using Npgsql.Replication;

namespace commands {
    public class Reminder {
        public string title { get; set; } = string.Empty;
        public DateTime date { get; set; }
        public bool completed { get; set; }

        public Reminder (string title, DateTime date, bool completed) {
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

                C.Success("Successfully added reminder to list.");
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

        public static async void View() {
            const string format = "{0} {1,-32} {2} {3, 6}";
            int j = 1;
            List<Reminder> reminders = new List<Reminder>();

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
                        (string)reader[2] ?? String.Empty,
                        (DateTime)reader[3],
                        (bool)reader[4]
                    );

                    reminders.Add(r);
                }
                    
                for (int i = 0; i < reminders.Count; i++) { 
                    string completed = "[ ]";

                    if (reminders[i].completed == true) {
                        completed = "[X]";
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(format, j, reminders[i].title, reminders[i].date, completed);
                        Console.ResetColor();
                    } else {
                        Console.WriteLine(format, j, reminders[i].title, reminders[i].date, completed);
                    }
                    
                    j++;
                }
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

    }
}
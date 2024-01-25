using Npgsql;
using rem;
using helper;

namespace commands {
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

                // Insert some data
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
            int i = 1;

            try {
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                
                cmd.CommandText = $"SELECT * FROM reminders WHERE user_id = {Program.user_id}";
                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync()) {
                    string completed = "[ ]";

                    if (reader[4].ToString() == "True") {
                        completed = "[X]";
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(format, i, reader[2], reader[3], completed);
                        Console.ResetColor();
                    } else {
                        Console.WriteLine(format, i, reader[2], reader[3], completed);
                    }
                    
                    i++;
                }
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

    }
}
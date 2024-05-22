using Npgsql;
using rem;
using print;

namespace database {
    public static class Database {
        public static async Task<bool> CheckForUser(string username, string password) {
            try {
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT * FROM users WHERE username = @username AND password = @password";
                cmd.Parameters.Add("@username", NpgsqlTypes.NpgsqlDbType.Text).Value = username;
                cmd.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Text).Value = password;
                
                List<int> users = new();
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        users.Add(reader.GetInt32(reader.GetOrdinal("id")));
                    }
                }

                con.Close();

                if (users.Count > 0) {
                    return true;
                }
            } catch (Exception e) {
                C.Error(e.Message);
            }
            
            return false;
        }
        
        public static async void Delete(int reminder_id) {
            try {
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"DELETE FROM reminders WHERE id = @reminder_id";
                cmd.Parameters.Add("@reminder_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = reminder_id;
                await cmd.ExecuteNonQueryAsync();

                con.Close();
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }
    }
}
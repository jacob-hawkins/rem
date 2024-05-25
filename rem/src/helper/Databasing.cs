using Npgsql;
using rem;
using print;
using types;

namespace database {
    public static class Database {
        public static async Task<Either<User, string>> GetUser(int id) {
            try {
                User? u = null;
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT * FROM users WHERE id = @id";
                cmd.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = id;
            
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if (reader.Read()) {
                        u = new User(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("password")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetInt32(reader.GetOrdinal("completed_reminders")),
                            reader.GetInt32(reader.GetOrdinal("total_reminders")),
                            reader.GetBoolean(reader.GetOrdinal("admin")),
                            reader.GetString(reader.GetOrdinal("notion_key"))
                        );
                    }
                }

                con.Close();
                return new Either<User, string>(u!);

            } catch (Exception e) {
                C.Error(e.Message);
                return new Either<User, string>("User could not be found.");
            }
        }

        public static async Task<Either<User, string>> GetUser(string username, string password) {
            try {
                User? u = null;
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"SELECT * FROM users WHERE username = @username AND password = @password";
                cmd.Parameters.Add("@username", NpgsqlTypes.NpgsqlDbType.Text).Value = username;
                cmd.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Text).Value = password;
            
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if (reader.Read()) {
                        u = new User(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("password")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetInt32(reader.GetOrdinal("completed_reminders")),
                            reader.GetInt32(reader.GetOrdinal("total_reminders")),
                            reader.GetBoolean(reader.GetOrdinal("admin")),
                            reader.GetString(reader.GetOrdinal("notion_key"))
                        );
                    }
                }

                con.Close();
                return new Either<User, string>(u!);

            } catch {
                return new Either<User, string>("User could not be found.");
            }
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
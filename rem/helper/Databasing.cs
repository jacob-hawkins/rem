using Npgsql;
using rem;
using print;

namespace database {
    public static class Database {
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
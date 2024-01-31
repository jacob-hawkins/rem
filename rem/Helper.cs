using rem;
using commands;
using Npgsql;

namespace helper {
    public static class Helper {    
        public static async void DeleteFromDB(int reminder_id) {
            try {
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"DELETE FROM reminders WHERE reminder_id = @reminder_id";
                cmd.Parameters.Add("@reminder_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = reminder_id;
                await cmd.ExecuteNonQueryAsync();

                con.Close();
                Console.WriteLine("Successfully deleted");
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

        public static void Usage() {
            string logo = @"
 /$$$$$$$  /$$$$$$$$ /$$      /$$
| $$__  $$| $$_____/| $$$    /$$$
| $$  \ $$| $$      | $$$$  /$$$$
| $$$$$$$/| $$$$$   | $$ $$/$$ $$
| $$__  $$| $$__/   | $$  $$$| $$
| $$  \ $$| $$      | $$\  $ | $$
| $$  | $$| $$$$$$$$| $$ \/  | $$
|__/  |__/|________/|__/     |__/";

            Console.WriteLine("{0}\n", logo);

            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Thank you for using Rem!\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0,-10}", "init");
            Console.ResetColor();

            Console.WriteLine("{0,50:N1}", "Initialize reminder system files. (This only needs to be run once)");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0,-10}", "add");
            Console.ResetColor();

            Console.WriteLine("{0,50:N1}", "Add a reminder to list. You will be prompted for a title, date, and optional 'work on days'");
        }
    }

    public static class C {
        public static void WriteBlue(string s) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(s);
            Console.ResetColor();        
        }

        public static void WriteGray(string s) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(s);
            Console.ResetColor();        
        }

        public static void WriteYellow(string s) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            Console.ResetColor();    
        }

        public static void Error(string s) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\u2718 " + s);
            Console.ResetColor();    
        }

        public static void Success(string s) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\u2714 " + s);
            Console.ResetColor();    
        }
    }

}

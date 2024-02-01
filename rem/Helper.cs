using rem;
using Npgsql;
using print;

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
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

         public static int FindBeginningOfWeek() {
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
            Console.Write("{0,-15}", "init");
            Console.ResetColor();

            Console.WriteLine("Initialize reminder system files. (This only needs to be run once)");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0,-15}", "add");
            Console.ResetColor();

            Console.WriteLine("Add a reminder to list. You will be prompted for a title, date, and optional 'work on days'");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0,-15}", "complete");
            Console.ResetColor();

            Console.WriteLine("Mark or unmark reminder as completed.");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0,-15}", "view");
            Console.ResetColor();

            Console.WriteLine("Print your reminders.");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0,-15}", "help");
            Console.ResetColor();

            Console.WriteLine("Displays all commands and descriptions available.");
        }
    }
}

namespace helper {
    public static class Helper {    
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

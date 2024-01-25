using rem;
using commands;

namespace helper {
    public static class Helper {
        public static bool IsInited() {        
            if (File.Exists(Program.path)) {
                return true;
            } else {
                return false;
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

        public static void Error(string err) {
            switch (err) {
                case "init":
                    C.Error("Rem file already exsits!");
                    break;

                case "commandNotFound":
                    C.Error("Command not found (See help).");
                    break;
                
                default:
                    C.Error("Error");
                    break;
            }
        }
    }

    public static class C {
        public static void WriteBlue(string s) {
            Console.ForegroundColor = ConsoleColor.Blue;
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

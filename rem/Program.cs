using commands;
using helper;
using view;

namespace rem {
    public class Program {
        public const string path = "./data/rem.md";
        public const int user_id = 1;
        public const string ConnectionString = "Server=localhost;Port=5432;User Id=postgres;Password=password;Database=rem;";

        static void Main(string[] args) {
            if (args.Length == 0) {
                Helper.Usage();
                return;
            }

            ControlFlow(args);
        }

        public static void ControlFlow(string[] args) {
            switch (args[0].ToString()) {
                case "init":
                    Commands.Init();
                    break;
                case "add":
                    Commands.Add();
                    break;
                case "complete":
                    if (args.Length < 3) {
                        C.Error("Usage: rem complete [category #] [reminder #]");
                        break;
                    } else {
                        Commands.Complete(args[1], int.Parse(args[2]));
                    }
                    break;
                case "view":
                    View.SeeReminders();
                    break;
                case "help":
                    Helper.Usage();
                    break;

                default:
                    C.Error("Command could not be found (use help to view commands).");
                    break;
            }
        }
    }
}
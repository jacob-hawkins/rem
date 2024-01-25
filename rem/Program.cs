using commands;
using helper;

namespace rem {
    public class Program {
        public const string path = "./data/rem.md";
        public const string user_id = "1";
        public const string ConnectionString = "Server=localhost;Port=5432;User Id=postgres;Password=password;Database=rem;";

        static void Main(string[] args) {
            if (args.Length == 0) {
                Helper.Usage();
                return;
            }

            ControlFlow(args[0].ToString());
        }

        public static void ControlFlow(string command) {
            switch (command) {
                case "init":
                    Commands.Init();
                    break;
                case "add":
                    Commands.Add();
                    break;
                case "view":
                    Commands.View();
                    break;
                case "help":
                    Helper.Usage();
                    break;

                default:
                    Helper.Error("commandNotFound");
                    break;
            }
        }
    }
}
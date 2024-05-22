using commands;
using helper;
using view;
using print;
using init;

namespace rem {
    public class Program {
        public const string path = "./data/rem.md";
        public const int user_id = 1;
        public const string ConnectionString = "Server=localhost;Port=5432;User Id=postgres;Password=password;Database=rem;";

        static async Task Main(string[] args) {
            if (args.Length == 0) {
                Helper.Usage();
                return;
            }

            await ControlFlow(args);
        }

        public static async Task ControlFlow(string[] args) {
            switch (args[0].ToString()) {
                case "init":
                    Init.Run();
                    break;
                case "add":
                    await Commands.Add(args);
                    break;
                case "complete":
                    if (args.Length < 3) {
                        C.Error("Usage: rem complete [category #] [reminder #]");
                        break;
                    } else {
                        await Commands.Complete(args[1], int.Parse(args[2]));
                    }
                    break;
                case "view":
                    await View.SeeReminders();
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
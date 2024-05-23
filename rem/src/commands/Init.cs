using helper;
using print;
using System.Text;
using database;
using types;

namespace init {
    public class Init {
        private const string env_path = "./.env.local";
        private static void AddToENV(string line) {
            using (StreamWriter outputFile = new StreamWriter(env_path, true)) {
                outputFile.WriteLine(line);
            }
        }

        private static string ReadPassword() {
            StringBuilder passwordBuilder = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Enter) {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace) {
                    if (passwordBuilder.Length > 0) {
                        passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
                    }
                }
                else {
                    passwordBuilder.Append(keyInfo.KeyChar);
                }
            }

            return passwordBuilder.ToString();
        }
        
        private static void SetUpNotion() {
            return;
        }
        private static async void Login() {
            string? username = "";
            while (username == "") {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Username: ");
                Console.ResetColor();
                username = Console.ReadLine();
            }
            
            string? password = "";
            while (password == "") {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Password: ");
                Console.ResetColor();
                password = ReadPassword();
            }
            
            Either<User, string> user = await Database.GetUser(username!, password);
            
            if (user.IsSuccess == false) {
                C.Error("No user could not be found. Check username and password.");
                return;
            }

            User u = user.Value!;
            
            AddToENV($"USER_ID = {u.Id}");
            AddToENV($"USER_NAME = {u.Username}");
            
            Console.WriteLine();
            C.Success("Account found!");
            Console.WriteLine();
            
            if (u.Notion_key == "-1") {
                // set up notion?
                bool res = Helper.BinaryResQuestion("Do you want to integrate using Notion?");

                if (res == true) {
                    C.WriteBlue("Great! I will walk you through step by step...");
                    SetUpNotion();
                }
            }
        }

        private static void SignUp() {
            AddToENV("Sign up");
        }

        public static void Run() {
            bool res = Helper.BinaryResQuestion("Running this will clear any previous setup. Do you want to continue?");
            if (!res) {
                return;
            }
            
            // clear env file
            using (StreamWriter outputFile = new StreamWriter(env_path)) {
                outputFile.WriteLine();
            }
           
            res = Helper.BinaryResQuestion("Do you already have an account?");
            Console.WriteLine();

            if (res) {
                Login();
            } else {
                SignUp();
            }
        }
    }
}
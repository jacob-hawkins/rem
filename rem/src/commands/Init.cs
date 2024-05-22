using helper;
using print;
using System.Text;
using database;
using types;

namespace init {
    public class Init {
        private static void AddToENV(List<string> lines) {
            using (StreamWriter outputFile = new StreamWriter("./.env.local")) {
                foreach (string line in lines)
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
        
        private static async void Login() {
            List<string> lines = new List<string> {};

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

            C.Success("Account found!");
            bool res = Helper.BinaryResQuestion("Do you want to integrate using Notion?");


            AddToENV(lines);
        }

        private static void SignUp() {
            List<string> lines = new List<string> {};
            lines.Add("Sign up");

            AddToENV(lines);
        }

        public static void Run() {
            bool res = Helper.BinaryResQuestion("Do you already have an account?");

            if (res == true) {
                Login();
            } else if (res == false) {
                SignUp();
            }
        }
    }
}
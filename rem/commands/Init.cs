using rem;
using commands;
using helper;
using print;
using Npgsql;
using System;
using System.Text;
using database;

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

            string? username = null;
            while (username == null) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Username: ");
                Console.ResetColor();
                username = Console.ReadLine();
            }
            
            string? password = null;
            while (password == null) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Password: ");
                Console.ResetColor();
                password = ReadPassword();
            }

            Console.WriteLine(username);
            Console.WriteLine(password);
            
            bool res = await Database.CheckForUser(username, password);

            if (res == false) {
                C.Error("No user could be found. Check username and password.");
                return;
            }

            AddToENV(lines);
        }

        private static void SignUp() {
            List<string> lines = new List<string> {};
            lines.Add("Sign up");

            AddToENV(lines);
        }

        public static void Run() {
            string? res = null;
            
            while (res == null) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Do you already have an account? [Y/N]: ");
                Console.ResetColor();
                res = Console.ReadLine();
            }
            
            if (String.Equals(res.ToUpper(), "Y")) {
                Login();
            } else if (String.Equals(res.ToUpper(), "N")) {
                SignUp();
            } else {
                C.Error("Invalid Character Entered");
                return;
            }
        }
    }}
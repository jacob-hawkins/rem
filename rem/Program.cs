using System;
using System.ComponentModel;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Data;
using Npgsql;


namespace rem {
    public class Program {
        public const string path = "./data/rem.md";
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

                default:
                    Helper.Error("commandNotFound");
                    break;
            }
        }
    }

    public static class Commands {
        public static void Init() {
            // check if already initiated
            if (Helper.IsInited()) {
                Helper.Error("init");
            } else {
                // initiate file
                string createText = "# REM" + Environment.NewLine + Environment.NewLine + 
                    "## Weekly Reminders" + Environment.NewLine + Environment.NewLine + 
                    "## Up Coming Reminders" + Environment.NewLine + Environment.NewLine + 
                    "## Daily Reminders" + Environment.NewLine + Environment.NewLine + 
                    "### 1. Sunday" + Environment.NewLine + Environment.NewLine + 
                    "### 2. Monday" + Environment.NewLine + Environment.NewLine + 
                    "### 3. Tuesday" + Environment.NewLine + Environment.NewLine + 
                    "### 4. Wednesday" + Environment.NewLine + Environment.NewLine + 
                    "### 5. Thursday" + Environment.NewLine + Environment.NewLine + 
                    "### 6. Friday" + Environment.NewLine + Environment.NewLine + 
                    "### 7. Saturday" + Environment.NewLine + Environment.NewLine;
            
                File.WriteAllText(Program.path, createText);
                C.Success("Successfully created your Rem file!");
            }
        }

        public async static void Add() {
            try {
                var con = new NpgsqlConnection(
                connectionString: Program.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand();
                cmd.Connection = con;

                DateTime dt = DateTime.Now;

                // Insert some data
                cmd.CommandText = $"INSERT INTO reminders (user_id, title, date) VALUES (@user_id, @title, @date)";
                cmd.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = 1;
                cmd.Parameters.Add("@title", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "Reminder from program";
                cmd.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date).Value = dt;
                await cmd.ExecuteNonQueryAsync();

                C.Success("Successfully added reminder to list.");

                // Retrieve all rows
                // cmd.CommandText = $"SELECT * FROM reminders";
                // NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                // while (await reader.ReadAsync()) {
                //     Console.WriteLine(reader[0]);
                //     Console.WriteLine(reader[1]);
                //     Console.WriteLine(reader[2]);
                //     Console.WriteLine(reader[3]);
                // }
            } catch (Exception e) {
                C.Error(e.Message);
            }
        }

    }

    public static class Helper {
        public static bool IsInited() {        
            if (File.Exists(Program.path)) {
                return true;
            } else {
                return false;
            }
        }

        public static void Usage() {
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
                    C.Error("Command not found (See usage).");
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
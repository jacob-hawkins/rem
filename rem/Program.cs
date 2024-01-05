using System;
using System.IO;
using System.Reflection.Metadata;
using System.Text;

namespace rem;
public class Program {
    public const string path = "./data/rem.md";

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
            C.WriteGreen("\u2714 Successfully created your Rem file!");
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
                C.WriteRed("\u2718 Rem file already exsits!");
                break;

            case "commandNotFound":
                C.WriteRed("\u2718 Command not found (See usage).");
                break;
            
            default:
                Console.WriteLine("\u2718 Error");
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

    public static void WriteRed(string s) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(s);
        Console.ResetColor();    
    }

    public static void WriteGreen(string s) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(s);
        Console.ResetColor();    
    }
}
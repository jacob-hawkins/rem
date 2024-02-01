namespace print {
     public static class C {
        public static void WriteBlue(string s) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(s);
            Console.ResetColor();        
        }

        public static void WriteGray(string s) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
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
using commands;
using helper;
using print;

namespace add {
    public class Add {
        public static DateTime FindDate(string d) {
            d = d.ToLower();

            if (d == "today" || d == "sunday" || d == "monday"
                || d == "tuesday" || d == "wednesday" || d == "thursday"
                || d == "friday" || d == "saturday") {
                if (d == "today") {
                    return DateTime.Today;
                }

                for (int i = 1; i <= 7; i++) {
                    string dt = DateTime.Today.AddDays(i).DayOfWeek.ToString().ToLower();
                    if (dt == d) {
                        return DateTime.Today.AddDays(i);
                    }
                }
            } else {
                try {
                    return DateTime.Parse(d);
                } catch {
                    return DateTime.MinValue;
                }
            }
            
            return DateTime.MinValue;
        }

        public static string PrintWorkOnDates() {
            string? dates;
            C.WriteBlue("When would you like to work on it? (Use commas to seperate dates)");
            dates = Console.ReadLine();

            if (dates == "") {
                C.Error("You must enter at least one work on date (Press enter again to cancel add).");
                Console.ForegroundColor = ConsoleColor.Blue;
                C.WriteBlue("When would you like to work on it? (Use commas to seperate dates)");
                Console.ResetColor();

                dates = Console.ReadLine();
                if (dates == "") return "";
            }

            return dates!;
        }
    }
}
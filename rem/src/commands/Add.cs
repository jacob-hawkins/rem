using database;
using helper;
using print;
using rem;
using types;

namespace commands {
    public class Add {
        public static async Task Run(String[] args) {
            DateTime dt;
            string title;
            char flag = ' ';
            List<DateTime> dates = [];
            int work_on_reminder_id = -1;
            int reminder_id = -1;
            List<Reminder> work_on_reminders = [];
            
            switch (args.Length) {
                case 1:
                    C.WriteBlue("Remember: ");
                    title = Console.ReadLine()!;
                    
                    if (title == "") return;
                    break;
                case 2:
                    if (args[1] == "-w") {
                        flag = 'w';

                        C.WriteBlue("Remember: ");
                        title = Console.ReadLine()!;
                        
                        if (title == "") return;
                    } else title = args[1];
                    break;
                case 3:
                    title = args[2];
                    if (args[1] == "-w") flag = 'w';
                    else {
                        C.Error("Unknown flag.");
                        return;
                    }
                    break;
                default:
                    C.Error("Usage: rem add [flag] [\"reminder\"]");
                    return;
            }

            if (title!.Contains("on ")) {
                var rem_split = title.Split("on ");
                
                title = rem_split[0];
                dt = FindDate(rem_split[1]);

                if (dt == DateTime.MinValue) {
                    C.Error("Invalid Date");
                    return;
                }
            } else {
                
                C.WriteBlue("When: ");
                string? date = Console.ReadLine();
                if (date == "") {
                    C.Error("You must enter a date (Press enter again to cancel add).");
                    C.WriteBlue("When: ");

                    date = Console.ReadLine();
                    if (date == "") return;
                }

                dt = FindDate(date!);

                if (dt == DateTime.MinValue) {
                    C.Error("Invalid Date");
                    return;
                }
            }

            if (flag == 'w') {
                dates = WorkOn();
                if (dates.Count == 0) return;
            }

            Reminder reminder = new Reminder(0, title, dt, false, dates.Count, false, 0);
            reminder_id = await Database.AddReminder(reminder);
            
            for (int i = 0; i < dates.Count; i++) {
                Reminder work_on = new Reminder(0, title, dates[i], false, 0, true, reminder_id);
                work_on_reminder_id = await Database.AddReminder(work_on);

                if (work_on_reminder_id == -1) {
                    C.Error($"Error adding \"{title}\"");
                }
            }
        }
        
        private static DateTime FindDate(string d) {
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

        private static List<DateTime> WorkOn() {
            List<DateTime> dates = [];
            string? res;
            
            C.WriteBlueLine("When would you like to work on it? (Use commas to separate dates)");
            res = Console.ReadLine();

            if (res == "") {
                C.Error("You must enter at least one work on date (Press enter again to cancel add).");
                C.WriteBlueLine("When would you like to work on it? (Use commas to separate dates)");

                res = Console.ReadLine();
                if (res == null || res == "") return [];
            }

            string[] res_split = res!.Split(",");
            for (int i = 0; i < res_split.Length; i++) {
                dates.Add(DateTime.Parse(res_split[i]));
            }

            return dates;
        }
    }
}
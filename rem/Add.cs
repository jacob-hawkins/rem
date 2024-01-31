using System.Security.Cryptography.X509Certificates;
using System.Xml;
using commands;
using helper;

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
    }
}
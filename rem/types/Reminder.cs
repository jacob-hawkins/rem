namespace types {
    public class Reminder {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool Completed { get; set; }
        public int Work_on_count { get; set; }
        public bool Work_on { get; set; }
        public int Work_on_reminder { get; set; }

        public Reminder (int id, string title, DateTime date, bool completed, int work_on_count, bool work_on, int work_on_reminder) {
            Id = id;
            Title = title;
            Date = date;
            Completed = completed;
            Work_on_count = work_on_count;
            Work_on = work_on;
            Work_on_reminder = work_on_reminder;
        }

        // Sort by date
        public static int CompareDates(Reminder x, Reminder y) {
            return DateTime.Compare(x.Date, y.Date);
        }

        // Sort false then true
        public static int CompareCompleted(Reminder x, Reminder y) {
            return x.Completed.CompareTo(y.Completed);
        }
    }
}
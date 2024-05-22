namespace user {
    public class User {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Completed_reminders {get; set;}
        public int Total_reminders {get; set;}
        public bool Admin {get; set;}
        public string Notion_key {get; set;}

        public User (int id, string username, string password, string email, int completed_reminders, int total_reminders, bool admin, string notion_key) {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            Completed_reminders = completed_reminders;
            Total_reminders = total_reminders;
            Admin = admin;
            Notion_key = notion_key;
        }
    }
}
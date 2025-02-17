namespace UserService.Model
{
    public class NotificationEvent<T>
    {
        public string EventType { get; set; }
        public DateTime TimeStamp { get; set; }
        public T Payload { get; set; }
    }

    public class NewUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}

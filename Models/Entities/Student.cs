namespace StudentManager.Models.Entities
{
    public class Student
    {
        public Guid Id {  set; get; }
        public string Name { set; get; }

        public string Email { set; get; }

        public string Phone { set; get; }

        public string Subscribed { set; get; }
    }
}

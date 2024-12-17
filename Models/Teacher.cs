namespace MyUquvMarkaz.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; } 
        public int Age { get; set; }
        public string Phone { get; set; }

        public ICollection<Subject> Subjects { get; set; } 
    }
}

namespace MyUquvMarkaz.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<Student> Students { get; set; } 
    }
}

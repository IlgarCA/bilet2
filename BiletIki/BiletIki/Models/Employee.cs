using System.ComponentModel.DataAnnotations.Schema;

namespace BiletIki.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Bio { get; set; }
        public string? Img { get; set; }
        [NotMapped]
        public IFormFile FormFile { get; set; }
    
    }
}

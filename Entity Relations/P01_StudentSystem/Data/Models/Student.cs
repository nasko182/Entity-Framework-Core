using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            this.StudentsCourses= new HashSet<StudentCourse>();
            this.Homeworks= new HashSet<Homework>();
        }
        [Key]
        public int StudentId { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Range(10,10)]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime RegisteredOn { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime? Birthday { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; }

        public ICollection<Homework> Homeworks { get; set; }
    }
}

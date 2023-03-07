using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            this.StudentsCourses = new HashSet<StudentCourse>();
            this.Homeworks= new HashSet<Homework>();
            this.Resources = new HashSet<Resource>();
        }
        [Key]
        public int CourseId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(80)]
        public string Name { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string? Description { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "DECIMAL")]
        public decimal Price { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; }

        public ICollection<Homework> Homeworks { get; set; }

        public ICollection<Resource> Resources { get; set; }

    }
}

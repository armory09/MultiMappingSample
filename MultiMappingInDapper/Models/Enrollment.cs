using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Enrollment")]
    public class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string Grade { get; set; }
        [Write(false)]
        public virtual Course Course { get; set; }
        [Write(false)]
        public virtual Student Student { get; set; }
    }
}

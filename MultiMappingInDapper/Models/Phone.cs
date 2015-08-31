using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Phone")]
    public class Phone
    {
        [Key]
        public int PhoneId { get; set; }
        public int ContactID { get; set; } // foreign key
        public string Number { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
    }
}

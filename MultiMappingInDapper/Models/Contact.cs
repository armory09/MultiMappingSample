using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }
        public string ContactName { get; set; }
        public IEnumerable<Phone> Phones { get; set; }
    }
}

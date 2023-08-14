using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.DB.Entities
{
    [Table("reportreasons")]
    public class ReportReason
    {
        [Key]
        public int Id { get; set;  }

        public string Reason { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.DB.Entities
{
    [Table("ebookreports")]
    public class EBookReport
    {
        [Key]
        public int Id { get; set;  }
        
        public int ReportDetailsId { get; set; }
        
        [ForeignKey("ReportDetailsId")]
        public virtual ReportDetails ReportDetails { get; set; }
    }
}
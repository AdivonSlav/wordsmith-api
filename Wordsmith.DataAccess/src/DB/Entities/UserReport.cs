using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.DB.Entities
{
    [Table("userreports")]
    public class UserReport
    {
        [Key] 
        public int Id { get; set; }
        
        public int UserId { get; set; } // The user who is being reported
        
        public int ReportDetailsId { get; set;  }
        
        [ForeignKey("ReportDetailsId")]
        public virtual ReportDetails ReportDetails { get; set; }
    }
}
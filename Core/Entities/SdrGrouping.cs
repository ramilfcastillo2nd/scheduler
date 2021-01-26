
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class SdrGrouping: BaseEntity
    {
        public int? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public UserProfile Manager { get; set; }
        public int? SdrId { get; set; }
        [ForeignKey("SdrId")]
        public UserProfile Sdr { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

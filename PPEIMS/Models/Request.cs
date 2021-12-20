using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PPEIMS.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReferenceNo { get; set; }
        public string CreatedBy { get; set; }
        public string DocumentStatus { get; set; }
        public string Status { get; set; }


        public DateTime DateSubmitted { get; set; }
        public int CreatedByUserId { get; set; }
        public int DepartmentHeadId { get; set; }
        public DateTime DepartmentApprovedDate { get; set; }
        public int SafetyId { get; set; }
        public DateTime SafetyApprovedDate { get; set; }
        public int WarehousemanId { get; set; }
        public DateTime WarehouseApprovedDate { get; set; }
        public int DepartmentId { get; set; }
        public int CompanyId { get; set; }
                                           //public virtual Department Departments { get; set;     }

    }
}

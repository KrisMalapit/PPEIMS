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
    }
}

using System;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPEIMS.Models
{
    public class RequestDetailUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RequestDetailId { get; set; }
        public virtual RequestDetail RequestDetails { get; set; }
        public int UserId { get; set; }
        public virtual User Users { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DocumentStatus { get; set; }
    }
}

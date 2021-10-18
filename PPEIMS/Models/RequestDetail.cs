using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PPEIMS.Models
{
    public class RequestDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RequestId { get; set; }
        public virtual Request Requests { get; set; }
        //public int UserId { get; set; }
        //public virtual User Users { get; set; }
        public int ItemId { get; set; }
        public virtual Item Items { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Type { get; set; }
        public string Remarks { get; set; }

    }
}

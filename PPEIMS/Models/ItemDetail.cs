using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPEIMS.Models
{
    public class ItemDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LineNo { get; set; }
        public int ItemId { get; set; }
        public virtual Item Items { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}

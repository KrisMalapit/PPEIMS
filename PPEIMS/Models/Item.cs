using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPEIMS.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR(20)")]
        [Required]
        public string No { get; set; }
      
        [Required]
        public string Description { get; set; }
        [Display(Name = "Description 2")]
       
        public string Description2 { get; set; }

        public string PPE { get; set; }

        public string Status { get; set; } = "Active";
    }
}

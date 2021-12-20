using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPEIMS.Models
{
    public class PPE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
      
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Status { get; set; } = "Active";
        public int Office { get; set; }
        public int Field { get; set; }
        //public PPE()
        //{
        //    Status = "Active";
        //}

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PPEIMS.Models
{
    public class DepartmentPPE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
       
        public int DepartmentId { get; set; }
        public virtual Department Departments { get; set; }
        public int PPEId { get; set; }
        public virtual PPE PPEs { get; set; }
        public int Office { get; set; }
        public int Field { get; set; }
        public string Status { get; set; }
      
    }
}

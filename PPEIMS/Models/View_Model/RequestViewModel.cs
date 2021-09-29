using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPEIMS.Models.View_Model
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string ReferenceNo { get; set; }
      
       
        public int[] detail_id { get; set; }
        public string[] no { get; set; }
        public string[] component { get; set; }
        public string[] qty { get; set; }
       
        
    }
}

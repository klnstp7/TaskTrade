using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
     public class FunctionColDto
    {
         public string id { get; set; }

         public string title { get; set; }

         public string coltype { get; set; }

         public string format { get; set; }

         public string className { get; set; }

         public List<OptionData> data {get;set;}
    }
 

    public class OptionData{
        public string value{get;set;}

        public string text{get;set;}

        public string isDefault{get;set;}
    }
}

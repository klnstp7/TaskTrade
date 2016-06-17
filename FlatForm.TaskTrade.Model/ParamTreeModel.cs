using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model
{
    public class ParamTreeModel
    {
        public long id { get; set; }

        public string text { get; set; }

        public string value { get; set; }

        public long ParentID { get; set; }

        public string state { get; set; }

        public List<ParamTreeModel> children { get; set; }

        public ParamTreeModel()
        {
            state = "closed";
        }
    }
}

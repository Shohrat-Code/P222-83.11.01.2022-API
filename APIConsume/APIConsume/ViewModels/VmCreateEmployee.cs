using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIConsume.ViewModels
{
    public class VmCreateEmployee
    {
        public List<VmPosition> Positions { get; set; }
        public VmEmployee Employee { get; set; }
    }
}

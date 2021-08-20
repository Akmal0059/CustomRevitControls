using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRevitControls.Items
{
    public interface IStackItem
    {
        double CalculatedWidth { get; set; }
        void CalculateWidth();
    }
}

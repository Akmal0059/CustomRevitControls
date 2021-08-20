using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRevitControls
{
    public class PulldownItem
    {
        public string IconPath { get; }
        public string Text { get; }

        public PulldownItem(string text, string iconPath)
        {
            Text = text;
            IconPath = iconPath;
        }
    }
}

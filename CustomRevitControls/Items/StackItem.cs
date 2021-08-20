using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRevitControls.Items
{
    public class StackItem
    {
        public string IconPath { get; }
        public string Text { get; }

        public StackItem(string text, string iconPath)
        {
            Text = text;
            IconPath = iconPath;
        }
    }
}

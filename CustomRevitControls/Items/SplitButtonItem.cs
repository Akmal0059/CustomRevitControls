using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CustomRevitControls
{
    public class SplitButtonItem
    {
        public string IconPath { get; }
        public string Text { get; }

        public SplitButtonItem(string text, string iconPath)
        {
            Text = text;
            IconPath = iconPath;
        }
    }
}

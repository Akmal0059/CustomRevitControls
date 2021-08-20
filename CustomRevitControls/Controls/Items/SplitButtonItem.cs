using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomRevitControls
{
    public class SplitButtonItem : RevitControl
    {
        public override string ControlName => GetType().Name;
        public override bool HasElements => false;

        public SplitButtonItem(object text, string iconPath)
        {
            Content = text;
            MainIcon = GetImageSource(iconPath);
        }
    }
}

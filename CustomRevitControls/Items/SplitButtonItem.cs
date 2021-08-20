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
        public override bool IsSelected { get; set; }
        public override object Content { get; set; }
        public override ImageSource MainIcon { get; set; }
        public override bool HasElements => false;
        public override IEnumerable Items { get; set; }

        public SplitButtonItem(object text, string iconPath)
        {
            Content = text;
            MainIcon = GetImageSource(iconPath);
        }
    }
}

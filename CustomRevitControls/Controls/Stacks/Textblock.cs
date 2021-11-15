using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomRevitControls
{
    public class Textblock : RevitControl
    {
        public override string ControlName => GetType().Name;
        public override bool HasElements => false;

        static Textblock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Textblock), new FrameworkPropertyMetadata(typeof(Textblock)));
        }

        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command);
        }
    }
}

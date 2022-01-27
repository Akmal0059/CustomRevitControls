using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CustomRevitControls
{
    public class Combobox : RevitControl
    {
        public override string ControlName => GetType().Name;
        public override bool HasElements => true;
        public Combobox(IEnumerable<string> comboItems)
        {
            ItemsSource = comboItems;

        }

        static Combobox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Combobox), new FrameworkPropertyMetadata(typeof(Combobox)));

        }

        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command, commands);
        }
    }
}

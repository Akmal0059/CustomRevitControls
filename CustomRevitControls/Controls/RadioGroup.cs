using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CustomRevitControls
{
    public class RadioGroup : RevitControl
    {
        public static DependencyProperty GroupNameProperty;
        public override string ControlName => GetType().Name;
        public override bool HasElements => true;

        public string GroupName
        {
            get => (string)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }
        public double Width => Items.Count * 90;


        static RadioGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioGroup), new FrameworkPropertyMetadata(typeof(RadioGroup)));
            GroupNameProperty = DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(RadioGroup));
        }

        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command, commands);
            Properties.Add(new PropertyItem(this, nameof(GroupName), new System.Windows.Controls.TextBox()));
        }
    }
}

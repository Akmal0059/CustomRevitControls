using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CustomRevitControls
{
    public class ToggleButton : RevitControl
    {
        public static DependencyProperty GroupNameProperty;
        public static DependencyProperty IsCheckedProperty;
        public override string ControlName => GetType().Name;
        public override bool HasElements => false;
        public string GroupName
        {
            get => (string)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }


        static ToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleButton), new FrameworkPropertyMetadata(typeof(ToggleButton)));
            GroupNameProperty = DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(ToggleButton));
            IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ToggleButton));
        }

        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command, commands);
            //Properties.Add(new PropertyItem(this, nameof(Items), new Button(), browseCommand: command));
        }
    }
}

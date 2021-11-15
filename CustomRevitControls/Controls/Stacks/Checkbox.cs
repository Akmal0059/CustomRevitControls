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
    public class Checkbox : RevitControl
    {
        public static DependencyProperty CheckboxWidthProperty;
        public static DependencyProperty CheckboxTextProperty;

        public override string ControlName => GetType().Name;
        public override bool HasElements => false;
        public string CheckboxText
        {
            get { return (string)base.GetValue(CheckboxTextProperty); }
            set { base.SetValue(CheckboxTextProperty, value); }
        }
        public double CheckboxWidth
        {
            get { return (double)base.GetValue(CheckboxWidthProperty); }
            set { base.SetValue(CheckboxWidthProperty, value); }
        }


        static Checkbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Checkbox), new FrameworkPropertyMetadata(typeof(Checkbox)));
            CheckboxWidthProperty = DependencyProperty.Register(nameof(CheckboxWidth), typeof(double), typeof(Checkbox));
            CheckboxTextProperty = DependencyProperty.Register(nameof(CheckboxText), typeof(string), typeof(Checkbox));
        }

        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command);
            Properties.Add(new PropertyItem(this, nameof(CheckboxText), new TextBox()));
            Properties.Add(new PropertyItem(this, nameof(CheckboxWidth), new TextBox()));
        }
    }
}

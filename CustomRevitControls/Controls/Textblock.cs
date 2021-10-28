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
        public static DependencyProperty TextblockWidthProperty;
        public static DependencyProperty TextblockTextProperty;

        public override string ControlName => GetType().Name;
        public override bool HasElements => false;
        public string TextblockText
        {
            get { return (string)base.GetValue(TextblockTextProperty); }
            set { base.SetValue(TextblockTextProperty, value); }
        }
        public double TextblockWidth
        {
            get { return (double)base.GetValue(TextblockWidthProperty); }
            set { base.SetValue(TextblockWidthProperty, value); }
        }

        static Textblock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Textblock), new FrameworkPropertyMetadata(typeof(Textblock)));
            TextblockWidthProperty = DependencyProperty.Register(nameof(TextblockWidth), typeof(double), typeof(Textblock));
            TextblockTextProperty = DependencyProperty.Register(nameof(TextblockText), typeof(string), typeof(Textblock));
        }

        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command);
            Properties.Add(new PropertyItem(this, nameof(TextblockText), new TextBox()));
            Properties.Add(new PropertyItem(this, nameof(TextblockWidth), new TextBox()));

        }
    }
}

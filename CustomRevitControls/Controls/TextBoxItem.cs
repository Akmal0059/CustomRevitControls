using CustomRevitControls.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Resources;

namespace CustomRevitControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomControls;assembly=CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:SplitItem/>
    ///
    /// </summary>
    public class TextBoxItem : RevitControl
    {
        public static DependencyProperty TextBoxWidthProperty;
        public static DependencyProperty TextBoxHintProperty;

        public override string ControlName => GetType().Name;
        public override bool HasElements => false;
        public double TextBoxWidth
        {
            get { return (double)base.GetValue(TextBoxWidthProperty); }
            set { base.SetValue(TextBoxWidthProperty, value); }
        }
        public string TextBoxHint
        {
            get { return (string)base.GetValue(TextBoxHintProperty); }
            set { base.SetValue(TextBoxHintProperty, value); }
        }
        static TextBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxItem), new FrameworkPropertyMetadata(typeof(TextBoxItem)));
            TextBoxWidthProperty = DependencyProperty.Register(nameof(TextBoxWidth), typeof(double), typeof(TextBoxItem));
            TextBoxHintProperty = DependencyProperty.Register(nameof(TextBoxHint), typeof(string), typeof(TextBoxItem));
        }

        protected override void AddSpecificResources(ResXResourceWriter rw)
        {
            rw.AddResource($"{CommandName}_Textbox_Width", TextBoxWidth);
            rw.AddResource($"{CommandName}_Textbox_HintText", TextBoxHint);
        }

        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command);
            //Properties.Add(new PropertyItem(this, nameof(EnterPressed), new ComboBox(), new List<object>() { "123", "111" }));
            Properties.Add(new PropertyItem(this, nameof(TextBoxWidth), new TextBox()));
            Properties.Add(new PropertyItem(this, nameof(TextBoxHint), new TextBox()));
        }
    }
}

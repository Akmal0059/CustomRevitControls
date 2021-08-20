using CustomRevitControls.Items;
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
    public class TextBoxItem : Control
    {
        public static DependencyProperty ContentProperty;
        public static DependencyProperty ItemsProperty;
        public static DependencyProperty CommandProperty;
        public static DependencyProperty CommandParameterProperty;
        public static DependencyProperty MainIconProperty;
        public static DependencyProperty TextBoxWidthProperty;


        public string ControlName => GetType().Name;
        public object Content
        {
            get { return base.GetValue(ContentProperty); }
            set { base.SetValue(ContentProperty, value); }
        }
        public ImageSource MainIcon
        {
            get { return (ImageSource)base.GetValue(MainIconProperty); }
            set { base.SetValue(MainIconProperty, value); }
        }
        public IEnumerable Items
        {
            get { return (IEnumerable)base.GetValue(ItemsProperty); }
            set { base.SetValue(ItemsProperty, value); }
        }
        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandProperty); }
            set { base.SetValue(CommandProperty, value); }
        }
        public object CommandParameter
        {
            get { return (object)base.GetValue(CommandParameterProperty); }
            set { base.SetValue(CommandParameterProperty, value); }
        }
        public double TextBoxWidth
        {
            get { return (double)base.GetValue(TextBoxWidthProperty); }
            set { base.SetValue(TextBoxWidthProperty, value); }
        }

        static TextBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxItem), new FrameworkPropertyMetadata(typeof(TextBoxItem)));
            ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(TextBoxItem));
            ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(TextBoxItem));
            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(TextBoxItem));
            CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(TextBoxItem));
            MainIconProperty = DependencyProperty.Register("MainIcon", typeof(ImageSource), typeof(TextBoxItem));
            TextBoxWidthProperty = DependencyProperty.Register("TextBoxWidth", typeof(double), typeof(TextBoxItem));

        }
    }
}

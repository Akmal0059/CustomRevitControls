using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    public class SplitItem : RevitControl
    {
        public static DependencyProperty ContentProperty;
        public static DependencyProperty ItemsProperty;
        public static DependencyProperty CommandProperty;
        public static DependencyProperty CommandParameterProperty;
        public static DependencyProperty CurrentIndexProperty;
        
        public override string ControlName => GetType().Name;
        public override bool HasElements => false;
        public override ImageSource MainIcon { get; set; }
        public override bool IsSelected { get; set; }
        public override object Content
        {
            get { return base.GetValue(ContentProperty); }
            set { base.SetValue(ContentProperty, value); }
        }
        public override IEnumerable Items
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
            get { return base.GetValue(CommandParameterProperty); }
            set { base.SetValue(CommandParameterProperty, value); }
        }
        public object CurrentIndex
        {
            get { return base.GetValue(CurrentIndexProperty); }
            set { base.SetValue(CurrentIndexProperty, value); }
        }

        static SplitItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitItem), new FrameworkPropertyMetadata(typeof(SplitItem)));
            ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(SplitItem));
            CurrentIndexProperty = DependencyProperty.Register("CurrentIndex", typeof(object), typeof(SplitItem));
            ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(SplitItem));
            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(SplitItem));
            CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(SplitItem));
        }
    }
}
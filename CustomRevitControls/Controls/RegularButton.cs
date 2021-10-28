using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using RevitAddinBase;
using RevitAddinBase.RevitControls;

namespace CustomRevitControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomRevitControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomRevitControls;assembly=CustomRevitControls"
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
    public class RegularButton : RevitControl
    {
        public static DependencyProperty CommandHandlerProperty;

        public override bool HasElements => false;
        public override string ControlName => GetType().Name;
        public string CommandHandler
        {
            get { return (string)base.GetValue(CommandHandlerProperty); }
            set { base.SetValue(CommandHandlerProperty, value); }
        }

        static RegularButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RegularButton), new FrameworkPropertyMetadata(typeof(RegularButton)));
            CommandHandlerProperty = DependencyProperty.Register(nameof(CommandHandler), typeof(string), typeof(RegularButton));
        }
        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command, commands);
        }
    }
}

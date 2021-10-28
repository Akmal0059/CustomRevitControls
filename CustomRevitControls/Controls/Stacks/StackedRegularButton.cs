using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using CustomRevitControls.Interfaces;
using System.Drawing;
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
    ///     <MyNamespace:PulldownButton/>
    ///
    /// </summary>
    public class StackedRegularButton : RevitControl, IStackItem
    {
        public static DependencyProperty CalculatedWidthProperty;

        public override bool HasElements => false;
        public override string ControlName => GetType().Name;
        public double CalculatedWidth 
        {
            get { return (double)base.GetValue(CalculatedWidthProperty); }
            set { base.SetValue(CalculatedWidthProperty, value); }
        }
        static StackedRegularButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StackedRegularButton), new FrameworkPropertyMetadata(typeof(StackedRegularButton)));
            CalculatedWidthProperty = DependencyProperty.Register(nameof(CalculatedWidth), typeof(double), typeof(StackedRegularButton));
        }

        public void CalculateWidth()
        {
            CalculatedWidth = 23 + 8;// icon + margin
            Font stringFont = new Font("Segoe UI", 16);

            // Measure string.
            SizeF stringSize = new SizeF();
            Graphics gr = Graphics.FromImage(new Bitmap(1, 1));
            stringSize = gr.MeasureString((string)Content, stringFont);
            CalculatedWidth += stringSize.Width;
        }

        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command, commands);
        }
    }
}

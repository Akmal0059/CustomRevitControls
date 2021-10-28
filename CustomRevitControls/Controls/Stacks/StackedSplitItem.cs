using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using CustomRevitControls.Interfaces;
using System.Drawing;
using System.Windows.Media;
using RevitAddinBase;
using RevitAddinBase.RevitControls;
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
    public class StackedSplitItem : RevitControl, IStackItem, ISplitItem
    {
        public static DependencyProperty CalculatedWidthProperty;
        public static DependencyProperty SelectedIndexProperty;

        public override bool HasElements => true;
        public override string ControlName => GetType().Name;
        public double CalculatedWidth
        {
            get { return (double)base.GetValue(CalculatedWidthProperty); }
            set { base.SetValue(CalculatedWidthProperty, value); }
        }
        public int? SelectedIndex
        {
            get => (int?)base.GetValue(SelectedIndexProperty);
            set
            {
                if (Int32.TryParse(value.ToString(), out int index))
                    base.SetValue(SelectedIndexProperty, index);
                else
                    base.SetValue(SelectedIndexProperty, null);
            }
        }

        static StackedSplitItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StackedSplitItem), new FrameworkPropertyMetadata(typeof(StackedSplitItem)));
            CalculatedWidthProperty = DependencyProperty.Register(nameof(CalculatedWidth), typeof(double), typeof(StackedSplitItem));
            SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int?), typeof(StackedSplitItem));
        }

        public void CalculateWidth()
        {
            CalculatedWidth = 25 + 8;// icon + margin
            Font stringFont = new Font("Segoe UI", 16);
            Graphics gr = Graphics.FromImage(new Bitmap(1, 1));

            // Measure string.
            double maxItemWidth = 0;
            if (SelectedIndex == null)
            {
                foreach (RevitControl item in Items)
                {
                    SizeF stringSize = new SizeF();
                    stringSize = gr.MeasureString((string)item.Content, stringFont);
                    if (maxItemWidth < stringSize.Width)
                        maxItemWidth = stringSize.Width;

                }
            }
            else
            {
                SizeF stringSize = new SizeF();
                stringSize = gr.MeasureString((string)Items[SelectedIndex.Value].Content, stringFont);
                if (maxItemWidth < stringSize.Width)
                    maxItemWidth = stringSize.Width;
            }
            CalculatedWidth += maxItemWidth;
        }
        protected override void AddSpecificResources(ResXResourceWriter rw)
        {
            rw.AddResource($"{CommandName}_SelectedIndex", SelectedIndex);
        }
        public override void SetProperties(ICommand command = null, List<string> commands = null)
        {
            SetCommonProperties(command, commands);
            Properties.Add(new PropertyItem(this, nameof(SelectedIndex), new TextBox(), true, SelectedIndexProperty));
        }
    }
}

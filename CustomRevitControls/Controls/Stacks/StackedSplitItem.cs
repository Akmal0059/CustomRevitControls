﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using CustomRevitControls.Items;
using System.Drawing;
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
    public class StackedSplitItem : RevitControl, IStackItem
    {
        public static DependencyProperty CommandProperty;
        public static DependencyProperty CommandParameterProperty;
        public static DependencyProperty CalculatedWidthProperty;

        public override bool HasElements => true;
        public override string ControlName => GetType().Name;
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
        public double CalculatedWidth
        {
            get { return (double)base.GetValue(CalculatedWidthProperty); }
            set { base.SetValue(CalculatedWidthProperty, value); }
        }

        static StackedSplitItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StackedSplitItem), new FrameworkPropertyMetadata(typeof(StackedSplitItem)));
            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(StackedSplitItem));
            CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(StackedSplitItem));
            CalculatedWidthProperty = DependencyProperty.Register("CalculatedWidth", typeof(double), typeof(StackedSplitItem));
        }

        public void CalculateWidth()
        {
            CalculatedWidth = 25 + 8;// icon + margin
            Font stringFont = new Font("Segoe UI", 16);
            Graphics gr = Graphics.FromImage(new Bitmap(1, 1));

            // Measure string.
            double maxItemWidth = 0;
            foreach (RevitControl item in Items)
            {
                SizeF stringSize = new SizeF();
                stringSize = gr.MeasureString((string)item.Content, stringFont);
                if (maxItemWidth < stringSize.Width)
                    maxItemWidth = stringSize.Width;

            }
            CalculatedWidth += maxItemWidth;
        }
    }
}

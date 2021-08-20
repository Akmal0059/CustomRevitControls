﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    public class PulldownButton : RevitControl
    {
        public static DependencyProperty ContentProperty;
        public static DependencyProperty ItemsProperty;
        public static DependencyProperty CommandProperty;
        public static DependencyProperty CommandParameterProperty;
        public static DependencyProperty MainIconProperty;


        public override bool HasElements => true;
        public override bool IsSelected { get; set; }
        public override string ControlName => GetType().Name;
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
            get { return (object)base.GetValue(CommandParameterProperty); }
            set { base.SetValue(CommandParameterProperty, value); }
        }
        public override ImageSource MainIcon
        {
            get { return (ImageSource)base.GetValue(MainIconProperty); }
            set { base.SetValue(MainIconProperty, value); }
        }

        static PulldownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PulldownButton), new FrameworkPropertyMetadata(typeof(PulldownButton)));
            ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(PulldownButton));
            MainIconProperty = DependencyProperty.Register("MainIcon", typeof(ImageSource), typeof(PulldownButton));
            ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(PulldownButton));
            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(PulldownButton));
            CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(PulldownButton));
        }
    }
}

using CustomRevitControls.Interfaces;
using RevitAddinBase;
using RevitAddinBase.RevitControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
    public class SplitItem : RevitControl, ISplitItem
    {
        public static DependencyProperty SelectedIndexProperty;
        public override string ControlName => GetType().Name;
        public override bool HasElements => true;
        public int? SelectedIndex
        {
            get => (int?)base.GetValue(SelectedIndexProperty);
            set { base.SetValue(SelectedIndexProperty, value); }
        }

        static SplitItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitItem), new FrameworkPropertyMetadata(typeof(SplitItem)));
            SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int?), typeof(SplitItem));
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
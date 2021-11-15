using CustomRevitControls.Commands;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CustomRevitControls
{
    public class PropertyItem
    {
        public string Name { get; private set; }
        public UIElement WpfControl { get; }
        public RevitControl RevitControl { get; private set; }

        private void BaseInit(RevitControl rControl, string name)
        {
            RevitControl = rControl;
            Name = name;
        }

        public PropertyItem(RevitControl rControl, string name, TextBox textBox, bool resetable = false, DependencyProperty prop = null)
        {
            BaseInit(rControl, name);
            textBox.SetBinding(TextBox.TextProperty, new Binding($"RevitControl.{name}"));
            if (!resetable)
            {
                WpfControl = textBox;
            }
            else
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(15) });

                textBox.Margin = new Thickness(0, 0, 2, 0);
                textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                Border border = new Border();
                border.Child = textBox;
                border.SetValue(Grid.ColumnProperty, 0);

                Button resetBtn = new Button();
                resetBtn.Width = 10;
                resetBtn.Height = 10;
                resetBtn.ToolTip = "Reset";
                resetBtn.HorizontalAlignment = HorizontalAlignment.Center;
                resetBtn.VerticalAlignment = VerticalAlignment.Center;
                resetBtn.Command = new ResetCommand(RevitControl, prop);
                resetBtn.SetValue(Grid.ColumnProperty, 1);

                grid.Children.Add(border);
                grid.Children.Add(resetBtn);
                WpfControl = grid;
            }
        }
        public PropertyItem(RevitControl rControl, string name, TextBox textBox, Button browseButton)
        {
            BaseInit(rControl, name);
            textBox.SetBinding(TextBox.TextProperty, new Binding($"RevitControl.{name}"));
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });

            textBox.Margin = new Thickness(0, 0, 2, 0);
            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            //textBox.SetValue(Grid.ColumnProperty, 0);
            Border border = new Border();
            border.Child = textBox;
            border.SetValue(Grid.ColumnProperty, 0);

            browseButton.Width = 30;
            browseButton.HorizontalAlignment = HorizontalAlignment.Right;
            browseButton.Content = ". . .";
            browseButton.Command = new SelectImageCommand(RevitControl);
            browseButton.SetValue(Grid.ColumnProperty, 1);

            grid.Children.Add(border);
            grid.Children.Add(browseButton);
            WpfControl = grid;
        }
        public PropertyItem(RevitControl rControl, string name, Button button, ICommand command)
        {
            BaseInit(rControl, name);
            button.Width = 40;
            button.Content = ". . .";
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.Command = command;
            WpfControl = button;
        }
        public PropertyItem(RevitControl rControl, string name, ComboBox comboBox, IEnumerable itemsSource)
        {
            BaseInit(rControl, name);
            comboBox.ItemsSource = itemsSource;
            WpfControl = comboBox;
        }
        public PropertyItem(RevitControl rControl, string name, CheckBox checkBox)
        {
            BaseInit(rControl, name);
            checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding($"RevitControl.{name}"));
            WpfControl = checkBox;
        }
    }
}
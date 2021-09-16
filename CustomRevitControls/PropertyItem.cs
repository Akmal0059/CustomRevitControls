using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public PropertyItem(RevitControl rControl, string name, TextBox textBox)
        {
            BaseInit(rControl, name);
            textBox.SetBinding(TextBox.TextProperty, new Binding($"RevitControl.{name}"));
            WpfControl = textBox;
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
    }
    public class SelectImageCommand : ICommand
    {
        private RevitControl revitControl;
        public SelectImageCommand(RevitControl rc) => revitControl = rc;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Image (*.png)|*.png";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                revitControl.IconPath = dialog.FileName;
                revitControl.Icon = GetImageSource(dialog.FileName);
            }
        }
        ImageSource GetImageSource(string path)
        {
            var bitmap = new Bitmap(path);
            var imageSource = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                imageSource.BeginInit();
                imageSource.StreamSource = memory;
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.EndInit();
            }
            return imageSource;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace CustomRevitControls.Commands
{
    public class DropdownCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ControlsContext viewModel;
        public DropdownCommand(ControlsContext vm)=> viewModel = vm;


        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var paramArray = parameter as object[];
            IEnumerable<RevitControl> items = (IEnumerable<RevitControl>)paramArray[0];
            FrameworkElement btn = (FrameworkElement)paramArray[1];
            RevitControl SplitItem = null;
            if (paramArray.Length == 3)
                SplitItem = (RevitControl)paramArray[2];

            //Window win = (Window)(parameter as object[])[1];
            Window win = Window.GetWindow(btn);

            Point startPoint = btn.TranslatePoint(new Point(0, 0), win);//.TransformToAncestor(win).Transform(new Point(0, 0));
            var p = win.PointToScreen(startPoint);
            Dropdown ui = new Dropdown(SplitItem);
            double maxWidth = 0;
            foreach(StackedRegularButton item in items)
            {
                double CalculatedWidth = 0;// 30 + 6;// icon + margin
                Font font = new Font("Segoe UI", 14);

                // Measure string.
                SizeF stringSize = new SizeF();
                
                Graphics gr = Graphics.FromImage(new Bitmap(1, 1));
                stringSize = gr.MeasureString((string)item.Content, font);
                CalculatedWidth += stringSize.Width;

                if (maxWidth < CalculatedWidth)
                    maxWidth = CalculatedWidth;
            }
            ui.Droplist.ItemsSource = items;
            ui.WindowStartupLocation = WindowStartupLocation.Manual;
            ui.Left = p.X;
            ui.Top = p.Y + btn.ActualHeight + 2;
            ui.Height = 42 * items.Count();
            ui.Width = maxWidth;
            ui.Show();
        }
    }
}

using System;
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
using System.Windows.Shapes;

namespace CustomRevitControls
{
    /// <summary>
    /// Interaction logic for Dropdown.xaml
    /// </summary>
    public partial class Dropdown : Window
    {
        ControlsContext viewModel;
        bool manualClosing = false;
        public Dropdown(ControlsContext viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();
        }
        protected override void OnDeactivated(EventArgs e)
        {
            if (!manualClosing)
            {
                base.OnDeactivated(e);
                Close();
            }
        }

        private void Droplist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)e.Source).SelectedItem is StackedRegularButton selectedSplit)
                viewModel.CurrentSplit = selectedSplit;
            manualClosing = true;
            Close();
        }
    }
}

using CustomRevitControls.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomRevitControls
{
    public class ControlsContext : INotifyPropertyChanged
    {
        RevitControl currentItem;

        public RevitControl Control { get; set; }
        public ICommand DropdownCommand { get; }
        public RevitControl CurrentItem 
        {
            get => currentItem;
            set
            {
                currentItem = value;
                OnPropertyChanged();
            }
        }

        public ControlsContext()
        {
            DropdownCommand = new DropdownCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

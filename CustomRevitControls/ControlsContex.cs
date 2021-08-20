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
        SplitButtonItem current;

        public object CurrentIndex { get; set; }
        public IEnumerable<SplitButtonItem> SplitItems { get; set; }
        public ICommand DropdownCommand { get; }
        public SplitButtonItem CurrentSplit
        {
            get => current;
            set
            {
                current = value;
                OnPropertyChanged();
            }
        }

        public ControlsContext()
        {
            //CurrentSplit = SplitItems.First();
            DropdownCommand = new DropdownCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

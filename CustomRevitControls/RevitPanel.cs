using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomRevitControls
{
    public class RevitPanel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<RevitControl> controls;
        string name, id, text;
        bool hasArrowBtn, hasSlideOut;

        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }
        public bool HasArrowButton
        {
            get => hasArrowBtn;
            set
            {
                hasArrowBtn = value;
                OnPropertyChanged();
            }
        }
        public bool HasSlideOut
        {
            get => hasSlideOut;
            set
            {
                hasSlideOut = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<RevitControl> Controls
        {
            get => controls;
            set
            {
                controls = value;
                HasSlideOut = controls.Any(x => x.IsSlideOut);
                OnPropertyChanged();
            }
        }
        //public string ClassName { get; set; }
        //public string AssemblyName { get; set; }
        //public string AvailabilityName { get; set; }

        public RevitPanel()
        {
            controls = new ObservableCollection<RevitControl>();
        }

        void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

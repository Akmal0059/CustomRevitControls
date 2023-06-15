using CustomRevitControls.Interfaces;
using RevitAddinBase.RevitContainers;
using RevitAddinBase.RevitControls;
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
    public class RevitTab : INotifyPropertyChanged
    {
        string name, id, title;

        public event PropertyChangedEventHandler PropertyChanged;

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
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<RevitPanel> Panels { get; set; }

        public RevitTab()
        {
            Panels = new ObservableCollection<RevitPanel>();
        }

        public RevitTab(RibbonTab ribbon, Dictionary<string, object> resDict, IEnumerable<string> commandNames)
        {
            Name = ribbon.Name;
            Title = ribbon.Title;
            List<RevitPanel> panels = new List<RevitPanel>();
            foreach (var p in ribbon.Panels)
            {
                RevitPanel panel = new RevitPanel();
                panel.Name = p.Name;
                panel.Id = p.Id;
                panel.Text = p.Text;
                foreach (var c in p.Items)
                {
                    var revitControl = RevitControl.GetRevitControl(c, resDict, commandNames: commandNames);

                    //SetPropeties(revitControl);
                    panel.Controls.Add(revitControl);
                }
                panels.Add(panel);
            }
            Panels = new ObservableCollection<RevitPanel>(panels);
        }

        public RibbonTab GetRibbonTab()
        {
            RibbonTab ribbonTab = new RibbonTab();
            ribbonTab.Name = Name;
            ribbonTab.Title = Title;

            foreach (var p in Panels)
            {
                var panel = new RibbonPanel();
                panel.Name = p.Name;
                panel.Id = p.Id;
                panel.Text = p.Text;
                panel.Items = new List<RibbonItemBase>();

                foreach (var item in p.Controls)
                {
                    var ribbon = item.GetRevitRibbon();
                    panel.Items.Add(ribbon);
                }
                ribbonTab.Panels.Add(panel);
            }

            return ribbonTab;
        }
        void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

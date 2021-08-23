using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CustomRevitControls
{
    public class PropertyItem
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public ICommand BrowseCommand { get; }
        public object SelectedValue { get; set; }
        public List<object> Items { get; set; }

        public PropertyItem(string name, Type type)
        {
            Name = name;
            Type = type;
            Items = new List<object>();
        }
    }
}

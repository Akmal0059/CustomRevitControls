using System.Collections;
using System.Windows.Media;

namespace CustomRevitControls
{
    public interface IRibbonBase
    {
        string ControlName { get; }
        bool IsSelected {  get; set;}
        object Content { get; set; }
        ImageSource MainIcon { get; set; }
        IEnumerable Items { get; set; }
    }
}

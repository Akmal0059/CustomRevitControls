using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomRevitControls
{
    public abstract class RevitControl : Control
    {
        public abstract string ControlName { get; }
        public abstract bool IsSelected {  get; set;}
        public abstract object Content { get; set; }
        public abstract ImageSource MainIcon { get; set; }
        public abstract bool HasElements { get; }
        public abstract IEnumerable Items { get; set; }

        protected ImageSource GetImageSource(string path)
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

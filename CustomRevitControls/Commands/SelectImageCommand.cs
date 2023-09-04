using System;
using System.Drawing;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomRevitControls
{
    public class SelectImageCommand : ICommand
    {
        private RevitControl revitControl;
        private string propName;
        public SelectImageCommand(RevitControl rc, string propertyName)
        {
            revitControl = rc;
            propName = propertyName;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            if (propName == nameof(revitControl.IconPath))
                dialog.Filter = "Image (*.png)|*.png";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (propName == nameof(revitControl.IconPath))
                    revitControl.IconPath = dialog.FileName;
                else if (propName == nameof(revitControl.TooltipImagePath))
                    revitControl.TooltipImagePath = dialog.FileName;
                else if (propName == nameof(revitControl.TooltipVideoPath))
                    revitControl.TooltipVideoPath = dialog.FileName;
            }
        }
        BitmapSource GetBitmapSource(string path)
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

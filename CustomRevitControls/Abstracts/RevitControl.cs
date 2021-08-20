using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomRevitControls
{
    public abstract class RevitControl : Control
    {
        public static DependencyProperty ContentProperty;
        public static DependencyProperty ItemsProperty;
        //public static DependencyProperty CommandProperty;
        //public static DependencyProperty CommandParameterProperty;
        public static DependencyProperty MainIconProperty;
        //public static DependencyProperty TextBoxWidthProperty;


        public abstract string ControlName { get; }
        public abstract bool HasElements { get; }
        public bool IsSelected { get; set; }
        public object Content
        {
            get { return base.GetValue(ContentProperty); }
            set { base.SetValue(ContentProperty, value); }
        }
        public ImageSource MainIcon
        {
            get { return (ImageSource)base.GetValue(MainIconProperty); }
            set { base.SetValue(MainIconProperty, value); }
        }
        public IEnumerable Items
        {
            get { return (IEnumerable)base.GetValue(ItemsProperty); }
            set { base.SetValue(ItemsProperty, value); }
        }

        static RevitControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RevitControl), new FrameworkPropertyMetadata(typeof(RevitControl)));
            ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(RevitControl));
            ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(RevitControl));
            //CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(RevitControl));
            //CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(RevitControl));
            MainIconProperty = DependencyProperty.Register("MainIcon", typeof(ImageSource), typeof(RevitControl));
            //TextBoxWidthProperty = DependencyProperty.Register("TextBoxWidth", typeof(double), typeof(RevitControl));

        }

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

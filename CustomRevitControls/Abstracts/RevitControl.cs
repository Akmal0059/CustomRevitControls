using CustomRevitControls.Interfaces;
using RevitAddinBase;
using RevitAddinBase.RevitControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomRevitControls
{
    public abstract class RevitControl : Control, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static DependencyProperty ContentProperty;
        public static DependencyProperty ItemsProperty;
        public static DependencyProperty IconProperty;
        public static DependencyProperty IconPathProperty;

        public abstract string ControlName { get; }
        public abstract bool HasElements { get; }
        public bool IsSelected { get; set; }
        public string CommandName { get; set;}
        public object Content
        {
            get { return base.GetValue(ContentProperty); }
            set { base.SetValue(ContentProperty, value); }
        }
        public string IconPath
        {
            get => (string)base.GetValue(IconPathProperty);
            set
            {
                base.SetValue(IconPathProperty, value);
                Icon = GetImageSource(value);
                OnPropertyChanged("IconPath");
            }
        }
        public ImageSource Icon
        {
            get => (ImageSource)base.GetValue(IconProperty);
            set
            {
                base.SetValue(IconProperty, value);
                OnPropertyChanged("Icon");
            }
        }
        public List<RevitControl> Items
        {
            get { return (List<RevitControl>)base.GetValue(ItemsProperty); }
            set { base.SetValue(ItemsProperty, value); }
        }
        public List<PropertyItem> Properties { get; set; } = new List<PropertyItem>();

        public RevitControl()
        {
            DataContext = new ControlsContext();
        }
        static RevitControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RevitControl), new FrameworkPropertyMetadata(typeof(RevitControl)));
            ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(RevitControl));
            ItemsProperty = DependencyProperty.Register("Items", typeof(List<RevitControl>), typeof(RevitControl));
            IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(RevitControl));
            IconPathProperty = DependencyProperty.Register("IconPath", typeof(string), typeof(RevitControl));
        }
        public abstract RibbonItemBase GetRevitRibbon();
        public abstract void SetProperties(ICommand command = null, List<string> commands = null);
        public void SetCommonProperties(ICommand command = null, List<string> commands = null)
        {
            if (!(this is StackButton) && !(this is StackedSplitItem))
                Properties.Add(new PropertyItem(this, "Content", new TextBox()));
            if(!(this is StackButton) &&  !(this is ISplitItem))
                Properties.Add(new PropertyItem(this, "IconPath", new TextBox(), new Button()));
            if (HasElements)
                Properties.Add(new PropertyItem(this, "Items", new Button(), command: command));
            Properties.Add(new PropertyItem(this, "CommandName",
                new System.Windows.Controls.ComboBox() { IsEditable = false}, commands));
        }

        public static RevitControl GetRevitControl(RibbonItemBase ribbonItem, bool isStacked = false)
        {
            if (ribbonItem is Textbox tb)
            {
                var textbox = new TextBoxItem();
                return textbox;
            }
            else if (ribbonItem is SplitButton sb)
            {
                if (!isStacked)
                {
                    SplitItem splitItem = new SplitItem();
                    splitItem.Items = sb.Items.Select(x => GetRevitControl(x, true)).ToList();
                    splitItem.Content = sb.Text;
                    splitItem.SelectedIndex = sb.SelectedIndex;
                    return splitItem;
                }
                else
                {
                    StackedSplitItem stackedSplitItem = new StackedSplitItem();
                    stackedSplitItem.Items = sb.Items.Select(x => GetRevitControl(x, true)).ToList();
                    stackedSplitItem.Content = sb.Text;
                    stackedSplitItem.SelectedIndex = sb.SelectedIndex;
                    return stackedSplitItem;
                }
            }
            else if (ribbonItem is StackItem si)
            {
                StackButton stackButton = new StackButton();
                stackButton.Items = si.Items.Select(x => GetRevitControl(x, true)).ToList();
                return stackButton;
            }
            else if (ribbonItem is PullButton pb)
            {
                if (!isStacked)
                {
                    PulldownButton pulldownButton = new PulldownButton();
                    pulldownButton.Content = pb.Text;
                    pulldownButton.IconPath = pb.IconPath;
                    pulldownButton.Items = pb.Items.Select(x => GetRevitControl(x, true)).ToList();
                    return pulldownButton;
                }
                else
                {
                    StackedPulldown stackedPulldownButton = new StackedPulldown();
                    stackedPulldownButton.Content = pb.Text;
                    stackedPulldownButton.IconPath = pb.IconPath;
                    stackedPulldownButton.Items = pb.Items.Select(x => GetRevitControl(x, true)).ToList();
                    return stackedPulldownButton;
                }
            }
            else if (ribbonItem is PushButton pushButton)
            {
                if (isStacked)
                {
                    var stackedBtn = new StackedRegularButton();
                    stackedBtn.Content = pushButton.Text;
                    stackedBtn.IconPath = pushButton.IconPath;
                    return stackedBtn;
                }
                else
                {
                    var btn = new RegularButton();
                    btn.Content = pushButton.Text;
                    btn.IconPath = pushButton.IconPath;
                    return btn;
                }
            }
            else
                throw new NotImplementedException();
        }

        protected ImageSource GetImageSource(string path)
        {
            if (path == null)
                return null;

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

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

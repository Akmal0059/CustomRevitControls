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
using System.Resources;
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
        public static DependencyProperty LongDescriptionProperty;
        public static DependencyProperty ShortDescriptionProperty;
        public static DependencyProperty ItemsProperty;
        public static DependencyProperty IconProperty;
        public static DependencyProperty DecriptionIconPathProperty;
        public static DependencyProperty IconPathProperty;
        public static DependencyProperty CommandNameProperty;
        public static DependencyProperty ContextualHelpProperty;
        public static DependencyProperty AvailabilityClassNameProperty;

        public abstract string ControlName { get; }
        public abstract bool HasElements { get; }
        public bool IsSelected { get; set; }
        public string CommandName
        {
            get { return (string)base.GetValue(CommandNameProperty); }
            set { base.SetValue(CommandNameProperty, value); }
        }
        public object Content
        {
            get { return base.GetValue(ContentProperty); }
            set { base.SetValue(ContentProperty, value); }
        }
        public string LongDescription
        {
            get { return (string)base.GetValue(LongDescriptionProperty); }
            set { base.SetValue(LongDescriptionProperty, value); }
        }
        public string ShortDescription
        {
            get { return (string)base.GetValue(ShortDescriptionProperty); }
            set { base.SetValue(ShortDescriptionProperty, value); }
        }
        public string ContextualHelp
        {
            get { return (string)base.GetValue(ContextualHelpProperty); }
            set { base.SetValue(ContextualHelpProperty, value); }
        }
        public string AvailabilityClassName
        {
            get { return (string)base.GetValue(AvailabilityClassNameProperty); }
            set { base.SetValue(AvailabilityClassNameProperty, value); }
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
        public string DescriptionIconPath
        {
            get => (string)base.GetValue(DecriptionIconPathProperty);
            set
            {
                base.SetValue(DecriptionIconPathProperty, value);
                DescriptionIcon = GetImageSource(value);
                OnPropertyChanged("DescriptionIconPath");
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
        public ImageSource DescriptionIcon
        {
            get => (ImageSource)base.GetValue(DecriptionIconPathProperty);
            set
            {
                base.SetValue(DecriptionIconPathProperty, value);
                OnPropertyChanged("DescriptionIcon");
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
            ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), typeof(RevitControl));
            LongDescriptionProperty = DependencyProperty.Register(nameof(LongDescription), typeof(string), typeof(RevitControl));
            ShortDescriptionProperty = DependencyProperty.Register(nameof(ShortDescription), typeof(string), typeof(RevitControl));
            ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(List<RevitControl>), typeof(RevitControl));
            IconProperty = DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(RevitControl));
            IconPathProperty = DependencyProperty.Register(nameof(IconPath), typeof(string), typeof(RevitControl));
            DecriptionIconPathProperty = DependencyProperty.Register(nameof(DescriptionIconPath), typeof(string), typeof(RevitControl));
            CommandNameProperty = DependencyProperty.Register(nameof(CommandName), typeof(string), typeof(RevitControl));
            ContextualHelpProperty = DependencyProperty.Register(nameof(ContextualHelp), typeof(string), typeof(RevitControl));
            AvailabilityClassNameProperty = DependencyProperty.Register(nameof(AvailabilityClassName), typeof(string), typeof(RevitControl));
        }

        public RibbonItemBase GetRevitRibbon()
        {
            RibbonItemBase ribbonItemBase = null;
            if (this is RegularButton rb || this is StackedRegularButton stRb)
            {
                ribbonItemBase = new PushButton();
                //(ribbonItemBase as PushButton).Text = (string)Content;
                //(ribbonItemBase as PushButton).IconPath = IconPath;
            }
            else if (this is PulldownButton pdb || this is StackedPulldown stPdb)
            {
                ribbonItemBase = new PullButton();
                //(ribbonItemBase as PullButton).Text = (string)Content;
                //(ribbonItemBase as PullButton).IconPath = IconPath;
                //(ribbonItemBase as PullButton).Items = new List<RibbonItemBase>();
                foreach (var item in Items)
                    (ribbonItemBase as PullButton).Items.Add(item.GetRevitRibbon());
            }
            else if (this is Separator sep)
            {
                return new RevitAddinBase.RevitControls.Separator();
            }
            else if (this is SplitItem si)
            {
                ribbonItemBase = new SplitButton();
                //(ribbonItemBase as SplitButton).Text = (string)Content;
                //(ribbonItemBase as SplitButton).SelectedIndex = si.SelectedIndex;
                (ribbonItemBase as SplitButton).Items = new List<RibbonItemBase>();
                foreach (var item in Items)
                    (ribbonItemBase as SplitButton).Items.Add(item.GetRevitRibbon());
            }
            else if(this is StackedSplitItem stSi)
            {
                ribbonItemBase = new SplitButton();
                //(ribbonItemBase as SplitButton).Text = (string)Content;
                //(ribbonItemBase as SplitButton).SelectedIndex = stSi.SelectedIndex;
                (ribbonItemBase as SplitButton).Items = new List<RibbonItemBase>();
                foreach (var item in Items)
                    (ribbonItemBase as SplitButton).Items.Add(item.GetRevitRibbon());
            }
            else if (this is TextBoxItem tbi)
            {
                ribbonItemBase = new RevitAddinBase.RevitControls.Textbox();
                //(ribbonItemBase as RevitAddinBase.RevitControls.Textbox).TextboxWidth = tbi.TextBoxWidth;
                //(ribbonItemBase as RevitAddinBase.RevitControls.Textbox).IconPath = IconPath;
                //(ribbonItemBase as RevitAddinBase.RevitControls.Textbox).HintText = tbi.TextBoxHint;
            }
            else if (this is StackButton sb)
            {
                ribbonItemBase = new StackItem();
                (ribbonItemBase as StackItem).Items = new List<RibbonItemBase>();
                foreach (var item in Items)
                    (ribbonItemBase as StackItem).Items.Add(item.GetRevitRibbon());
            }

            ribbonItemBase.CommandName = CommandName;
            ribbonItemBase.LongDescription = LongDescription;
            ribbonItemBase.ShortDescription = ShortDescription;
            ribbonItemBase.ContextualHelp = ContextualHelp;
            ribbonItemBase.AvailabilityClassName = ContextualHelp;
            return ribbonItemBase;
        }
        public static RevitControl GetRevitControl(RibbonItemBase ribbonItem, Dictionary<string, object> resources, bool isStacked = false)
        {
            RevitControl revitControl = null;
            string text = (string)resources[$"{ribbonItem.CommandName}_Button_caption"];
            Bitmap icon = (Bitmap)resources[$"{ribbonItem.CommandName}_Button_image"];
            ImageSource imageSource = GetImageSource(icon);
            if (ribbonItem is RevitAddinBase.RevitControls.Separator)
            {
                return new Separator()
                {
                    IsEnabled = false
                };
            }
            else if (ribbonItem is Textbox tb)
            {
                revitControl = new TextBoxItem();
                (revitControl as TextBoxItem).TextBoxWidth = (double)resources[$"{ribbonItem.CommandName}_Textbox_Width"];//tb.TextboxWidth;
                (revitControl as TextBoxItem).Icon = imageSource;// tb.IconPath;
                (revitControl as TextBoxItem).TextBoxHint = (string)resources[$"{ribbonItem.CommandName}_Textbox_HintText"];//tb.HintText;
                //return textbox;
            }
            else if (ribbonItem is SplitButton sb)
            {
                if (!isStacked)
                {
                    revitControl = new SplitItem();
                    (revitControl as SplitItem).Items = sb.Items.Select(x => GetRevitControl(x, resources, true)).ToList();
                    (revitControl as SplitItem).Content = text;//sb.Text;
                    (revitControl as SplitItem).SelectedIndex = (int)resources[$"{ribbonItem.CommandName}_SelectedIndex"];//sb.SelectedIndex;
                    //return splitItem;
                }
                else
                {
                    revitControl = new StackedSplitItem();
                    (revitControl as StackedSplitItem).Items = sb.Items.Select(x => GetRevitControl(x, resources, true)).ToList();
                    (revitControl as StackedSplitItem).Content = text;//sb.Text;
                    (revitControl as StackedSplitItem).SelectedIndex = (int)resources[$"{ribbonItem.CommandName}_SelectedIndex"];//sb.SelectedIndex;
                    //return stackedSplitItem;
                }
            }
            else if (ribbonItem is StackItem si)
            {
                revitControl = new StackButton();
                (revitControl as StackButton).Items = si.Items.Select(x => GetRevitControl(x, resources, true)).ToList();
                //return stackButton;
            }
            else if (ribbonItem is PullButton pb)
            {
                if (!isStacked)
                {
                    revitControl = new PulldownButton();
                    (revitControl as PulldownButton).Content = text;//pb.Text;
                    (revitControl as PulldownButton).Icon = imageSource;//pb.IconPath;
                    (revitControl as PulldownButton).Items = pb.Items.Select(x => GetRevitControl(x, resources, true)).ToList();
                    //return pulldownButton;
                }
                else
                {
                    revitControl = new StackedPulldown();
                    (revitControl as StackedPulldown).Content = text;// pb.Text;
                    (revitControl as StackedPulldown).Icon = imageSource;//pb.IconPath;
                    (revitControl as StackedPulldown).Items = pb.Items.Select(x => GetRevitControl(x, resources, true)).ToList();
                    //return stackedPulldownButton;
                }
            }
            else if (ribbonItem is PushButton pushButton)
            {
                if (isStacked)
                {
                    revitControl = new StackedRegularButton();
                    (revitControl as StackedRegularButton).Content = text;//pushButton.Text;
                    (revitControl as StackedRegularButton).Icon = imageSource;//pushButton.IconPath;
                    //return stackedBtn;
                }
                else
                {
                    revitControl = new RegularButton();
                    (revitControl as RegularButton).Content = text;//pushButton.Text;
                    (revitControl as RegularButton).Icon = imageSource;//pushButton.IconPath;
                    //return btn;
                }
            }
            else
                throw new NotImplementedException();

            revitControl.CommandName = ribbonItem.CommandName;
            //revitControl.LongDescription = (string)resources[$"{ribbonItem.CommandName}_Button_long_description"];//ribbonItem.LongDescription;
            //revitControl.ShortDescription = (string)resources[$"{ribbonItem.CommandName}_Button_tooltip_text"];//ribbonItem.ShortDescription;
            //revitControl.ContextualHelp = (string)resources[$"{ribbonItem.CommandName}_Help_file_name"];//ribbonItem.ContextualHelp;
            //revitControl.AvailabilityClassName = (string)resources[$"{ribbonItem.CommandName}_aviability_type"];//ribbonItem.AvailabilityClassName;
            return revitControl;
        }


        public abstract void SetProperties(ICommand command = null, List<string> commands = null);
        public void SetCommonProperties(ICommand command = null, List<string> commands = null)
        {
            if (!(this is StackButton) && !(this is StackedSplitItem))
            {
                TextBox multiLineTB = new TextBox();
                multiLineTB.AcceptsReturn = true;
                multiLineTB.TextWrapping = TextWrapping.Wrap;
                Properties.Add(new PropertyItem(this, nameof(Content), multiLineTB));
            }
            if (!(this is StackButton) && !(this is ISplitItem))
            {
                Properties.Add(new PropertyItem(this, nameof(IconPath), new TextBox(), new Button()));
            }
            if (HasElements)
            {
                Properties.Add(new PropertyItem(this, nameof(Items), new Button(), command: command));
            }
            Properties.Add(new PropertyItem(this, nameof(CommandName), new TextBox()));
            Properties.Add(new PropertyItem(this, nameof(LongDescription), new TextBox() { AcceptsReturn = true }));
            Properties.Add(new PropertyItem(this, nameof(ShortDescription), new TextBox() { AcceptsReturn = true }));
            Properties.Add(new PropertyItem(this, nameof(ContextualHelp), new TextBox()));
            Properties.Add(new PropertyItem(this, nameof(AvailabilityClassName), new TextBox()));
        }

        protected virtual void AddSpecificResources(ResXResourceWriter rw)
        {

        }
        public void AddStringResources(ResXResourceWriter rw)
        {
            if (!HasElements)
            {
                rw.AddResource($"{CommandName}_Button_caption", Content);
                rw.AddResource($"{CommandName}_Button_long_description", LongDescription);
                rw.AddResource($"{CommandName}_Button_tooltip_text", ShortDescription);
                rw.AddResource($"{CommandName}_Help_file_name", ContextualHelp);
                rw.AddResource($"{CommandName}_aviability_type", AvailabilityClassName);
                AddSpecificResources(rw);
            }
            else
            {
                foreach (RevitControl rc in Items)
                    rc.AddStringResources(rw);
            }
        }
        public void AddMediaResources(ResXResourceWriter rw)
        {
            if (!HasElements)
            {
                rw.AddResource($"{CommandName}_Button_image", new Bitmap(IconPath));
                //rw.AddResource($"{CommandName}_Button_tooltip_image", new Bitmap(DescriptionIconPath));
            }
            else
            {
                foreach (RevitControl rc in Items)
                    rc.AddMediaResources(rw);
            }
        }


        protected static ImageSource GetImageSource(string path)
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
        protected static ImageSource GetImageSource(Bitmap bitmap)
        {
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

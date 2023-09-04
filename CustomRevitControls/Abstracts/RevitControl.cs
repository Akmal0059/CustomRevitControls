using CustomRevitControls.Interfaces;
using RevitAddinBase;
using RevitAddinBase.RevitControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        #region DependencyProperties
        public static DependencyProperty ContentProperty;
        public static DependencyProperty LongDescriptionProperty;
        public static DependencyProperty ShortDescriptionProperty;
        public static DependencyProperty ItemsProperty;
        public static DependencyProperty IsSlideOutProperty;
        public static DependencyProperty HideTextProperty;
        public static DependencyProperty IconProperty;
        public static DependencyProperty IconPathProperty;
        public static DependencyProperty TooltipImageProperty;
        public static DependencyProperty TooltipImagePathProperty;
        public static DependencyProperty TooltipVideoProperty;
        public static DependencyProperty TooltipVideoPathProperty;
        public static DependencyProperty CommandNameProperty;
        public static DependencyProperty ContextualHelpProperty;
        public static DependencyProperty AvailabilityClassNameProperty;
        #endregion

        #region Properties
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
        public bool IsSlideOut
        {
            get { return (bool)base.GetValue(IsSlideOutProperty); }
            set { base.SetValue(IsSlideOutProperty, value); }
        }
        public bool HideText
        {
            get { return (bool)base.GetValue(HideTextProperty); }
            set { base.SetValue(HideTextProperty, value); }
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
        public BitmapSource Icon
        {
            get => (BitmapSource)base.GetValue(IconProperty);
            set
            {
                base.SetValue(IconProperty, value);
                OnPropertyChanged("Icon");
            }
        }
        public BitmapSource TooltipImage
        {
            get => (BitmapSource)base.GetValue(TooltipImageProperty);
            set
            {
                base.SetValue(TooltipImageProperty, value);
                OnPropertyChanged("TooltipImage");
            }
        }
        public string TooltipImagePath
        {
            get => (string)base.GetValue(TooltipImagePathProperty);
            set
            {
                base.SetValue(TooltipImagePathProperty, value);
                TooltipImage = GetImageSource(value);
                OnPropertyChanged("TooltipImagePath");
            }
        }
        public byte[] TooltipVideo
        {
            get => (byte[])base.GetValue(TooltipVideoProperty);
            set
            {
                base.SetValue(TooltipVideoProperty, value);
                OnPropertyChanged("TooltipVideo");
            }
        }
        public string TooltipVideoPath
        {
            get => (string)base.GetValue(TooltipVideoPathProperty);
            set
            {
                base.SetValue(TooltipVideoPathProperty, value);
                TooltipVideo = GetByteArray(value);
                OnPropertyChanged("TooltipVideoPath");
            }
        }
        public List<RevitControl> Items
        {
            get { return (List<RevitControl>)base.GetValue(ItemsProperty); }
            set { base.SetValue(ItemsProperty, value); }
        }
        public List<PropertyItem> Properties { get; set; } = new List<PropertyItem>();
        public IEnumerable<string> ItemsSource { get; set; }
        #endregion


        public RevitControl()
        {
            DataContext = new ControlsContext();
            Items = new List<RevitControl>();
        }
        static RevitControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RevitControl), new FrameworkPropertyMetadata(typeof(RevitControl)));
            ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), typeof(RevitControl));
            LongDescriptionProperty = DependencyProperty.Register(nameof(LongDescription), typeof(string), typeof(RevitControl));
            ShortDescriptionProperty = DependencyProperty.Register(nameof(ShortDescription), typeof(string), typeof(RevitControl));
            ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(List<RevitControl>), typeof(RevitControl));
            IsSlideOutProperty = DependencyProperty.Register(nameof(IsSlideOut), typeof(bool), typeof(RevitControl));
            HideTextProperty = DependencyProperty.Register(nameof(HideText), typeof(bool), typeof(RevitControl));
            IconProperty = DependencyProperty.Register(nameof(Icon), typeof(BitmapSource), typeof(RevitControl));
            IconPathProperty = DependencyProperty.Register(nameof(IconPath), typeof(string), typeof(RevitControl));
            TooltipImageProperty = DependencyProperty.Register(nameof(TooltipImage), typeof(BitmapSource), typeof(RevitControl));
            TooltipImagePathProperty = DependencyProperty.Register(nameof(TooltipImagePath), typeof(string), typeof(RevitControl));
            TooltipVideoProperty = DependencyProperty.Register(nameof(TooltipVideo), typeof(byte[]), typeof(RevitControl));
            TooltipVideoPathProperty = DependencyProperty.Register(nameof(TooltipVideoPath), typeof(string), typeof(RevitControl));
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
            else if (this is StackedSplitItem stSi)
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
                ribbonItemBase = new Textbox();
                (ribbonItemBase as Textbox).TextboxWidth = tbi.TextBoxWidth;
                (ribbonItemBase as Textbox).IconPath = IconPath;
                (ribbonItemBase as Textbox).HintText = tbi.TextBoxHint;
            }
            else if (this is StackButton sb)
            {
                ribbonItemBase = new StackItem();
                (ribbonItemBase as StackItem).Items = new List<RibbonItemBase>();
                foreach (var item in Items)
                    (ribbonItemBase as StackItem).Items.Add(item.GetRevitRibbon());
            }
            else if (this is Textblock tb)
            {
                ribbonItemBase = new RevitAddinBase.RevitControls.Label();
                (ribbonItemBase as RevitAddinBase.RevitControls.Label).Text = tb.Content.ToString();
            }
            else if (this is Combobox combo)
            {
                ribbonItemBase = new RevitAddinBase.RevitControls.ComboBox();
                foreach (var item in Items)
                    (ribbonItemBase as RevitAddinBase.RevitControls.ComboBox).Items.Add(item.GetRevitRibbon());
            }
            else if (this is ToggleButton toggle)
            {
                ribbonItemBase = new RevitAddinBase.RevitControls.ToggleButton();
            }
            else if (this is RadioGroup radioGroup)
            {
                ribbonItemBase = new RevitAddinBase.RevitControls.RadioGroup();
                foreach (var item in radioGroup.Items)
                {
                    (ribbonItemBase as RevitAddinBase.RevitControls.RadioGroup).Items.Add(item.GetRevitRibbon());
                }
            }

            ribbonItemBase.CommandName = CommandName;
            return ribbonItemBase;
        }

        public static RevitControl GetRevitControl(RibbonItemBase ribbonItem, Dictionary<string, object> resources, bool isStacked = false, IEnumerable<string> commandNames = null)
        {
            RevitControl revitControl = null;
            string text = (string)GetResx(resources, ribbonItem, "_Button_caption");
            Bitmap icon = (Bitmap)GetResx(resources, ribbonItem, "_Button_image");
            BitmapSource imageSource = GetImageSource(icon);

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
                (revitControl as TextBoxItem).TextBoxWidth = (double)GetResx(resources, ribbonItem, "_Textbox_Width");//tb.TextboxWidth;
                (revitControl as TextBoxItem).Icon = imageSource;// tb.IconPath;
                (revitControl as TextBoxItem).TextBoxHint = (string)GetResx(resources, ribbonItem, "_Textbox_HintText");//tb.HintText;
                //return textbox;
            }
            else if (ribbonItem is SplitButton sb)
            {
                if (!isStacked)
                {
                    revitControl = new SplitItem();
                    (revitControl as SplitItem).Items = sb.Items.Select(x => GetRevitControl(x, resources, true)).ToList();
                    (revitControl as SplitItem).Content = text;//sb.Text;
                    (revitControl as SplitItem).SelectedIndex = (int?)GetResx(resources, ribbonItem, "_SelectedIndex");//sb.SelectedIndex;
                    //return splitItem;
                }
                else
                {
                    revitControl = new StackedSplitItem();
                    (revitControl as StackedSplitItem).Items = sb.Items.Select(x => GetRevitControl(x, resources, true)).ToList();
                    (revitControl as StackedSplitItem).Content = text;//sb.Text;
                    (revitControl as StackedSplitItem).SelectedIndex = (int?)GetResx(resources, ribbonItem, "_SelectedIndex");//sb.SelectedIndex;
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
                    revitControl = new StackedRegularButton(commandNames);
                    (revitControl as StackedRegularButton).Content = text;//pushButton.Text;
                    (revitControl as StackedRegularButton).Icon = imageSource;//pushButton.IconPath;
                    //return stackedBtn;
                }
                else
                {
                    revitControl = new RegularButton(commandNames);
                    (revitControl as RegularButton).Content = text;//pushButton.Text;
                    (revitControl as RegularButton).Icon = imageSource;//pushButton.IconPath;
                    //return btn;
                }
            }
            else if (ribbonItem is RevitAddinBase.RevitControls.Label label)
            {
                revitControl = new Textblock();
                (revitControl as Textblock).Content = label.Text;
            }
            else if (ribbonItem is RevitAddinBase.RevitControls.ComboBox combo)
            {
                revitControl = new Combobox(null);
                (revitControl as Combobox).Items = combo.Items.Select(x => GetRevitControl(x, resources, true)).ToList();
            }
            else if (ribbonItem is RevitAddinBase.RevitControls.RadioGroup radio)
            {
                revitControl = new RadioGroup();
                (revitControl as RadioGroup).Items = radio.Items.Select(x => GetRevitControl(x, resources, false)).ToList();
                (revitControl as RadioGroup).GroupName = radio.GroupName;
            }
            else if (ribbonItem is RevitAddinBase.RevitControls.ToggleButton toggle)
            {
                revitControl = new ToggleButton();
                (revitControl as ToggleButton).GroupName = toggle.GroupName;
                (revitControl as ToggleButton).Content = toggle.Text;
                (revitControl as ToggleButton).Icon = imageSource;
            }
            else
                throw new NotImplementedException();
            revitControl.IsSlideOut = ribbonItem.IsSlideOut;
            revitControl.CommandName = ribbonItem.CommandName;
            revitControl.LongDescription = (string)GetResx(resources, ribbonItem, "_Button_long_description");
            revitControl.ShortDescription = (string)GetResx(resources, ribbonItem, "_Button_tooltip_text");
            revitControl.ContextualHelp = (string)GetResx(resources, ribbonItem, "_Help_file_name");
            revitControl.AvailabilityClassName = (string)GetResx(resources, ribbonItem, "_aviability_type");
            return revitControl;
        }


        public abstract void SetProperties(ICommand command = null, List<string> commands = null);
        public void SetCommonProperties(ICommand command = null, List<string> commands = null)
        {
            if (commands != null)
                ItemsSource = commands;
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
            Properties.Add(new PropertyItem(this, nameof(IsSlideOut), new CheckBox()));
            Properties.Add(new PropertyItem(this, nameof(CommandName), new System.Windows.Controls.ComboBox(), ItemsSource));
            Properties.Add(new PropertyItem(this, nameof(LongDescription), new TextBox() { AcceptsReturn = true }));
            Properties.Add(new PropertyItem(this, nameof(ShortDescription), new TextBox() { AcceptsReturn = true }));
            Properties.Add(new PropertyItem(this, nameof(TooltipImagePath), new TextBox(), new Button()));
            Properties.Add(new PropertyItem(this, nameof(TooltipVideoPath), new TextBox(), new Button()));
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
                if (this is PulldownButton || this is StackedPulldown)
                {
                    rw.AddResource($"{CommandName}_Button_caption", Content);
                    rw.AddResource($"{CommandName}_Button_long_description", LongDescription);
                    rw.AddResource($"{CommandName}_Button_tooltip_text", ShortDescription);
                    rw.AddResource($"{CommandName}_Help_file_name", ContextualHelp);
                    rw.AddResource($"{CommandName}_aviability_type", AvailabilityClassName);
                }
                else if (this is SplitItem || this is StackedSplitItem)
                {
                    rw.AddResource($"{CommandName}_Button_caption", Content);
                    rw.AddResource($"{CommandName}_SelectedIndex", (this as ISplitItem).SelectedIndex);
                }
                foreach (RevitControl rc in Items)
                    rc.AddStringResources(rw);
            }
        }
        public void AddMediaResources(ResXResourceWriter rw)
        {
            if (Icon != null)
                rw.AddResource($"{CommandName}_Button_image", GetBitmap(Icon));

            if (TooltipImage != null)
                rw.AddResource($"{CommandName}_Button_tooltip_image", GetBitmap(TooltipImage));

            if (TooltipVideo != null && TooltipVideo.Length != 0)
                rw.AddResource($"{CommandName}_Button_tooltip_video", TooltipVideo);

            if (HasElements)
            {
                foreach (RevitControl rc in Items)
                    rc.AddMediaResources(rw);
            }
        }

        Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(
                source.PixelWidth,
                source.PixelHeight,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            BitmapData data = bmp.LockBits(
              new Rectangle(System.Drawing.Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);

            return bmp;
        }
        protected static byte[] GetByteArray(string path)
        {
            if (path == null)
                return null;

            if(!File.Exists(path))
                return null;

            return File.ReadAllBytes(path);
        }
        protected static BitmapSource GetImageSource(string path)
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
        protected static BitmapSource GetImageSource(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

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
        protected static object GetResx(Dictionary<string, object> resources, RibbonItemBase ribbonItem, string key)
        {
            string dictKey = $"{ribbonItem.CommandName}{key}";
            if (resources.ContainsKey(dictKey))
                return resources[dictKey];
            else
                return null;
        }
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

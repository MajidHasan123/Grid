using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Quantux.MVVM;
using System.Xml;
using System.Collections.Generic;

namespace QuantuxGrid
{
    /// <summary>
    /// Interaction logic for CustomQuantuxGrid.xaml
    /// </summary>
    public partial class CustomQuantuxGrid : UserControl
    {
        ArrayList colName = new ArrayList();                                //Stores Sort Order info
        string SortImage = "";                                              //Image Show Sort state

        string oneToNineImg = "../Resources/OneToNineSortPrestine.png";     // initially set sorting image prestine state
        string aTozImg = "../Resources/AtoZnot.png";

        // Dependency Properties
        public static DependencyProperty FilterBarBackColorProperty;

        public static readonly DependencyProperty HeadingsItemsSourceProperty;

        public static readonly DependencyProperty GridItemSourceProperty;

        #region Properties

        private bool _isExpand = false;
        public bool IsExpand
        {
            get
            {
                return _isExpand;
            }

            set
            {
                _isExpand = value;
            }
        }
        #endregion

        //Static Constructor
        static CustomQuantuxGrid()
        {

            HeadingsItemsSourceProperty =
                DependencyProperty.Register("HeadingsItemsSource",
                typeof(IEnumerable), typeof(CustomQuantuxGrid));

            FilterBarBackColorProperty = DependencyProperty.Register(
               "FilterBarBackColor", typeof(Color), typeof(CustomQuantuxGrid),
                 new FrameworkPropertyMetadata(Colors.Red));

            GridItemSourceProperty = DependencyProperty.Register("GridItemSource",
                typeof(IEnumerable), typeof(CustomQuantuxGrid));
        }

        public CustomQuantuxGrid()
        {
            InitializeComponent();

            // By Defualt filter expandar is disabled
        }

        public Color FilterBarBackColor
        {
            get { return (Color)GetValue(FilterBarBackColorProperty); }
            set { SetValue(FilterBarBackColorProperty, value); }
        }

        public IEnumerable HeadingsItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(HeadingsItemsSourceProperty);
            }
            set
            {
                SetValue(HeadingsItemsSourceProperty, value);
            }
        }

        public IEnumerable GridItemSource
        {
            get
            {
                return (IEnumerable)GetValue(GridItemSourceProperty);
            }
            set
            {
                SetValue(GridItemSourceProperty, value);
            }
        }

        string FilterHeadingPnlName = "";

       /// <summary>
       /// Create UI of Grid Heading 
       /// Load data in Grid
       /// </summary>
        public void LoadData()
        {
            //Filter.ItemsSource = GridItemSource;
            InsturmentList.ItemsSource = GridItemSource;

            var x = GridItemSource.GetType();
            Type y = x.GetGenericArguments()[0];
            var props = y.GetProperties();

            int noOfCol = props.Count();

            DataTemplate dt = new DataTemplate();
            dt.DataType = y;

            FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            // List data
            for (var i = 0; i < noOfCol; i++)
            {
                FrameworkElementFactory lbl = new FrameworkElementFactory(typeof(Label));
                lbl.SetBinding(Label.ContentProperty, new Binding(props[i].Name));
                lbl.SetValue(ForegroundProperty, Brushes.Black);
                lbl.SetValue(WidthProperty, 130.0);
                lbl.SetValue(HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
                lbl.SetValue(BorderBrushProperty, Brushes.Black);
                spFactory.AppendChild(lbl);
            }

            string FilterSortImage = "";
            dt.VisualTree = spFactory;
            InsturmentList.ItemTemplate = dt;

            for (var i = 0; i < noOfCol; i++)
            {
                var propType = props[i].PropertyType;
                if (propType.FullName == "System.Int32" || propType.FullName == "System.Double")
                {
                    FilterSortImage = oneToNineImg;
                }
                else
                {
                    FilterSortImage = aTozImg;
                }

                // Grid Heading Section
                #region Xaml For Controls
                string gridHeadingxml =
                @"<DockPanel>
                <Border  BorderThickness=""0 0.5 1 0""
                         Background=""#14191E""
		                 BorderBrush=""#64788C"">
                <DockPanel LastChildFill=""True"" Width=""130"">
                    <Button Background=""#14191E"" 
                            DockPanel.Dock=""Left""
                            HorizontalAlignment=""Left""
                            Margin =""10 0 0 0""
                            BorderThickness =""0"">
                           		<Image Source=""../Resources/FilterIcon.png""
                           			   DockPanel.Dock=""Left""
                           			   Width=""16""
                           			   Height=""13""/>
                                 <Button.Style>
                                    <Style TargetType=""{x:Type Button}"">
                                        <Setter Property=""Template"">
                                            <Setter.Value>
                                                <ControlTemplate TargetType=""{x:Type Button}"">
                                                    <Border x:Name=""Overlay"">
                                                        <ContentPresenter/>
                                                    </Border>
                                                    <ControlTemplate.Triggers>
                                                        <Trigger Property=""IsEnabled"" Value=""false"">
                                                            <Setter TargetName=""Overlay"" 
                                                                    Property=""Background""     
                                                                    Value=""#14191E""/>
                                                        </Trigger>
                                                    </ControlTemplate.Triggers>
                                                </ControlTemplate>
                                            </Setter.Value>
                                        </Setter>
                                    </Style>
                                </Button.Style>
                           </Button>

                    <Button Background=""#14191E"" 
                            DockPanel.Dock=""Right""
                            HorizontalAlignment=""Right""
                            BorderThickness =""0"">
                           		<Image Source=""" + FilterSortImage + @"""
                           			   DockPanel.Dock=""Left""
                           			   Width=""16""
                           			   Height=""13""/>
                             <Button.Style>
                                    <Style TargetType=""{x:Type Button}"">
                                        <Setter Property=""Template"">
                                            <Setter.Value>
                                                <ControlTemplate TargetType=""{x:Type Button}"">
                                                    <Border x:Name=""Overlay"">
                                                        <ContentPresenter/>
                                                    </Border>
                                                    <ControlTemplate.Triggers>
                                                        <Trigger Property=""IsEnabled"" Value=""false"">
                                                            <Setter TargetName=""Overlay"" 
                                                                    Property=""Background""     
                                                                    Value=""#14191E""/>
                                                        </Trigger>
                                                    </ControlTemplate.Triggers>
                                                </ControlTemplate>
                                            </Setter.Value>
                                        </Setter>
                                    </Style>
                                </Button.Style>
                           </Button>
        
            <Label FontSize=""14""
					BorderThickness=""0""
                    FontFamily=""Calibri""
                    Background=""#14191E"" 
                    HorizontalAlignment=""Center""
                    Content=""" + props[i].Name + @""" 
                    Foreground=""#7E92A3"" />

           
                </DockPanel>
            </Border>
      </DockPanel>";

                #endregion

                ParserContext context = new ParserContext();
                context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

                using (Stream s = new MemoryStream(Encoding.UTF8.GetBytes(gridHeadingxml)))
                {
                    DockPanel pnl = XamlReader.Load(s, context) as DockPanel;

                    for (int k = 0; k < pnl.Children.Count; k++)
                    {
                        dynamic wp = pnl.Children[k];
                        dynamic C = wp.Child;

                        for (int j = 0; j < 1; j++)
                        {
                            dynamic b = C.Children[j];
                            Button btn = b as Button;
                            btn.Name = props[i].Name;
                            btn.Click += FiterBtnClick;
                        }

                        // Sort Image Button
                        for (int j = 1; j < 2; j++)
                        {
                            dynamic b = C.Children[1];
                            Button btn = b as Button;
                            btn.Name = props[i].Name;
                            btn.Click += SortBtnClicked;
                        }
                    }
                    GridHeadings.Children.Add(pnl);
                }
            }
        }

       /// <summary>
       /// Remove Filer and Sorting on data and remove UI
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void CrossBtnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            UIElement pnlToRemove;

            for (int i = 0; i < Filter.Children.Count; i++)
            {
                if (colName.Contains(btn.Name))
                {
                    pnlToRemove = Filter.Children[i];
                    Filter.Children.Remove(pnlToRemove);
                    colName.Remove(btn.Name);
                    break;
                }
            }
        }

        /// <summary>
        /// Show Filter Dialogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FiterBtnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }
        
        /// <summary>
        /// Apply Sort on Data and create UI 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortBtnClicked(object sender, RoutedEventArgs e)
        {
            Button btnClicked = sender as Button;

            #region XAML FOR CONTROLS

            // Filer Heading Section XAML
            string filterHedingXaml =
            @"<StackPanel HorizontalAlignment=""Left"">
                     <WrapPanel Margin=""5 5 0 0"">

                        <Button Height=""30"" 
                                VerticalAlignment=""Top""
                                Margin =""2""
                                FontSize=""15"" 
                                Foreground = ""Black""
                                Background = ""Transparent""
                                BorderThickness = ""0""
                                VerticalContentAlignment = ""Center"" >
                                 <Button.Content>
                                     <Image Source=""../Resources/crossICon.png""
                                        Width=""20"" 
                                		Height=""20""/>
                                 </Button.Content>
                            <Button.Style>
                                    <Style TargetType=""{x:Type Button}"">
                                        <Setter Property=""Template"">
                                            <Setter.Value>
                                                <ControlTemplate TargetType=""{x:Type Button}"">
                                                    <Border x:Name=""Overlay"">
                                                        <ContentPresenter/>
                                                    </Border>
                                                    <ControlTemplate.Triggers>
                                                        <Trigger Property=""IsEnabled"" Value=""false"">
                                                            <Setter TargetName=""Overlay"" 
                                                                    Property=""Background""     
                                                                    Value=""Transparent""/>
                                                        </Trigger>
                                                    </ControlTemplate.Triggers>
                                                </ControlTemplate>
                                            </Setter.Value>
                                        </Setter>
                                    </Style>
                                 </Button.Style>
                       </Button>

                        <StackPanel Orientation=""Horizontal""  
                                    Background=""#14191E""
                                    VerticalAlignment=""Top"">
                          <!--Filter Button-->
                          <Button Background=""#14191E""
                                  Margin=""10 0 0 0""
                                  BorderThickness =""0"">
                           		<Image Source=""../Resources/FilterIcon.png""
                           			   DockPanel.Dock=""Left""
                           			   Width=""16""
                           			   Height=""13""/>
                                    <Button.Style>
                                    <Style TargetType=""{x:Type Button}"">
                                        <Setter Property=""Template"">
                                            <Setter.Value>
                                                <ControlTemplate TargetType=""{x:Type Button}"">
                                                    <Border x:Name=""Overlay"">
                                                        <ContentPresenter/>
                                                    </Border>
                                                    <ControlTemplate.Triggers>
                                                        <Trigger Property=""IsEnabled"" Value=""false"">
                                                            <Setter TargetName=""Overlay"" 
                                                                    Property=""Background""     
                                                                    Value=""#14191E""/>
                                                        </Trigger>
                                                    </ControlTemplate.Triggers>
                                                </ControlTemplate>
                                            </Setter.Value>
                                        </Setter>
                                    </Style>
                                </Button.Style>
                           </Button>

                            <Label  Background=""#14191E""
                                    FontSize = ""12""
                                    BorderBrush = ""#64788C""
                                    BorderThickness = ""0""
                                    FontWeight = ""Bold""
                                    Height=""30""
                                    FontFamily = ""Calibri""
                                    HorizontalContentAlignment = ""Center""
                                    Content = """ + btnClicked.Name + @"""
                                    Foreground = ""#7E92A3""
                                    Width = ""200"" />
                          <!-- Sort Button-->
                          <Button Background=""#14191E"" 
                                  BorderThickness =""0"">
                           		<Image Source=""" + SortImage + @"""
                           			   DockPanel.Dock=""Left""
                           			   Width=""16""
                           			   Height=""13""/>
                                  <Button.Style>
                                    <Style TargetType=""{x:Type Button}"">
                                        <Setter Property=""Template"">
                                            <Setter.Value>
                                                <ControlTemplate TargetType=""{x:Type Button}"">
                                                    <Border x:Name=""Overlay"">
                                                        <ContentPresenter/>
                                                    </Border>
                                                    <ControlTemplate.Triggers>
                                                        <Trigger Property=""IsEnabled"" Value=""false"">
                                                            <Setter TargetName=""Overlay"" 
                                                                    Property=""Background""     
                                                                    Value=""#14191E""/>
                                                        </Trigger>
                                                    </ControlTemplate.Triggers>
                                                </ControlTemplate>
                                            </Setter.Value>
                                        </Setter>
                                    </Style>
                                </Button.Style>
                           </Button>
                            <Label Background=""White""
                                    BorderBrush=""#64788C""
                                    BorderThickness=""1""
                                    Margin=""2 0 0 0""
                                    HorizontalAlignment=""Left""
                                    Height=""30"" 
                                    MaxWidth=""929"">

                                <!--Symbol Filter Text-->
                                <TextBlock Text=""test"" 
                                            Foreground=""#66CCFF"" 
                                            TextTrimming=""CharacterEllipsis""
                                            VerticalAlignment=""Top""/>
                            </Label>
                        </StackPanel>
                     </WrapPanel>
                    </StackPanel>";

            #endregion

            var x = GridItemSource.GetType();
            Type y = x.GetGenericArguments()[0];
            var props = y.GetProperties();


            var propType = props[0].PropertyType;
            if (propType.FullName == "System.Int32" || propType.FullName == "System.Double")
            {
                if (SortImage == "../Resources/OneToNineSortPrestine.PNG")

                    SortImage = "../Resources/OneToNine.PNG";
                else
                    SortImage = "../Resources/OneToNineSortPrestine.PNG";
            }
            else
            {
                if (SortImage == "../Resources/AtoZnot.png")

                    SortImage = "../Resources/AZsort.png";
                else
                    SortImage = "../Resources/AtoZnot.png";
            }


            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

            using (Stream s = new MemoryStream(Encoding.UTF8.GetBytes(filterHedingXaml)))
            {
                StackPanel pnl = XamlReader.Load(s, context) as StackPanel;
                pnl.Name = "Panel" + btnClicked.Name;
                FilterHeadingPnlName = pnl.Name;

                for (int k = 0; k < pnl.Children.Count; k++)
                {
                    dynamic wp = pnl.Children[k];

                    for (int j = 0; j < 1; j++)
                    {
                        dynamic b = wp.Children[j];
                        Button btn1 = b as Button;
                        btn1.Name = btnClicked.Name;
                        btn1.Click += CrossBtnClick;
                    }
                }

                if (oneToNineImg == "../Resources/OneToNineSortPrestine.PNG")
                {
                    oneToNineImg = "../Resources/OneToNine.PNG";
                }
                else
                {
                    oneToNineImg = "../Resources/OneToNineSortPrestine.PNG";
                }
                // Cheeck Buttton Click Counter
                if (colName.Contains(btnClicked.Name))
                {
                    return;
                }
                colName.Add(btnClicked.Name);
                Filter.Children.Add(pnl);
            }
        }
    }
}

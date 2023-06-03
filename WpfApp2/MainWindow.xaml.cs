using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }

    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            ImageObjects = new List<ImageObject>();
            ImageObjects.Add(new ImageObject() { ImagePath = "1.jpeg", Title = "First Image", Description = "....." });
            ImageObjects.Add(new ImageObject() { ImagePath = "2.jpeg", Title = "Second Image", Description = "....." });
            ImageObjects.Add(new ImageObject() { ImagePath = "3.jpeg", Title = "Third Image", Description = "....." });
            ImageObjects.Add(new ImageObject() { ImagePath = "4.jpeg", Title = "Four Image", Description = "....." });
            ImageObjects.Add(new ImageObject() { ImagePath = "5.jpeg", Title = "Five Image", Description = "....." });
        }

        public List<ImageObject> ImageObjects { get; set; }
    }

    public class ImageObject
    {
        public string ImagePath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
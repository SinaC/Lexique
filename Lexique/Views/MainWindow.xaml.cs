using System.Windows;
using Lexique.ViewModels;

namespace Lexique.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel vm = new MainViewModel();

            DataContext = vm;
        }
    }
}

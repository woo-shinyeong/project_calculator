using System.Windows;
using System.Windows.Controls;

namespace project_calculator.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Page
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new CalculatorViewModel();
        }
    }
}
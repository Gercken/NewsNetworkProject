using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NewsNetworkProject.Infrastructure;

namespace NewsNetworkProject;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        CreateConnection conn = new CreateConnection();
        ConnectionLab.Content = conn.ConnectTest();
        
        
        foreach (var lab in conn.TestMethod())
        {
            GroupListStackPanel.Children.Add(lab);
        }
        
        HeadingListStackPanel.Children.Add(new Button() { Content = "Heading3ct" });
    }
}
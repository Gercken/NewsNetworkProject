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
    private CreateConnection _conn = new CreateConnection();
    
    public MainWindow()
    {
        InitializeComponent();
        
        HeadingListStackPanel.Children.Add(new Button() { Content = "Heading3ct" });
        
    }
    
    private async void BtnConnect_OnClick(object sender, RoutedEventArgs e)
    {
        await Task.Run(() =>
        {
            CreateConnection conn = new CreateConnection();
            _conn = conn;
            string connString = conn.ConnectToServer();
        
            if (connString.Equals("200") && conn.PassUserInfo().Equals("381") && conn.PassPasswordInfo().Equals("281"))
            {
                List<string> groupList = conn.TestMethod();

                Dispatcher.Invoke(() =>
                {
                    ConnectionLab.Content = "Connected";
                    foreach (var group in groupList)
                    {
                        Label lab = new Label { Content = group };
                        GroupListStackPanel.Children.Add(lab);
                    }
                });
            }
        });
    }
}
using System.Text;
using System.Windows;
using System.Windows.Controls;
using NewsNetworkProject.Infrastructure;
using NewsNetworkProject.Views;

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
                    for (int i = 0; i < 1000; i++)
                    {
                        Label lab = new Label { Content = groupList[i] };
                        GroupListView.Items.Add(lab);
                    }
                });
                
            }
        });
    }

    private void SetupView_OnClick(object sender, RoutedEventArgs e)
    {
        GettingHeadline gh = new GettingHeadline(_conn);
        
        foreach (var headline in gh.GettingHeadlineFromGroup(GroupListView.SelectedItem.ToString()))
        {
            HeadingListStackPanel.Children.Add( new Label { Content = headline.ToString() } );
        }
        
        SetupWindow setup = new SetupWindow();
        setup.Show();
    }
}
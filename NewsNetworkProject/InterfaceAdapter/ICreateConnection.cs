namespace NewsNetworkProject.InterfaceAdapter;

public interface ICreateConnection
{
    public string ConnectToServer();
    public string PassUserInfo();
    public string PassPasswordInfo();
    public void Disconnect();
}
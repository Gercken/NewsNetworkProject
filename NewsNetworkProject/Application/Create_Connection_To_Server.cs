using System.Net.Sockets;
using NewsNetworkProject.Infrastructure;
using NewsNetworkProject.InterfaceAdapter;


namespace NewsNetworkProject.Application;

public class Create_Connection_To_Server
{
    public void CreateConnectionToServer()
    {
        ICreateConnection conn = new CreateConnection();
        
    }
}
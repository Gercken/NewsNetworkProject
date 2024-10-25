using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using NewsNetworkProject.InterfaceAdapter;

namespace NewsNetworkProject.Infrastructure;

public class CreateConnection : ICreateConnection
{
    // Hardcoded for now
    private static string _url = "news.sunsite.dk";
    private static  int _port = 119;
    private string? _responseData;

    private NetworkStream? _stream;

    private StreamReader? _reader;
    private StreamWriter? _writer;
    
    public TcpClient? _client;

    public TcpClient Client
    {
        get
        {
            return _client;
        }
        set
        {
            _client = value;
        }
    }
    
    public string ConnectToServer()
    {
        try
        {
            // Initialize TcpClient and connect to the server
            _client = new TcpClient(_url, _port);
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream, Encoding.ASCII);
            _writer = new StreamWriter(_stream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true };

            // Read server's initial response
             _responseData = _reader.ReadLine();
            if (_responseData!.Substring(0, 3) != "200")
                return "Connection Failed: " + _responseData.Substring(3);
            
            
            
            // Connection succeeded
            return _responseData.Substring(0,3);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return "Connection Failed: " + ex.Message;
        }
    }

    public string PassUserInfo()
    {
        try
        {
            // Send AUTHINFO USER command
            _writer.WriteLine("AUTHINFO USER aleger01@easv365.dk");
            _responseData = _reader.ReadLine();
            if (!_responseData!.StartsWith("381"))
                return "Username not accepted: " + _responseData;
            return _responseData.Substring(0, 3);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error in PassUserInfo: " + e.Message);
            return "Error : " + e.Message;
        }
    }

    public string PassPasswordInfo()
    {
        try
        {
            // Send AUTHINFO PASS command
            _writer.WriteLine("AUTHINFO PASS acbf92");
            _responseData = _reader.ReadLine();
            if (!_responseData!.StartsWith("281"))
                return "Password not accepted: " + _responseData;
            return _responseData.Substring(0, 3);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error in PassPasswordInfo: " + e.Message);
            return "Error : " + e.Message;
        }
    }

    // Needs to be moved to another class since it has nothing to do with creating the connection
    public List<string> TestMethod()
    {
        if (_writer == null || _reader == null)
        {
            throw new InvalidOperationException("The connection has not been established. Please connect to the server first.");
        }
        
        List<string> groupList = new List<string>();
        try
        {
            // Send LIST command to the server
            _writer.WriteLine("LIST");
            _writer.Flush();

            // Read the server response
            string? responseData = _reader.ReadLine();
            if (responseData!.StartsWith("215"))
            {
                Debug.WriteLine("Newsgroup list:");

                // Read the newsgroups list until the server sends a "."
                string line;
                while ((line = _reader.ReadLine()) != null)
                {
                    if (line == ".")
                        break;
                    
                    groupList.Add(GetNewsGroup(line));
                }
            }
            else
            {
                Debug.WriteLine($"Unexpected response: {responseData}");
            }
        }
        catch (SocketException e)
        {
            Debug.WriteLine(e.Message);
        }
        catch (IOException e)
        {
            Debug.WriteLine(e.Message);
        }
        return groupList;
    }

    private string GetNewsGroup(string line)
    {
        string[] result = line.Split(" ");
        return result[0];
    }

    public void Disconnect()
    {
        // Close the connection
        if (_stream != null)
        {
            _writer.WriteLine("QUIT");
            _writer.Flush();
            _writer.Close();
            _reader.Close();
            _stream.Close();
            _client.Close();
            Debug.WriteLine("Disconnected from the server.");
        }
    }
}
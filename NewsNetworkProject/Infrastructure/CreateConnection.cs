using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using NewsNetworkProject.Entity;

namespace NewsNetworkProject.Infrastructure;

public class CreateConnection
{
    private static string url = "news.sunsite.dk";
    private static  int port = 119;

    private NetworkStream _stream;

    private StreamReader _reader;
    private StreamWriter _writer;
    
    private TcpClient client = new TcpClient(url, port);
    
    public string Connect()
    {
        try
        {
            
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes("");
            
            _stream = client.GetStream();
            _stream.Write(bytes, 0, bytes.Length);
            _reader = new StreamReader(_stream);
            bytes = new byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 something = _stream.Read(bytes, 0, bytes.Length);
            responseData = System.Text.Encoding.ASCII.GetString(bytes, 0, something);

            if (responseData.Substring(0,3) != "200") return "Connection Failed " + responseData.Substring(3);
            
            byte[] user = System.Text.Encoding.ASCII.GetBytes("AUTHINFO USER aleger01@easv365.dk\r\n");
            
            _stream = client.GetStream();
            _stream.Write(user, 0, user.Length);
            _reader = new StreamReader(_stream);
            bytes = new byte[256];
            
            // Read the first batch of the TcpServer response bytes.
            something = _stream.Read(bytes, 0, bytes.Length);
            responseData = System.Text.Encoding.ASCII.GetString(bytes, 0, something);
            
            MessageBox.Show(responseData.Substring(0,3));
            
            byte[] pass = System.Text.Encoding.ASCII.GetBytes("AUTHINFO pass acbf92\r\n");
            
            _stream = client.GetStream();
            _stream.Write(pass, 0, pass.Length);
            _reader = new StreamReader(_stream);
            bytes = new byte[256];
            
            // Read the first batch of the TcpServer response bytes.
            something = _stream.Read(bytes, 0, bytes.Length);
            responseData = System.Text.Encoding.ASCII.GetString(bytes, 0, something);
            
            MessageBox.Show(responseData.Substring(0,3));


            return "Connected";


        }
        catch (System.Reflection.TargetInvocationException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public string ConnectTest()
    {
        try
        {
            // Initialize TcpClient and connect to the server
            client = new TcpClient(url, port);
            _stream = client.GetStream();
            _reader = new StreamReader(_stream, Encoding.ASCII);
            _writer = new StreamWriter(_stream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true };

            // Read server's initial response
            string responseData = _reader.ReadLine();
            if (responseData.Substring(0, 3) != "200")
                return "Connection Failed: " + responseData.Substring(3);

            // Send AUTHINFO USER command
            _writer.WriteLine("AUTHINFO USER aleger01@easv365.dk");
            responseData = _reader.ReadLine();
            if (!responseData.StartsWith("381"))
                return "Username not accepted: " + responseData;

            // Send AUTHINFO PASS command
            _writer.WriteLine("AUTHINFO PASS acbf92");
            responseData = _reader.ReadLine();
            if (!responseData.StartsWith("281"))
                return "Password not accepted: " + responseData;

            // Connection and authentication succeeded
            return "Connected";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return "Connection Failed: " + ex.Message;
        }
    }

    public List<Label> TestMethod()
    {
        List<Label> lableList = new List<Label>();
        try
        {
            // Send LIST command to the server
            _writer.WriteLine("LIST");
            _writer.Flush();

            // Read the server response
            string responseData = _reader.ReadLine();
            if (responseData.StartsWith("215"))
            {
                Debug.WriteLine("Newsgroup list:");

                // Read the newsgroups list until the server sends a "."
                string line;
                while ((line = _reader.ReadLine()) != null)
                {
                    if (line == ".")
                        break;

                    // Create object that can be displayed
                    Label label = new Label();
                    label.Content = line;
                    lableList.Add(label);
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
        return lableList;
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
            client.Close();
            Debug.WriteLine("Disconnected from the server.");
        }
    }
}
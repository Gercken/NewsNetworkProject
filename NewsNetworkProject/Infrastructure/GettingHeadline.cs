using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace NewsNetworkProject.Infrastructure;

public class GettingHeadline
{
    private CreateConnection _conn { get; set; }
    
    private NetworkStream _stream;

    private StreamReader _reader;
    private StreamWriter _writer;
    
    public GettingHeadline(CreateConnection conn)
    {
        _conn = conn;
    }
    
    public List<string> GettingHeadlineFromGroup(string? group)
    {
        // Group
        List<string> headingList = new List<string>();
        try
        {
            _stream = _conn.Client.GetStream();
            _reader = new StreamReader(_stream, Encoding.ASCII);
            _writer = new StreamWriter(_stream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true };
            
            _writer.WriteLine("GROUP " + group);
            string? responseData = _reader.ReadLine();
            if (responseData!.StartsWith("211"))
            {

                // Read the newsgroups list until the server sends a "."
                string line;
                while ((line = _reader.ReadLine()) != null)
                {
                    if (line == ".")
                        break;
                    
                    headingList.Add(line);
                }
            }
            else
            {
                Debug.WriteLine($"Unexpected response: {responseData}");
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return headingList;
    }
}
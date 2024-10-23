using System;
using JetBrains.Annotations;
using NewsNetworkProject.Infrastructure;
using Xunit;

namespace NewsNetworkProject.Tests.Infrastructure;

[TestSubject(typeof(CreateConnection))]
public class CreateConnectionTest : IDisposable
{
    // Setup
    private CreateConnection _connection = new CreateConnection();
    private string _actual;
    

    [Fact]
    public void ConnectToServerGood()
    {
        _actual = _connection.ConnectToServer();
        
        Assert.Equal("200", _actual);
        
    }

    [Fact]
    public void ConnetToServerBad()
    {
        _actual = _connection.ConnectToServer();
        
        Assert.NotEqual("381", _actual);
        
    }

    [Fact]
    public void PassUserInfoGood()
    {
        _actual = _connection.PassUserInfo();
        
        Assert.Equal("381", _actual);
        
    }
    
    [Fact]
    public void PassUserInfoBad()
    {
        _actual = _connection.PassUserInfo();
        
        Assert.NotEqual("281", _actual);
        
    }
    
    [Fact]
    public void PassPasswordGood()
    {
        _actual = _connection.PassPasswordInfo();
        
        Assert.Equal("281", _actual);
        
    }

    [Fact]
    public void PassPasswordBad()
    {
        _actual = _connection.PassPasswordInfo();
        
        Assert.NotEqual("181", _actual);
        
    }
    

    // TearDown
    public void Dispose()
    {
        _connection.Disconnect();
        
    }
}
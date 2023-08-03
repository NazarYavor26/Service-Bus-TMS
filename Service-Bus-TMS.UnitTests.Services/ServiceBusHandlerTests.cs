using NUnit.Framework;
using Service_Bus_TMS.BLL.Services;
using Service_Bus_TMS.DAL.Entities;

namespace Service_Bus_TMS.UnitTests.Services;

[TestFixture]
public class ServiceBusHandlerTests
{
    private ServiceBusHandler _serviceBusHandler;

    [SetUp]
    public void Setup()
    {
        _serviceBusHandler = new ServiceBusHandler("testQueue");
    }

    [Test]
    public void SendMessage_ShouldSendMessage()
    {
        // Arrange
        var task = new Task();

        // Act & Assert
        Assert.DoesNotThrow(() => _serviceBusHandler.SendMessage(task));
    }
    
    [Test]
    public void SendMessage_NullTask_DoesNotThrowException()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => _serviceBusHandler.SendMessage(null));
    }
    
    [Test]
    public void SendAndReceiveMessage_Should_SendAndReceive()
    {
        // Arrange

        var serviceBusHandler = new ServiceBusHandler("sendAndReceiveQueue");
        var task = new Task { TaskID = 1, TaskName = "Test Task" };

        // Act
        serviceBusHandler.SendMessage(task);
        var receivedTasks = serviceBusHandler.ReceiveMessage();

        // Assert
        Assert.That(receivedTasks, Has.Count.EqualTo(1));
    }

    [Test]
    public void ReceiveMessage_NoMessages_ReturnsEmptyList()
    {
        // Arrange
        var serviceBusHandler = new ServiceBusHandler();

        // Act
        var receivedTasks = serviceBusHandler.ReceiveMessage();

        // Assert
        Assert.That(receivedTasks, Is.Empty);
    }
    
    [Test]
    public void ReceiveMessage_ReceiveSingleMessage_ReturnsListWithOneTask()
    {
        // Arrange
        var task = new Task();
        _serviceBusHandler.SendMessage(task);

        // Act
        var receivedTasks = _serviceBusHandler.ReceiveMessage();

        // Assert
        Assert.That(receivedTasks, Has.Count.EqualTo(1));
    }
    
    [Test]
    public void ReceiveMessage_ReceiveMultipleMessages_ReturnsListWithCorrectCount()
    {
        // Arrange
        _serviceBusHandler = new ServiceBusHandler("receiveMultipleMessagesQueue");
        const int taskCount = 5;
        
        for (int i = 0; i < taskCount; i++)
        {
            _serviceBusHandler.SendMessage(new Task());
        }

        // Act
        var receivedTasks = _serviceBusHandler.ReceiveMessage();

        // Assert
        Assert.That(receivedTasks, Has.Count.EqualTo(taskCount));
    }
}
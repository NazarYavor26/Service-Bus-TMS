using System.Collections.Generic;
using System.Text;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service_Bus_TMS.BLL.Services;
using Service_Bus_TMS.DAL.Entities;

namespace Service_Bus_TMS.UnitTests.Services;

[TestFixture]
public class ServiceBusHandlerTests
{
    private Mock<IModel> _mockChannel;
    private Mock<IConnection> _mockConnection;
    private ServiceBusHandler _serviceBusHandler;

    [SetUp]
    public void Setup()
    {
        _mockChannel = new Mock<IModel>();
        _mockConnection = new Mock<IConnection>();
        _mockConnection.Setup(c => c.CreateModel()).Returns(_mockChannel.Object);

        var mockConnectionFactory = new Mock<IConnectionFactory>();
        mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockConnection.Object);

        _serviceBusHandler = new ServiceBusHandler();
    }

    [Test]
    public void SendMessage_ValidTask_SendsMessage()
    {
        // Arrange
        var task = new Task();

        // Act
        _serviceBusHandler.SendMessage(task);

        // Assert
        _mockChannel.Verify(c => c.BasicPublish(
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            It.IsAny<bool>(), 
            It.IsAny<IBasicProperties>(), 
            It.IsAny<byte[]>()), 
            Times.Once);
    }

    [Test]
    public void ReceiveMessage_NoMessages_ReturnsEmptyList()
    {
        // Act
        var result = _serviceBusHandler.ReceiveMessage();

        // Assert
        Assert.IsEmpty(result);
    }

    [Test]
    public void ReceiveMessage_WithMessages_ReturnsListOfTasks()
    {
        // Arrange
        var tasks = new List<Task>
        {
            new(),
            new()
        };

        var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(tasks[0]));
        var eventArgs = new BasicDeliverEventArgs("tag", 1, false, "exchange", "routingKey", null, messageBytes);

        _mockChannel.Setup(c => c.BasicConsume(
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<IBasicConsumer>()))
            .Callback<string, bool, string, IBasicConsumer>((queue, autoAck, consumerTag, consumer) =>
            {
                consumer.HandleBasicDeliver("consumerTag", 1, false, "exchange", "routingKey", null, messageBytes);
            });

        // Act
        var result = _serviceBusHandler.ReceiveMessage();

        // Assert
        Assert.That(result.Count, Is.EqualTo(tasks.Count));
    }
}